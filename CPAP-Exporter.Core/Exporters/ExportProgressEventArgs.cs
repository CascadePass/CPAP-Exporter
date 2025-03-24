namespace CascadePass.CPAPExporter.Core
{
    public class ExportProgressEventArgs : EventArgs
    {
        public int CurrentRowIndex { get; set; }

        public int ExpectedRows { get; set; }
    }
}
