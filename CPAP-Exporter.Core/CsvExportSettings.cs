namespace CascadePass.CPAPExporter.Core
{
    public class CsvExportSettings : ExportSettings
    {
        public CsvExportSettings() : base()
        {
            this.IncludeColumnHeaders = true;
            this.Delimiter = ",";
            this.EventFilenames = [];
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include the header row in the CSV output.
        /// </summary>
        /// <value>
        /// <c>true</c> to include the header row; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeColumnHeaders { get; set; }

        /// <summary>
        /// Gets or sets the delimiter to use in the CSV output.
        /// </summary>
        public string Delimiter { get; set; }

        public List<string> EventFilenames { get; set; }
    }
}
