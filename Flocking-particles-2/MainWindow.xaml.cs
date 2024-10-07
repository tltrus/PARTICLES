using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WpfApp.Classes;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public static Random rnd = new Random();
        public static int width, height;
        DrawingVisual visual;
        DrawingContext dc;
        System.Windows.Threading.DispatcherTimer Drawtimer;

        List<Boid> boids = new List<Boid>();
        List<Predator> predators = new List<Predator>();

        public MainWindow()
        {
            InitializeComponent();

            visual = new DrawingVisual();

            width = (int)g.Width;
            height = (int)g.Height;

            Drawtimer = new System.Windows.Threading.DispatcherTimer();
            Drawtimer.Tick += new EventHandler(DrawtimerTick);
            Drawtimer.Interval = new TimeSpan(0, 0, 0, 0, 30);

            Init();
        }

        private void Init()
        {
            // Добавляем ботов
            for (int i = 0; i < 100; i++)
            {
                boids.Add(new Boid());
            }

            // Добавляем хищника
            for (int i = 0; i < 5; i++)
            {
                predators.Add(new Predator());
            }
        }

        // ОТРИСОВКА
        private void Draw()
        {
            g.RemoveVisual(visual);
            using (dc = visual.RenderOpen())
            {
                foreach (var predator in predators)
                {
                    predator.Update();
                    predator.Show(dc);
                    predator.Edges();
                }

                foreach (var boid in boids)
                {
                    boid.Edges();
                    boid.Flock(boids, predators);
                    boid.Update();
                    boid.Show(dc);
                }

                dc.Close();
                g.AddVisual(visual);
            }
        }

        private void DrawtimerTick(object sender, EventArgs e) => Draw();
        private void BtnStop_Click(object sender, RoutedEventArgs e) => Drawtimer.Stop();
        private void BtnStart_Click(object sender, RoutedEventArgs e) => Drawtimer.Start();

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Drawtimer.Stop();
            boids.Clear();
            predators.Clear();

            Init();

            Drawtimer.Start();
        }
    }
}
