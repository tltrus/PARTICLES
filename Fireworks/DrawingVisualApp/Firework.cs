using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace DrawingVisualApp
{
    class Firework
    {
        Color color;
        Particle firework;
        bool exploded;
        List<Particle> particles = new List<Particle>();
        List<Confetti> confetties = new List<Confetti>();
        bool isMulticolour, isShapeDraw;

        public Firework()
        {
            RandomColor();

            firework = new Particle(MainWindow.rnd.Next(MainWindow.width),
                                    MainWindow.height,
                                    color,
                                    true);
        }

        public void RandomColor()
        {
            byte A = (byte)MainWindow.rnd.Next(255);
            byte R = (byte)MainWindow.rnd.Next(255);
            byte G = (byte)MainWindow.rnd.Next(255);
            byte B = (byte)MainWindow.rnd.Next(255);
            color = Color.FromRgb(R, G, B);
        }

        public void SetMulticolour() => isMulticolour = true;
        public void SetShapeDraw() => isShapeDraw = true;

        public bool Done()
        {
            if (exploded && particles.Count == 0)
                return true;
            else
                return false;
        }

        public void Update()
        {
            if (!exploded)
            {
                firework.ApplyForce(MainWindow.gravity);
                firework.Update();

                if (firework.vel.Y >= 0)
                {
                    exploded = true;
                    Explode();
                }
            }

            foreach (var p in particles.ToList())
            {
                p.ApplyForce(MainWindow.gravity);
                p.Update();

                if (p.Done())
                    particles.RemoveAt(particles.IndexOf(p));
            }

            foreach (var c in confetties.ToList())
            {
                c.ApplyForce(MainWindow.gravity * 0.5);
                c.Update();

                if (c.isFinished())
                    confetties.RemoveAt(confetties.IndexOf(c));
            }
        }

        public void Explode()
        {
            for (var i = 0; i < 200; i++)
            {
                if (isMulticolour)
                    RandomColor();

                if (MainWindow.rnd.NextDouble() < 0.5)
                {
                    var p = new Particle(
                      firework.pos.X,
                      firework.pos.Y,
                      color,
                      false
                    );
                    particles.Add(p);
                }
                else
                {
                    confetties.Add(new Confetti(firework.pos.X, firework.pos.Y, color));
                }
            }
        }

        public void Show(DrawingContext dc)
        {
            if (!exploded)
                firework.Show(dc);

            foreach (var p in particles)
                p.Show(dc);

            if (isShapeDraw)
            {
                foreach (var confetti in confetties)
                    confetti.Show(dc);
            }
        }
    }
}
