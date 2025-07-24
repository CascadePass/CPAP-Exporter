using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.CPAPExporter
{
    public class NullableObjectToVisibilityConverter : IValueConverter
    {
        public bool CollapseWhenNull { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = value != null;
            return isVisible ? Visibility.Visible :
                   CollapseWhenNull ? Visibility.Collapsed : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
