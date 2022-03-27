using Firma.ViewModels;
using System.Windows;

namespace Firma
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var window = new MainWindow();
            window.DataContext = new MainWindowViewModel();
            window.Show();
        }
    }
}
