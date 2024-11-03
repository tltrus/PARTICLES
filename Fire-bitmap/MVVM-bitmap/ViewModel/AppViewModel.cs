using System.Windows.Media;
using System.Windows.Media.Imaging;
using MVVM_bitmap.Model;

namespace MVVM_bitmap
{
    public class AppViewModel
    {
        System.Windows.Threading.DispatcherTimer timer;
        List<Particle> particles;
        WriteableBitmap wb;
        Random rnd = new Random();

        private BitmapSource bitmapSource;
        public BitmapSource ButtonSource
        {
            get { return bitmapSource; }
        }

        #region region_RelayCommand

        private RelayCommand startCommand;
        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ??
                  (startCommand = new RelayCommand(obj =>
                  {
                      timer.Start();
                  }));
            }
        }

        #endregion

        public AppViewModel()
        {
            particles = new List<Particle>();

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            wb = new WriteableBitmap(
                (int)780,
                (int)380,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            bitmapSource = wb;

            wb.Clear(Colors.WhiteSmoke);
        }

        private void timerTick(object sender, EventArgs e) => Drawing();

        private void Drawing()
        {
            wb.Clear(Colors.WhiteSmoke);

            for (int i = 0; i < 5; ++i)
            {
                particles.Add(new Particle(rnd));
            }

            for (int i = particles.Count - 1; i >= 0; i--)  
            {
                particles[i].Update();
                if (particles[i].Finished())
                    particles.RemoveAt(particles.IndexOf(particles[i]));

                var color = Color.FromArgb(particles[i].alpha, 255, 0, 0);
                wb.FillEllipseCentered((int)particles[i].x, (int)particles[i].y, (int)particles[i].radius, (int)particles[i].radius, color);
            }

        }
    }
}
