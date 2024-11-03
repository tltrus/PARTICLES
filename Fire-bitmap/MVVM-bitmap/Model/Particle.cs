
namespace MVVM_bitmap.Model
{
    public class Particle
    {
        public double x;
        public double y;
        double vx;
        double vy;
        public byte alpha = 100;
        Random rnd;
        public double radius = 7;

        public Particle(Random rnd)
        {
            this.rnd = rnd;

            x = 200;
            y = 360;

            vx = -1 + rnd.NextDouble() * 2;
            vy = rnd.Next(-5, 0);
        }

        public bool Finished() => alpha > 250;

        public void Update()
        {
            x += vx;
            y += vy;
            alpha += 3;
            radius -= 0.1;
        }
    }
}
