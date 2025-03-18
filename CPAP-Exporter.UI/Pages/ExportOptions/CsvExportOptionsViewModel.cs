using CascadePass.CPAPExporter.Core;

namespace CascadePass.CPAPExporter
{
    public class CsvExportOptionsViewModel : ExportOptionsViewModel
    {
        #region Constructors

        public CsvExportOptionsViewModel(ExportParameters parameters)
        {
            this.ExportParameters = parameters;
        }

        public CsvExportOptionsViewModel(ExportParameters parameters, CsvExportSettings exportSettings)
        {
            this.Settings = exportSettings;
            this.ExportParameters = parameters;
        }

        #endregion

        #region Properties

        public new CsvExportSettings Settings
        {
            get => base.Settings as CsvExportSettings;
            set => this.SetPropertyValue(ref base.settings, value, nameof(this.Settings));
        }

        public bool IsActive
        {
            get => this.settings.IsActive;
            set
            {
                if (this.settings.IsActive != value)
                {
                    this.settings.IsActive = value;
                    this.OnPropertyChanged(nameof(this.IsActive));
                }
            }
        }

        public OutputFileRule OutputFileHandling
        {
            get => this.settings.OutputFileHandling;
            set {
                if (this.settings.OutputFileHandling != value)
                {
                    this.settings.OutputFileHandling = value;
                    this.OnPropertyChanged(nameof(this.OutputFileHandling));
                }
            }
        }

        public bool IncludeEvents
        {
            get => this.settings.IncludeEvents;
            set
            {
                if (this.settings.IncludeEvents != value)
                {
                    this.settings.IncludeEvents = value;
                    this.OnPropertyChanged(nameof(this.IncludeEvents));
                }
            }
        }

        public bool IncludeColumnHeaders
        {
            get => this.Settings.IncludeColumnHeaders;
            set
            {
                if (this.Settings.IncludeColumnHeaders != value)
                {
                    this.Settings.IncludeColumnHeaders = value;
                    this.OnPropertyChanged(nameof(this.IncludeColumnHeaders));
                }
            }
        }

        public bool IncludeRowNumber
        {
            get => this.settings.IncludeRowNumber;
            set
            {
                if (this.settings.IncludeRowNumber != value)
                {
                    this.settings.IncludeRowNumber = value;
                    this.OnPropertyChanged(nameof(this.IncludeRowNumber));
                }
            }
        }

        public bool IncludeSessionNumber
        {
            get => this.settings.IncludeSessionNumber;
            set
            {
                if (this.settings.IncludeSessionNumber != value)
                {
                    this.settings.IncludeSessionNumber = value;
                    this.OnPropertyChanged(nameof(this.IncludeSessionNumber));
                }
            }
        }

        public bool IncludeTimestamp
        {
            get => this.settings.IncludeTimestamp;
            set
            {
                if (this.settings.IncludeTimestamp != value)
                {
                    this.settings.IncludeTimestamp = value;
                    this.OnPropertyChanged(nameof(this.IncludeTimestamp));
                }
            }
        }

        public bool IncludePythonBoilerplate
        {
            get => this.settings.IncludePythonBoilerplate;
            set
            {
                if (this.settings.IncludePythonBoilerplate != value)
                {
                    this.settings.IncludePythonBoilerplate = value;
                    this.OnPropertyChanged(nameof(this.IncludePythonBoilerplate));
                }
            }
        }

        public string Delimiter
        {
            get => this.Settings.Delimiter;
            set
            {
                if (this.Settings.Delimiter != value)
                {
                    this.Settings.Delimiter = value;
                    this.OnPropertyChanged(nameof(this.Delimiter));
                }
            }
        }

        #endregion

        #region Methods

        public override ExportSettings CreateSettings() => new CsvExportSettings();

        public override void WriteSettings()
        {
            this.Settings.Filenames.Clear();

            foreach (var filename in this.ExportFilenames)
            {
                this.Settings.Filenames.Add(filename.RawFilename);

                if (this.Settings.IncludeEvents)
                {
                    this.Settings.EventFilenames.Add(filename.EventsFilename);
                }
            }
        }
    }

    #endregion
}
