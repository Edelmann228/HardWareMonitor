using System.Windows;
using HardWareMonitor.ViewModels;

namespace HardWareMonitor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Можно добавить дополнительную инициализацию если нужно
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Действия при загрузке окна
            var viewModel = DataContext as MainViewModel;
            if (viewModel != null)
            {
                // Можно вызвать что-то при загрузке
            }
        }
    }
}