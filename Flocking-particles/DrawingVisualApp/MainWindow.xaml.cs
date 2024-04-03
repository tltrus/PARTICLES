using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisualApp
{
    /// <summary>
    /// Coding Challenge #124 - Flocking Simulation
    /// https://thecodingtrain.com/CodingChallenges/124-flocking-boids.html
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timerDraw, timerControl;
        public static Random rnd = new Random();
        public static int width, height;
        Vector2D mouse = new Vector2D();

        DrawingVisual visual;
        DrawingContext dc;

        List<Particle> particles = new List<Particle>();

        public static double alignSlider, cohesionSlider, separationSlider;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            timerDraw = new System.Windows.Threading.DispatcherTimer();
            timerDraw.Tick += new EventHandler(timerDrawTick);
            timerDraw.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timerControl = new System.Windows.Threading.DispatcherTimer();
            timerControl.Tick += new EventHandler(timerControlTick);
            timerControl.Interval = new TimeSpan(0, 0, 0, 0, 10);


            for (int i = 0; i < 60; ++i)
            {
                var x = rnd.Next(10, width - 10);
                var y = rnd.Next(10, height - 10);
                particles.Add(new Particle(x, y, 1.5f, Brushes.White));
            }

            timerControl.Start();
            timerDraw.Start();
        }

        private void timerDrawTick(object sender, EventArgs e) => Drawing();
        private void timerControlTick(object sender, EventArgs e) => Control();

        private void Control()
        {
            foreach (var p in particles)
            {
                p.Flock(particles);
                p.Update();
            }
        }

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach(var p in particles)
                {
                    p.Show(dc);
                    p.Edges();
                }
                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void sbAlign_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => alignSlider = e.NewValue;
        private void sbSeparation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => separationSlider = e.NewValue;
        private void sbCohesion_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => cohesionSlider = e.NewValue;
        private void g_MouseMove(object sender, MouseEventArgs e)
        {
            mouse.X = e.GetPosition(g).X;
            mouse.Y = e.GetPosition(g).Y;
        }
    }
}
