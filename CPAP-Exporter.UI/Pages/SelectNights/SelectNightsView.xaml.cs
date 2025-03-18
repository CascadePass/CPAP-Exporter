using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CascadePass.CPAPExporter
{
    public partial class SelectNightsView : UserControl
    {
        public SelectNightsView()
        {
            this.InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SelectAllCheckBox.DataContext = this.SelectNightDataGrid.DataContext;
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.RefreshCheckBoxColumnBindings();
        }

        private void RefreshCheckBoxColumnBindings()
        {
            foreach (var item in this.SelectNightDataGrid.Items)
            {
                var row = (DataGridRow)this.SelectNightDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    if (this.SelectNightDataGrid.Columns[0] is DataGridCheckBoxColumn checkBoxColumn)
                    {
                        if (checkBoxColumn.GetCellContent(row) is CheckBox cellContent)
                        {
                            var binding = BindingOperations.GetBindingExpression(cellContent, CheckBox.IsCheckedProperty);
                            binding?.UpdateTarget();
                        }
                    }
                }
            }
        }
    }
}
