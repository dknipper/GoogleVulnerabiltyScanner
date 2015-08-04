using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DorkWindowsApp.ViewModels;

namespace DorkWindowsApp
{
    public partial class MainWindow
    {
        public GoogleDorkMasterViewModel ViewModelDataContext;

        public MainWindow()
        {
            InitializeComponent();
            ViewModelDataContext = new GoogleDorkMasterViewModel();
            DataContext = ViewModelDataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Rect.Visibility == Visibility.Collapsed)
            {
                Rect.Visibility = Visibility.Visible;
                var button = sender as Button;
                if (button != null)
                {
                    button.Content = "<";
                }
            }
            else
            {
                Rect.Visibility = Visibility.Collapsed;
                var button = sender as Button;
                if (button != null)
                {
                    button.Content = ">";
                }
            }
        }

        private void ExitCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void VulnerabilitiesPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Device.Target.GetType().Name != "DataGridCell" || e.Key != Key.Delete)
            {
                return;
            }
            var res = MessageBox.Show("Are you sure want to delete this vulnerability?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            e.Handled = (res == MessageBoxResult.No);
        }
    }
}
