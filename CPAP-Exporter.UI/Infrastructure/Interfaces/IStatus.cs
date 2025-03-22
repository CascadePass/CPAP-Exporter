namespace CascadePass.CPAPExporter
{
    public interface IStatus
    {
        string StatusText { get; set; }

        StatusProgressBar ProgressBar { get; set; }
    }
}
