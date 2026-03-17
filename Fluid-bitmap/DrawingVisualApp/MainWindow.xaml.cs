using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timerDraw;
        public static Random rnd = new Random();
        public static int width, height;

        Fluid fluid;
        public static int SCALE = 4;
        double t = 0;
        public static int N = 128;
        public static int iter = 8;

        // Для быстрого рендеринга
        WriteableBitmap bitmap;
        byte[] pixelData;
        int stride;

        // Кэшированные значения
        private int? cxCache;
        private int? cyCache;
        private float[] densityCache;

        // Предварительно вычисленные массивы для пакетного добавления
        private int[] batchX = new int[25];
        private int[] batchY = new int[25];
        private float[] batchAmounts = new float[25];

        // Для измерения производительности
        private DateTime lastFrameTime;
        private int frameCount;
        private const int targetFPS = 50;

        public MainWindow()
        {
            InitializeComponent();

            width = (int)g.Width;
            height = (int)g.Height;

            // Инициализация WriteableBitmap
            stride = width * 4;
            pixelData = new byte[height * stride];
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);

            // Создаем Image и добавляем в Grid
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = bitmap;
            g.Children.Add(img);

            // Оптимизированный таймер
            timerDraw = new System.Windows.Threading.DispatcherTimer();
            timerDraw.Tick += new EventHandler(timerDrawTick);
            timerDraw.Interval = TimeSpan.FromMilliseconds(1000.0 / targetFPS);

            fluid = new Fluid(0.0001f, 0.0001f, 0.1f);
            densityCache = new float[N * N];

            lastFrameTime = DateTime.Now;

            // Инициализация кэша
            cxCache = null;
            cyCache = null;

            timerDraw.Start();
        }

        private void timerDrawTick(object sender, EventArgs e)
        {
            // Контроль FPS
            var now = DateTime.Now;
            var elapsed = now - lastFrameTime;
            if (elapsed.TotalMilliseconds < 1000.0 / targetFPS)
                return;

            lastFrameTime = now;
            Drawing();
        }

        private void Drawing()
        {
            // Кэшируем значения - без coalescing assignment
            if (cxCache == null)
            {
                cxCache = (int)(0.5 * width / SCALE);
            }
            if (cyCache == null)
            {
                cyCache = (int)(0.5 * height / SCALE);
            }

            int cx = cxCache.Value;
            int cy = cyCache.Value;

            // Пакетное добавление плотности
            int index = 0;
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    batchX[index] = cx + i;
                    batchY[index] = cy + j;
                    batchAmounts[index] = rnd.Next(30, 100);
                    index++;
                }
            }
            fluid.addDensityBatch(batchX, batchY, batchAmounts, index);

            // Оптимизированное добавление скорости
            double localT = t;
            for (int i = 0; i < 3; i++)
            {
                double angle = Graphics.Noise(localT, 0) * Math.PI * 6;
                float vx = (float)(Math.Cos(angle) * 0.5);
                float vy = (float)(Math.Sin(angle) * 0.5);
                localT += 0.01;
                fluid.addVelocity(cx, cy, vx, vy);
            }
            t = localT;

            // Шаг симуляции
            fluid.Step();

            // Быстрый рендеринг
            //RenderFluidOptimized(); 
            RenderFluidUnsafe();
        }

        private void RenderFluidOptimized()
        {
            // Быстрая очистка
            Array.Clear(pixelData, 0, pixelData.Length);

            int n = N;
            int scale = SCALE;
            int heightLimit = height;
            int widthLimit = width;
            int strideLocal = stride;
            byte[] localPixelData = pixelData;

            // Получаем весь массив плотности за один раз
            float[] densityArray = fluid.GetDensityArray();

            // Используем Parallel.For для многопоточной обработки
            Parallel.For(0, n, j =>
            {
                int screenYBase = j * scale;
                if (screenYBase >= heightLimit) return;

                int maxDY = Math.Min(scale, heightLimit - screenYBase);
                int rowOffset = j * n;

                for (int i = 0; i < n; i++)
                {
                    float density = densityArray[rowOffset + i];

                    // Быстрое преобразование в байт
                    int dInt = (int)density;
                    byte d = (byte)(dInt > 255 ? 255 : (dInt < 0 ? 0 : dInt));

                    int screenXBase = i * scale;
                    if (screenXBase >= widthLimit) continue;

                    int maxDX = Math.Min(scale, widthLimit - screenXBase);

                    // Оптимизированная отрисовка квадрата
                    int baseY = screenYBase;
                    for (int dy = 0; dy < maxDY; dy++)
                    {
                        int y = baseY + dy;
                        int rowStart = y * strideLocal + screenXBase * 4;

                        // Разворачиваем внутренний цикл для маленьких SCALE
                        if (scale <= 4)
                        {
                            // Быстрая версия для маленьких масштабов
                            int endIdx = rowStart + maxDX * 4;
                            for (int idx = rowStart; idx < endIdx; idx += 4)
                            {
                                localPixelData[idx] = d;      // Blue
                                localPixelData[idx + 1] = d;  // Green
                                localPixelData[idx + 2] = d;  // Red
                                localPixelData[idx + 3] = 255;
                            }
                        }
                        else
                        {
                            // Стандартная версия для больших масштабов
                            for (int dx = 0; dx < maxDX; dx++)
                            {
                                int idx = rowStart + dx * 4;
                                localPixelData[idx] = d;
                                localPixelData[idx + 1] = d;
                                localPixelData[idx + 2] = d;
                                localPixelData[idx + 3] = 255;
                            }
                        }
                    }
                }
            });

            // Асинхронное обновление bitmap
            bitmap.Dispatcher.BeginInvoke(new Action(() =>
            {
                bitmap.WritePixels(new Int32Rect(0, 0, width, height),
                                  localPixelData, strideLocal, 0);
            }));
        }

        // метод рендеринга с использованием unsafe
        private unsafe void RenderFluidUnsafe()
        {
            fixed (byte* pPixelData = pixelData)
            {
                int n = N;
                int scale = SCALE;
                float[] densityArray = fluid.GetDensityArray();

                for (int j = 0; j < n; j++)
                {
                    int screenY = j * scale;
                    if (screenY >= height) continue;

                    int rowOffset = j * n;

                    for (int i = 0; i < n; i++)
                    {
                        float density = densityArray[rowOffset + i];
                        byte d = (byte)(density > 255 ? 255 : (density < 0 ? 0 : density));

                        int screenX = i * scale;
                        if (screenX >= width) continue;

                        int maxDX = Math.Min(scale, width - screenX);
                        int maxDY = Math.Min(scale, height - screenY);

                        for (int dy = 0; dy < maxDY; dy++)
                        {
                            int y = screenY + dy;
                            byte* row = pPixelData + y * stride + screenX * 4;

                            for (int dx = 0; dx < maxDX; dx++)
                            {
                                byte* pixel = row + dx * 4;
                                pixel[0] = d; // Blue
                                pixel[1] = d; // Green
                                pixel[2] = d; // Red
                                pixel[3] = 255; // Alpha
                            }
                        }
                    }
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelData, stride, 0);
        }

        // Освобождение ресурсов
        protected override void OnClosed(EventArgs e)
        {
            if (fluid != null)
            {
                fluid.Dispose();
            }
            base.OnClosed(e);
        }
    }
}