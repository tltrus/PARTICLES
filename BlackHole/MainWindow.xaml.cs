using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    // Based on "#144 — 2D Black Hole Visualization" https://thecodingtrain.com/challenges/144-2d-black-hole-visualization
    public partial class MainWindow : Window
    {
        public static int c = 41;
        public static int G = 2;
        public static double dt = 0.11;


        System.Windows.Threading.DispatcherTimer timer;
        static Random rnd = new Random();
        WriteableBitmap wb;
        static int imgWidth, imgHeight;
        List<Particle> particles = new List<Particle>();
        BlackHole hole;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            imgWidth = (int)image.Width; imgHeight = (int)image.Height;

            wb = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null); image.Source = wb;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            hole = new BlackHole(imgWidth / 2, imgHeight / 2, 1000);

            // particles
            for (int y = 0; y < imgHeight / 2; y+=10)
                particles.Add(new Particle(imgWidth - 20, y));
        }


        void Control()
        {
            foreach (var particle in particles)
            {
                hole.Pull(particle);
                particle.Update();
                wb.FillEllipseCentered((int)particle.pos.X, (int)particle.pos.Y, 1, 1, Colors.Red);

            }

            wb.FillEllipseCentered((int)hole.pos.X, (int)hole.pos.Y, (int)(hole.rs * 10), (int)(hole.rs * 10), Colors.Black);

        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) => timer.Start();

        private void timerTick(object sender, EventArgs e)
        {
            //wb.Clear(Colors.Black);
            Control();
        }
    }
}


