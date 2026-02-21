using System.Windows;
using HardWareMonitor.ViewModels;

namespace HardWareMonitor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        // Обрабатывает загрузку окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
        }
    }
}