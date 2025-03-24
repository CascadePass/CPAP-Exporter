using System.Collections.Specialized;
using System.ComponentModel;

namespace CascadePass.CPAPExporter
{
    public class ExportOptionsPageViewModel : PageViewModel
    {
        private CsvExportOptionsViewModel csvExportOptions;

        #region Constructor

        public ExportOptionsPageViewModel(ExportParameters exportParameters) : base(Resources.PageTitle_Options, Resources.PageDesc_Options)
        {
            this.ExportParameters = exportParameters;
            this.csvExportOptions = new(this.ExportParameters) { ProgressInterval = this.ExportParameters.UserPreferences.ProgressInterval };
            this.ExportParameters.Settings.Add(this.csvExportOptions.Settings);

            this.ExportParameters.Reports.CollectionChanged += this.Reports_CollectionChanged;
            this.csvExportOptions.PropertyChanged += this.CsvOptions_PropertyChanged;

            this.csvExportOptions.CreateFilenames();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a <see cref="ViewModel"/> with options for
        /// writing CSV files.
        /// </summary>
        public CsvExportOptionsViewModel CsvExportOptions
        {
            get => this.csvExportOptions;
            set => this.SetPropertyValue(ref this.csvExportOptions, value, nameof(this.CsvExportOptions));
        }

        #endregion

        #region Methods

        public void CreateFilenames()
        {
            this.csvExportOptions?.CreateFilenames();
        }

        public override bool Validate()
        {
            return this.CsvExportOptions.IsActive;
        }

        #region Event Handlers

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.ExportParameters))
            {
                this.CreateFilenames();
            }
        }

        private void CsvOptions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IncludeEvents" || e.PropertyName == "OutputFileHandling")
            {
                this.CreateFilenames();
            }

            this.OnPropertyChanged(nameof(this.IsValid));
        }

        private void Reports_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CreateFilenames();

            this.OnPropertyChanged(nameof(this.IsValid));
        }

        #endregion

        #endregion
    }
}
