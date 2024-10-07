using System.Windows;
using System.Windows.Media;

namespace WpfApp.Classes
{
    class Predator : Particle
    {
        public Predator()
        {
            position = new Vector2D(MainWindow.rnd.Next(MainWindow.width), MainWindow.rnd.Next(MainWindow.height));
            velocity = Vector2D.Random2D(MainWindow.rnd);
        }

        public void Update()
        {
            position.Add(velocity);
        }

        public void Show(DrawingContext dc)
        {
            Point p = new Point(position.X, position.Y);
            dc.DrawEllipse(Brushes.Yellow, new Pen(Brushes.Red, 1.5), p, 7, 7);
        }
    }
}
