
namespace WpfApp.Classes
{
    class Particle
    {
        public Vector2D position;
        public Vector2D velocity;
        public Vector2D acceleration;

        public void Edges()
        {
            if (position.X > MainWindow.width)
            {
                position.X = 0;
            }
            else if (position.X < 0)
            {
                position.X = MainWindow.width;
            }
            if (position.Y > MainWindow.height)
            {
                position.Y = 0;
            }
            else if (position.Y < 0)
            {
                position.Y = MainWindow.height;
            }
        }
    }
}
