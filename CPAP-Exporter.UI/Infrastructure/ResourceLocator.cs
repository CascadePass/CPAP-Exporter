using System.Windows;
using System.Windows.Media;

namespace CascadePass.CPAPExporter
{
    public static class ResourceLocator
    {
        public static T GetResource<T>(string key) where T : class
        {
            if (Application.Current == null)
            {
                throw new InvalidOperationException("Application.Current is null. Ensure WPF app is properly initialized.");
            }

            if (Application.Current.Resources.Contains(key))
            {
                return Application.Current.Resources[key] as T;
            }

            foreach (var dictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (dictionary.Contains(key))
                    return dictionary[key] as T;
            }

            // TODO: Raise an event so this can be logged or handled
            return null;
        }

        public static Color? GetColorResource(string key)
        {
            if (Application.Current == null)
            {
                throw new InvalidOperationException("Application.Current is null. Ensure WPF app is properly initialized.");
            }

            if (Application.Current.Resources.Contains(key) &&
                Application.Current.Resources[key] is Color color)
            {
                return color;
            }

            foreach (var dictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (dictionary.Contains(key) && dictionary[key] is Color mergedColor)
                {
                    return mergedColor;
                }
            }

            // TODO: Raise an event so this can be logged or handled
            return null;
        }

        public static object GetResource(string key)
        {
            return ResourceLocator.GetResource<object>(key);
        }
    }
}
