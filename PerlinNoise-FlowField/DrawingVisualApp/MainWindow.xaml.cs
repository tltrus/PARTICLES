using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{

    // Based on "#24 — Perlin Noise Flow Field" https://thecodingtrain.com/challenges/24-perlin-noise-flow-field

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;
        List<Particle> particles = new List<Particle>();

        int scl = 10;
        double inc = 0.1, zoff = 0;
        int rows, cols;

        Vector2D[] flowfield;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            cols = width / scl;
            rows = height / scl;

            flowfield = new Vector2D[cols * rows];
            
            for (int i = 0; i < 300; ++i)
                particles.Add(new Particle(rnd.Next(0, width), rnd.Next(0, height)));
            
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            double yoff = 0;
            for (int y = 0; y < rows; y++)
            {
                double xoff = 0;
                for (int x = 0; x < cols; x++)
                {
                    int index = x + y * cols;
                    double angle = Perlin3D.perlin(xoff, yoff, zoff) * Math.PI * 2;
                    Vector2D v = Vector2D.FromAngle(angle);
                    v.SetMag(1);
                    flowfield[index] = v;
                    xoff += inc;
                }
                yoff += inc;
                zoff += 0.0003;
            }

            g.RemoveVisual(visual);

            //DrawingVisual visual = new DrawingVisual();

            using (dc = visual.RenderOpen())
            {
                foreach (var p in particles)
                {
                    p.Follow(flowfield, cols, scl);
                    p.Update();
                    p.Edges();
                    p.Show(dc);
                }

                dc.Close();
            }
            g.AddVisual(visual);
        }
    }
}
