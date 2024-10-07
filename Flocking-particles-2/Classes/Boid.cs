using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;


namespace WpfApp.Classes
{
    class Boid : Particle
    {
        double maxForce = 0.2;
        int maxSpeed = 4;
        bool escaping = false;

        public Boid()
        {
            position = new Vector2D(MainWindow.rnd.Next(MainWindow.width), MainWindow.rnd.Next(MainWindow.height));
            velocity = Vector2D.Random2D(MainWindow.rnd);
            velocity.SetMag(MainWindow.rnd.Next(2, 4));
            acceleration = new Vector2D();
        }

        public Vector2D Align(List<Boid> boids)
        {
            int perceptionRadius = 50;
            Vector2D steering = new Vector2D(0, 0);
            int total = 0;
            foreach (var other in boids)
            {
                double d = Vector2D.Dist(position, other.position);
                if (other != this && d < perceptionRadius)
                {
                    steering.Add(other.velocity);
                    total++;
                }
            }
            if (total > 0)
            {
                steering.Div(total);
                steering.SetMag(maxSpeed);
                steering.Sub(velocity);
                steering.Limit(maxForce);
            }
            return steering;
        }

        public Vector2D Separation(List<Boid> boids)
        {
            int perceptionRadius = 50;
            Vector2D steering = new Vector2D(0, 0);
            int total = 0;
            foreach (var other in boids)
            {
                double d = Vector2D.Dist(position, other.position);

                if (other != this && d < perceptionRadius)
                {
                    var diff = Vector2D.Sub(position, other.position);
                    diff.Div(d * d);
                    steering.Add(diff);
                    total++;
                }
            }
            if (total > 0)
            {
                steering.Div(total);
                steering.SetMag(maxSpeed);
                steering.Sub(velocity);
                steering.Limit(maxForce);
            }
            return steering;
        }

        public Vector2D Cohesion(List<Boid> boids)
        {
            int perceptionRadius = 50;
            Vector2D steering = new Vector2D(0, 0);
            int total = 0;
            foreach (var other in boids)
            {
                double d = Vector2D.Dist(position, other.position);

                if (other != this && d < perceptionRadius)
                {
                    steering.Add(other.position);
                    total++;
                }
            }
            if (total > 0)
            {
                steering.Div(total);
                steering.Sub(position);
                steering.SetMag(maxSpeed);
                steering.Sub(velocity);
                steering.Limit(maxForce);
            }
            return steering;
        }


        public void Flock(List<Boid> boids, List<Predator> predators)
        {
            Vector2D alignment = Align(boids);
            Vector2D cohesion = Cohesion(boids);
            Vector2D separation = Separation(boids);

            alignment.Mult(1.5);
            cohesion.Mult(1.0);
            separation.Mult(1.0);


            // Код для идентификации хищника:

            escaping = false;

            Vector2D fleeDir = new Vector2D();
            foreach (var p in predators)
            {
                var d = Vector2D.Dist(p.position, position);
                if (d < 50)
                {
                    fleeDir.Add(p.position);
                    escaping = true;
                }
            }

            acceleration.Mult(0);

            if (escaping == true)
            {
                var newvel = Vector2D.Sub(position, fleeDir);
                acceleration.Add(newvel);
            }
            else
            {
                acceleration.Add(separation);
                acceleration.Add(alignment);
                acceleration.Add(cohesion);
            }
        }

        public void Update()
        {
            position.Add(velocity); 
            velocity.Add(acceleration);
            velocity.Limit(maxSpeed);
            acceleration.Mult(0);
        }

        public void Show(DrawingContext dc)
        {
            Point p = new Point(position.X, position.Y);
            dc.DrawEllipse(Brushes.White, null, p, 3, 3);
        }
    }
}
