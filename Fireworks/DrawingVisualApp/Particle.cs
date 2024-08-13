using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Particle
    {
        public Vector2D pos;
        public Vector2D vel;
        Vector2D acc;
        double lifespan = 255;
        bool isFirework;
        Color color;


        public Particle(double x, double y, Color color, bool isFirework)
        {
            pos = new Vector2D(x, y);
            acc = new Vector2D();

            this.color = color;

            this.isFirework = isFirework;

            if (isFirework)
                vel = new Vector2D(0, MainWindow.rnd.Next(-12, -8));
            else
            {
                vel = Vector2D.Random2D(MainWindow.rnd);
                vel.Mult(MainWindow.rnd.Next(2, 10));
            }
        }

        public void ApplyForce(Vector2D force) => acc.Add(force);

        public void Update()
        {
            if (!isFirework)
            {
                vel.Mult(0.9);
                lifespan -= 4;
            }

            vel.Add(acc);
            pos.Add(vel);
            acc.Mult(0);
        }

        public bool Done()
        {
            if (lifespan < 0)
                return true;
            else
                return false;
        }


        public void Show(DrawingContext dc)
        {
            Brush brush;
            
            if (!isFirework)
            {
                color.A = (byte)lifespan;
                brush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            else
                brush = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));

            Point p = new Point(pos.X, pos.Y);
            dc.DrawEllipse(brush, null, p, 2, 2);
        }
    }
}
