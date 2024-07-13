using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    class Particle
    {
        public Vector2D pos;
        public int radius = 8;
        public Brush brush;

        public Particle(Vector2D position, int radius, Brush brush)
        {
            pos = position.CopyToVector();
            this.brush = brush;
            this.radius = radius;
        }

        public void Show(DrawingContext dc) => dc.DrawEllipse(brush, null, new Point(pos.X, pos.Y), radius, radius);
    }
}
