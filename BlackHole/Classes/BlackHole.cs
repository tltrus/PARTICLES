
namespace WpfApp
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

        public void Pull(Particle photon)
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

    }


}
