using System;
using System.Windows;
using System.Windows.Controls;

namespace MiniProject.Helpers
{
    public class BrowserBinding
    {
        public static readonly DependencyProperty BindableSourceProperty = DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(BrowserBinding), new UIPropertyMetadata(null, OnBindableSourceChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        private static void OnBindableSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var browser = o as WebBrowser;
            if (browser == null)
                return;

            var uri = (string)e.NewValue;

            try
            {
                browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            }
            catch (ObjectDisposedException) { }
        }
    }
}
