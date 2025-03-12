using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter
{
    public class StatusBarViewModel : ViewModel
    {
        private MainWindow mainWindow;

        public MainWindow MainWindow
        {
            get => this.mainWindow;
            set => this.SetPropertyValue(ref this.mainWindow, value, nameof(this.MainWindow));
        }

        public string StatusText => (this.MainWindow?.PageViewer.DataContext as PageViewModel)?.StatusText;

        public double FontSize
        {
            get => this.MainWindow?.FontSize ?? default;
            set
            {
                if (this.MainWindow != null)
                {
                    this.MainWindow.FontSize = value;
                }
                else
                {
                    throw new InvalidOperationException("MainWindow is not set.");
                }
            }
        }
    }
}
