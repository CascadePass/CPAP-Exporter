namespace CascadePass.CPAPExporter
{
    /// <summary>
    /// An enum defining the pages in CPAP-Exporter which can be navigated
    /// to using the buttons along the left edge of the window.
    /// </summary>
    public enum NavigationStep
    {
        /// <summary>
        /// Choose which files to open.
        /// </summary>
        OpenFiles,

        /// <summary>
        /// Choose which nights to export data from.
        /// </summary>
        SelectDays,

        /// <summary>
        /// Choose which data to export.
        /// </summary>
        SelectSignals,

        /// <summary>
        /// Choose settings for the export, eg whether to combine files.
        /// </summary>
        Settings,

        /// <summary>
        /// Save the data and interact with saved files.
        /// </summary>
        Export,
    }
}
