using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace DrawingVisualApp
{
    class Particle
    {
        public Vector2D position;
        public Vector2D velocity;
        public Vector2D acceleration;
        public Brush brush;
        public float radius; // radius
        public double maxForce = 0.2;
        public int maxSpeed = 4;

        public Particle(double x, double y, float radius, Brush brush)
        {
            position = new Vector2D(x, y);
            velocity = new Vector2D();
            acceleration = new Vector2D();
            maxForce = 0.2;
            maxSpeed = 4;
            this.brush = brush;
            this.radius = radius;
        }

        public Vector2D Align(List<Particle> particles)
        {
            int perceptionRadius = 50;
            Vector2D steering = new Vector2D(0, 0);
            int total = 0;
            foreach (var other in particles)
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

        public Vector2D Cohesion(List<Particle> particles)
        {
            int perceptionRadius = 50;
            Vector2D steering = new Vector2D(0, 0);
            int total = 0;
            foreach (var other in particles)
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

        public Vector2D Separation(List<Particle> particles)
        {
            int perceptionRadius = 50;
            Vector2D steering = new Vector2D(0, 0);
            int total = 0;
            foreach (var other in particles)
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

        public void Flock(List<Particle> particles)
        {
            Vector2D alignment = Align(particles);
            Vector2D cohesion = Cohesion(particles);
            Vector2D separation = Separation(particles);

            alignment.Mult(MainWindow.alignSlider);
            cohesion.Mult(MainWindow.cohesionSlider);
            separation.Mult(MainWindow.separationSlider);

            acceleration.Add(alignment);
            acceleration.Add(cohesion);
            acceleration.Add(separation);
        }

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
            dc.DrawEllipse(Brushes.White, null, p, radius, radius);
        }
    }
}
