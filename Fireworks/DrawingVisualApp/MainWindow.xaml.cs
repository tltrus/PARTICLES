using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    // Based on "#27 — Fireworks" https://thecodingtrain.com/challenges/27-fireworks

    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timerDraw;
        public static Random rnd = new Random();
        public static int width, height;

        DrawingVisual visual;
        DrawingContext dc;

        List<Firework> fireworks = new List<Firework>();
        public static Vector2D gravity = new Vector2D(0, 0.2);


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
            if (rnd.NextDouble() < 0.03)
            {
                var f = new Firework();
                if (cbMulticoloured.IsChecked == true)
                    f.SetMulticolour();

                if (cbShapes.IsChecked == true)
                    f.SetShapeDraw();

                fireworks.Add(f);
            }


            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach (var f in fireworks.ToList())
                {
                    f.Update();
                    f.Show(dc);
                    if (f.Done())
                        fireworks.RemoveAt(fireworks.IndexOf(f));
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }
    }
}
