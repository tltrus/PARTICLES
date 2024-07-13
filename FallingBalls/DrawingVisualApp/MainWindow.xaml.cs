using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;
        List<Particle> particles = new List<Particle>();
        Vector2D vel = new Vector2D(0, 1);
        Vector2D mouse = new Vector2D(0, 0);
        Brush[] brushes = new Brush[] { Brushes.Brown, Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.White, Brushes.Blue, Brushes.Pink };

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            for (int i = 0; i < 50; ++i)
            {
                var pos = new Vector2D(rnd.Next(200, width - 200), rnd.Next(200, height - 200));
                var brush = brushes[rnd.Next(brushes.Count())];
                var radius = rnd.Next(8, 15);
                var particle = new Particle(pos, radius, brush);
                particles.Add(particle);
            }
            timer.Start();
        }


        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                for(int k = 0; k < 3; k++) 
                {
                    for (int i = 0; i < particles.Count; i++)
                    {
                        for (int j = 0; j < particles.Count; j++)
                        {
                            if (i == j) continue;

                            var p1 = particles[i].pos;
                            var p2 = particles[j].pos;

                            var mag = (p1 - p2).Mag();
                            var len = particles[i].radius + particles[j].radius;

                            if (mag < len)
                            {
                                Vector2D penetrationDirection = (p2 - p1).Normalize();
                                double penetrationDepth = particles[i].radius + particles[j].radius - (p2 - p1).Mag();

                                if (particles[i].pos.Y < height - particles[i].radius)
                                {
                                    particles[i].pos += -penetrationDirection.Mult(penetrationDepth * 0.5);
                                }

                                if (particles[j].pos.Y < height - particles[j].radius)
                                {
                                    particles[j].pos += penetrationDirection.Mult(penetrationDepth * 0.5);
                                }
                            }
                        }
                    }
                }

                foreach (var p in particles)
                {
                    if (p.pos.Y < height - p.radius)
                        p.pos += vel;
                    p.Show(dc);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void g_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouse.X = e.GetPosition(g).X;
            mouse.Y = e.GetPosition(g).Y;

            var radius = rnd.Next(8, 15);
            var brush = brushes[rnd.Next(brushes.Count())];
            var partile = new Particle(mouse, radius, brush);
            particles.Add(partile);
        }
    }
}
