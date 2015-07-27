using System;
using System.Windows;
using System.Windows.Controls;

namespace DorkWindowsApp.Utilities
{
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached
                ("BindableSource",
                    typeof(string),
                    typeof(WebBrowserUtility),
                    new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var browser = dependencyObject as WebBrowser;
            if (browser == null)
            {
                return;
            }
            var uri = e.NewValue as string;
            browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
        }
    }
}
