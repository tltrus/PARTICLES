using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        public static int c = 30;
        public static double G = 0.51033;
        public static double dt = 0.1;

        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        List<Photon> photons;
        BlackHole hole;

        public MainWindow()
        {
            InitializeComponent();

            width = (int)g.Width;
            height = (int)g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            hole = new BlackHole(width / 2, height / 2, 1000);

            visual = new DrawingVisual();

            Init();
        }

        void Init()
        {
            // photons
            photons = new List<Photon>();
            for (int y = 0; y < height / 2; y += 10)
                photons.Add(new Photon(width - 20, y));
        }

        private void Drawing()
        {
            g.RemoveVisual(visual);
            //visual = new DrawingVisual();

            using (dc = visual.RenderOpen())
            {
                dc.DrawEllipse(Brushes.LightGray, null, new Point(width / 2, height / 2), 80, 80);
                dc.DrawEllipse(Brushes.White, null, new Point(width / 2, height / 2), 50, 50);

                foreach (var photon in photons)
                {
                    hole.Pull(photon);
                    photon.Update();
                    photon.Drawing(dc);
                }

                hole.Drawing(dc);

                dc.Close();
                g.AddVisual(visual);
            }
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Init();
            timer.Start();
        }
        private void timerTick(object sender, EventArgs e) => Drawing();
    }
}
