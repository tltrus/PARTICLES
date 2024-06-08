using System;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Particle
    {
        public Vector2D pos, prevpos;
        public Vector2D vel = new Vector2D();
        public Vector2D acc = new Vector2D();
        int maxspeed = 4;
        Brush brush;


        public Particle(int x, int y)
        {
            pos = new Vector2D(x, y);
            prevpos = pos.CopyToVector2D();

            brush = Brushes.White;
        }

        public void Update()
        {
            vel.Add(acc);
            vel.Limit(maxspeed);
            pos.Add(vel);
            acc.Mult(0);
        }

        public void Follow(Vector2D[] Vector2Ds, int cols, int scl)
        {
            int x = (int)Math.Floor(pos.X / scl);
            int y = (int)Math.Floor(pos.Y / scl);
            int index = x + y * cols;
            Vector2D force = Vector2Ds[index];
            ApplyForce(force);
        }

        private void ApplyForce(Vector2D force) => acc.Add(force);

        public void Show(DrawingContext dc)
        {
            Point p0 = new Point();
            p0.X = pos.X; 
            p0.Y = pos.Y;

            Point p1 = new Point();
            p1.X = prevpos.X;
            p1.Y = prevpos.Y;

            dc.DrawLine(new Pen(brush, 0.6), p0, p1);

            prevpos = pos.CopyToVector2D();
        }

        public void Edges()
        {
            if (pos.X >= MainWindow.width)
            {
                pos.X = 0;
                prevpos = pos.CopyToVector2D();
            }
            if (pos.X < 0)
            {
                pos.X = MainWindow.width - 1;
                prevpos = pos.CopyToVector2D();
            }
            if (pos.Y >= MainWindow.height)
            {
                pos.Y = 0;
                prevpos = pos.CopyToVector2D();
            }
            if (pos.Y < 0)
            {
                pos.Y = MainWindow.height - 1;
                prevpos = pos.CopyToVector2D();
            }
        }
    }
}
