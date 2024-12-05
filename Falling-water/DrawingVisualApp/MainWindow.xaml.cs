using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawingVisualApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer, timer2;
        public static Random rnd = new Random();
        public static double width, height;
        Point pMouse;
        int oldCell;

        DrawingVisual visual;
        DrawingContext dc;

        Map Tank;

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = g1.Width;
            height = g1.Height;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);

            timer2 = new System.Windows.Threading.DispatcherTimer();
            timer2.Tick += new EventHandler(timer2Tick);
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 300);

            Init();

            timer.Start();
        }

        void Init()
        {
            Tank = new Map(80, 80, 5);
        }


        private void Draw()
        {
            g1.RemoveVisual(visual);

            using (dc = visual.RenderOpen())
            {
                Tank.Draw(dc);

                dc.Close();
                g1.AddVisual(visual);
            }
        }

        private void timerTick(object sender, EventArgs e) => Draw();
        private void timer2Tick(object sender, EventArgs e) => Tank.SetTo(1, Tank.cols / 2, 1);
        private void btnTimer_Click(object sender, RoutedEventArgs e) => timer2.Start();
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pMouse = e.GetPosition(g1);
            oldCell = Tank.GetCellindex(pMouse);

            Tank.SetWall(pMouse);
            Draw();
        }
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            pMouse = e.GetPosition(g1);
            Draw();
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                pMouse = e.GetPosition(g1);

                int index = Tank.GetCellindex(pMouse);

                if (index != oldCell)
                {
                    Tank.SetWall(pMouse);
                    Draw();
                    oldCell = index;
                }
            }
        }
    }
}
