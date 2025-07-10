using System.Windows;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IThemeDetector themeDetector;

        public App()
        {
            this.themeDetector = new CpapExporterThemeDetector();
            this.themeDetector.ThemeChanged += this.OnThemeChanged;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.themeDetector.ApplyTheme();
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            this.themeDetector.ApplyTheme();
        }
    }

}
