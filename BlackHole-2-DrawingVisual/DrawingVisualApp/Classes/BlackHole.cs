using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DrawingVisualApp
{
    class BlackHole
    {
        public Vector2D pos;
        public int mass;
        public double rs;

        public BlackHole(int x, int y, int m)
        {
            pos = new Vector2D(x, y);
            mass = m;
            rs = (2 * MainWindow.G * mass) / (MainWindow.c * MainWindow.c);
        }

        public void Pull(Photon photon)
        {
            // 1. вычисляется вектор силы как разница векторов положения дыры и фотона
            Vector2D force = pos - photon.pos;

            // 2. вычисляется длина вектора силы между дырой и фотоном
            double r = force.Mag();

            // 3. вычисляется сила притяжения
            double fg = MainWindow.G * mass / (r * r);

            // 4. для вектора силы устанавливается его длина исходя из скаляра силы в шаге 3
            force.SetMag(fg);

            // 5. к вектору скорости фотона прибавляем вектор силы
            photon.vel.Add(force);
            photon.vel.SetMag(MainWindow.c);

            if (r < rs)
            {
                photon.Stop();
            }
        }

        public void Drawing(DrawingContext dc)
        {
            dc.DrawEllipse(Brushes.Black, null, new Point(pos.X, pos.Y), mass * 0.03, mass * 0.03);
        }

    }
}
