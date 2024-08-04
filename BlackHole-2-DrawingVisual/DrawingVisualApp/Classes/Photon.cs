using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DrawingVisualApp
{
    internal class Photon
    {
        public Vector2D vel, pos;
        bool stopped;

        public Photon(double x, double y)
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

        public void Drawing(DrawingContext dc)
        {
             dc.DrawEllipse(Brushes.Red, null, new Point(pos.X, pos.Y), 3, 3);
        }
    }
}
