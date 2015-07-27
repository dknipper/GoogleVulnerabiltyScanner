using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMapper;
using DorkBusiness.Google.Entities;
using DorkWindowsApp.ViewModels;

namespace DorkWindowsApp
{
    public partial class MainWindow
    {
        public GoogleDorkMasterViewModel ViewModelDataContext;

        public MainWindow()
        {
            Mapper.CreateMap<GoogleDorkViewModel, GoogleDork>();
            Mapper.CreateMap<GoogleDork, GoogleDorkViewModel>();

            Mapper.CreateMap<GoogleDorkParentViewModel, GoogleDorkParent>();
            Mapper.CreateMap<GoogleDorkParent, GoogleDorkParentViewModel>();

            InitializeComponent();
            ViewModelDataContext = new GoogleDorkMasterViewModel();
            DataContext = ViewModelDataContext;
            //tv.ItemsSource = ViewModelDataContext.GoogleDorkParentViewModels;
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

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                WbSample.Navigate(TxtUrl.Text);
        }

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((WbSample != null) && (WbSample.CanGoBack));
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WbSample.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((WbSample != null) && (WbSample.CanGoForward));
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WbSample.GoForward();
        }

        private void GoToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void GoToPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WbSample.Navigate(TxtUrl.Text);
        }

        private void ExitCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
