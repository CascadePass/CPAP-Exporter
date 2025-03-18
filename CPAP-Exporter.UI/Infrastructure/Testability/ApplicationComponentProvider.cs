namespace CascadePass.CPAPExporter
{
    public static class ApplicationComponentProvider
    {
        static ApplicationComponentProvider()
        {
            ApplicationComponentProvider.Status = new Status();
        }

        public static IStatus Status { get; }
    }
}
