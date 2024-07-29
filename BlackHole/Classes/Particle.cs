
namespace WpfApp
{
    class Particle
    {
        public Vector2D pos;
        public Vector2D vel;
        bool stopped;

        public Particle(double x, double y)
        {
            pos = new Vector2D(x, y);
            vel = new Vector2D(-MainWindow.c, 0);
            stopped = false;
        }

        public void Stop() => stopped = true;

        public void Update()
        {
            if (!stopped)
            {
                Vector2D deltaV = vel;
                deltaV.Mult(MainWindow.dt);
                pos.Add(deltaV);
            }
        }
    }
}
