using System.ComponentModel;

namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// Base class for objects implementing <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class Observable : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when the value of a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetPropertyValue<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetPropertyValue<T>(ref T field, T value, string[] propertyNames)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;

                foreach (string propertyName in propertyNames)
                {
                    this.OnPropertyChanged(propertyName);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property whose
        /// value has changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
