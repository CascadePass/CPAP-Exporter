namespace CascadePass.CPAPExporter
{
    public static class ApplicationComponentProvider
    {
        static ApplicationComponentProvider()
        {
            ApplicationComponentProvider.Status = new Status();
            ApplicationComponentProvider.PageViewModelProvider = new PageViewModelProvider();
        }

        public static IStatus Status
        {
            get;
#if DEBUG
            set;
#endif
        }

        public static IPageViewModelProvider PageViewModelProvider
        {
            get;
#if DEBUG
            set;
#endif
        }
    }
}
