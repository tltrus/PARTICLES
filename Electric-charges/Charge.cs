using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    internal class Charge
    {
        public int x, y;
        private int dir_x, dir_y;
        public double q; // заряд
        int r;
        Color color;

        public Charge(double q, int x, int y, Color color)
        {
            this.q = q;
            this.x = x;
            this.y = y;
            this.color = color;
            r = (int)Math.Abs(q) * 10;

            dir_x = MainWindow.rnd.Next(-1, 2);
            dir_y = MainWindow.rnd.Next(-1, 2);
        }

        public void Draw(WriteableBitmap wb)
        {
            wb.FillEllipseCentered(x, y, r, r, color);
        }

        public void Update()
        {
            x += dir_x;
            y += dir_y;
        }
    }
}
