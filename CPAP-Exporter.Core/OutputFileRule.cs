namespace CascadePass.CPAPExporter.Core
{
    /// <summary>
    /// Defines the options for exporting CPAP data to data files.
    /// </summary>
    public enum OutputFileRule
    {
        /// <summary>
        /// Export data for each night into separate data files.
        /// </summary>
        OneFilePerNight,

        /// <summary>
        /// Export data for all selected nights into a single data file.
        /// </summary>
        CombinedIntoSingleFile,
    }
}
