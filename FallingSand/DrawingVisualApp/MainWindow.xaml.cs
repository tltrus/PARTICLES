using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisualApp
{
    // Based on : "#180 — Falling Sand" https://thecodingtrain.com/challenges/180-falling-sand

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public Random rnd = new Random();
        public int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        Point mouse = new Point();
        float[,] grid;
        int cols, rows, w;
        float hueValue = 1;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;



            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);

            Init();
        }

        private void Init()
        {
            w = 5;
            cols = width / w;
            rows = height / w;
            grid = new float[rows, cols];

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                float[,] nextGrid = new float[rows, cols];

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        float state = grid[i, j];

                        if (state > 0)
                        {
                            // draw sand
                            int y = i * w;
                            int x = j * w;

                            Rect rect = new Rect()
                            {
                                X = x,
                                Y = y,
                                Width = w,
                                Height = w
                            };
                            var brush = HsvToRgb(state, 1f, 1f);
                            dc.DrawRectangle(new SolidColorBrush(brush), null, rect);

                            // calculate
                            if (i == rows - 1)
                            {
                                nextGrid[i, j] = state;
                                continue;
                            }

                            // What is below?
                            float below = grid[i + 1, j];


                            // Randomly fall left or right
                            int dir = 1;
                            if (rnd.NextDouble() < 0.5)
                            {
                                dir *= -1;
                            }

                            // Check below left or right
                            float belowA = -1;
                            float belowB = -1;
                            if (WithinCols(j + dir))
                            {
                                belowA = grid[i + 1, j + dir];
                            }
                            if (WithinCols(j - dir))
                            {
                                belowB = grid[i + 1, j - dir];
                            }

                            // Can it fall below or left or right?
                            if (below == 0)
                            {
                                nextGrid[i + 1, j] = state;
                            }
                            else if (belowA == 0)
                            {
                                nextGrid[i + 1, j + dir] = state;
                            }
                            else if (belowB == 0)
                            {
                                nextGrid[i + 1, j - dir] = state;
                            
                            // Stay put!
                            }
                            else
                            {
                                nextGrid[i, j] = state;
                            }
                        }
                    }

                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        grid[i, j] = nextGrid[i, j];
                    }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        // Check if a row is within the bounds
        private bool WithinCols(int i)
        {
            return i >= 0 && i <= cols - 1;
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mouse.X = e.GetPosition(g).X;
                mouse.Y = e.GetPosition(g).Y;

                int x = (int)mouse.X / w;
                int y = (int)mouse.Y / w;

                grid[y, x] = hueValue;

                // Change the color of the sand over time
                hueValue += 0.5f;
                if (hueValue > 360)
                {
                    hueValue = 1;
                }
            }
        }

        private Color HsvToRgb(float h, float s, float v)
        {
            int i;
            float f, p, q, t;

            if (s < float.Epsilon)
            {
                byte c = (byte)(v * 255);
                return Color.FromRgb(c, c, c);
            }

            h /= 60;
            i = (int)Math.Floor(h);
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            float r, g, b;
            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }
    }
}
