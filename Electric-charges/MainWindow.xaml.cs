using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        WriteableBitmap wb;
        int imgWidth, imgHeight;
        List<Charge> charges;
        int destiny = 27;

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            imgWidth = (int)image.Width; imgHeight = (int)image.Height;

            wb = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null);
            image.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);

            charges = new List<Charge>();
            charges.Add(new Charge(1, 100, 100, Colors.White));
            charges.Add(new Charge(-2, 400, 200, Colors.White));
            charges.Add(new Charge(1, 250, 370, Colors.White));

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Draw();

        void Draw()
        {
            wb.Clear(Colors.Black);

            for(int y = 0; y < imgHeight; y++) 
            {
                for (int x = 0; x < imgWidth; x++)
                {
                    double u = 0;

                    foreach (var charge in charges)
                    {
                        var ri = Math.Sqrt((x - charge.x) * (x - charge.x) + (y - charge.y) * (y - charge.y)) / 500;
                        if (ri != 0) u += charge.q / ri;
                    }

                    var a = (int)(u * destiny);

                    if (a % destiny == 0)
                    {
                        wb.SetPixel(x, y, Colors.White);
                    }
                }

            }

            foreach (var charge in charges)
            {
                charge.Draw(wb);
                charge.Update();
            }
        }
    }
}
