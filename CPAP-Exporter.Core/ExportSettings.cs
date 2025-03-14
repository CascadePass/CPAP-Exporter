namespace CascadePass.CPAPExporter.Core
{
    public class ExportSettings
    {
        public ExportSettings()
        {
            this.IsActive = true;

            this.OutputFileHandling = OutputFileRule.OneFilePerNight;

            this.IncludeRowNumber = true;
            this.IncludeSessionNumber = true;
            this.IncludeTimestamp = true;
            this.IncludeEvents = true;

            this.IncludePythonBoilerplate = false;

            this.Filenames = [];
        }

        public bool IsActive { get; set; }

        public OutputFileRule OutputFileHandling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the row number in the CSV output.
        /// </summary>
        /// <value>
        /// <c>true</c> to include the row number; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeRowNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the session number in the CSV output.
        /// </summary>
        /// <value>
        /// <c>true</c> to include the session number; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeSessionNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the timestamp in the CSV output.
        /// </summary>
        /// <value>
        /// <c>true</c> to include the timestamp; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// The time of a data point is calculated as the start time of the session it appears
        /// in, plus 2 seconds per ordinal within the Samples collection.
        /// </remarks>
        public bool IncludeTimestamp { get; set; }

        public bool IncludeEvents { get; set; }

        public bool IncludePythonBoilerplate { get; set; }

        public List<string> Filenames { get; set; }
    }
}
