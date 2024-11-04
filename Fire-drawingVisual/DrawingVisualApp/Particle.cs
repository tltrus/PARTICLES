using System.Windows;
using System.Windows.Media;

namespace DrawingVisualApp
{
    class Particle
    {
        double x;
        double y;
        double vx;
        double vy;
        float alpha = 240f;

        public Particle()
        {
            x = MainWindow.width/2;
            y = MainWindow.height - 10;


            vx = -1 + MainWindow.rnd.NextDouble() * 3;
            vy = MainWindow.rnd.Next(-5, 0);
        }

        public bool Finished()
        {
            return alpha < 1;
        }

        public void Update()
        {
            x += vx;
            y += vy;
            alpha -= 5;
        }

        public void Show(DrawingContext dc)
        {
            Point p = new Point(x, y);
            Brush brush = new SolidColorBrush(Color.FromArgb((byte)alpha, 255, 0, 0));
            dc.DrawEllipse(brush, null, p, 8, 8);
        }
    }
}
