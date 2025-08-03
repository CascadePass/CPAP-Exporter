using System.Windows;

namespace CascadePass.CPAPExporter
{
    public class CpapExporterThemeDetector : ThemeDetector
    {
        public CpapExporterThemeDetector() : base()
        {
        }

        public CpapExporterThemeDetector(IRegistryProvider registryProviderToUse) : base(registryProviderToUse)
        {
        }

        public override bool ApplyTheme()
        {
            if (Application.Current?.Dispatcher is null)
            {
                // This is probably a unit test.
                return false;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {

                Application.Current.Resources.MergedDictionaries.Clear();

                var currentTheme = this.GetThemeType();
                if (currentTheme == ThemeType.Dark)
                {
                    Application.Current.Resources.MergedDictionaries.Add(new() { Source = new Uri("/Themes/Dark.xaml", UriKind.Relative) });
                }
                else if (currentTheme == ThemeType.Light)
                {
                    Application.Current.Resources.MergedDictionaries.Add(new() { Source = new Uri("/Themes/Light.xaml", UriKind.Relative) });
                }

                Application.Current.Resources.MergedDictionaries.Add(new() { Source = new Uri("/Fonts.xaml", UriKind.Relative) });
                Application.Current.Resources.MergedDictionaries.Add(new() { Source = new Uri("/Styles.xaml", UriKind.Relative) });

                Application.Current.Resources.MergedDictionaries.Add(new() { Source = new Uri("pack://application:,,,/PresentationFramework.Fluent;component/Themes/Fluent.xaml", UriKind.Absolute) });
            });

            return true;
        }
    }
}
