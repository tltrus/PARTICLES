using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Confetti
    {
        public Vector2D vel;
        Vector2D acc;
        double lifetime = 255;
        int r = 9;
        Shape2D shape;

        public Confetti(double x, double y, Color color)
        {
            acc = new Vector2D();

            vel = Vector2D.Random2D(MainWindow.rnd);
            vel.Mult(MainWindow.rnd.NextDoubleRange(1, 3));

            if (MainWindow.rnd.NextDouble() < 0.5)
            {
                shape = new Shape2D(x, y, Shape2DType.ARROW);
                shape.SetBrush(color);
            }
            else
            {
                shape = new Shape2D(x, y, Shape2DType.ROMB);
                shape.SetBrush(color);
            }
        }
        public bool isFinished() => lifetime < 0;

        public void ApplyForce(Vector2D force) => acc.Add(force);


        public void Update()
        {
            vel.Add(acc);
            shape.Addition(vel);
            shape.RotateAroundCenter(MainWindow.rnd.Next(10));

            acc.Mult(0);
            lifetime -= 5;
        }

        public void Show(DrawingContext dc)
        {
            shape.SetOpacity(lifetime);
            shape.Show(dc);
        }
    }
}
