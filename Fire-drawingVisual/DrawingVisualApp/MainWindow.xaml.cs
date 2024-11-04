using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timerDraw;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        List<Particle> particles = new List<Particle>();



        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            timerDraw = new System.Windows.Threading.DispatcherTimer();
            timerDraw.Tick += new EventHandler(timerDrawTick);
            timerDraw.Interval = new TimeSpan(0, 0, 0, 0, 30);

            timerDraw.Start();
        }

        private void timerDrawTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                for (int i = 0; i < 5; ++i)
                {
                    particles.Add(new Particle());
                }

                for (int i = particles.Count-1; i >= 0; i--)
                {
                    particles[i].Update();
                    particles[i].Show(dc);
                    if (particles[i].Finished())
                        particles.RemoveAt(particles.IndexOf(particles[i]));
                }
                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void g_MouseMove(object sender, MouseEventArgs e)
        {
        }
    }
}
