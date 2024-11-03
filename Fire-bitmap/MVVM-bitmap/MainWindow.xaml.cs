using System.Windows;

namespace MVVM_bitmap
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel();
        }
    }
}