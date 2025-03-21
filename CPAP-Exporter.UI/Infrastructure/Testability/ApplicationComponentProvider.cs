namespace CascadePass.CPAPExporter
{
    public static class ApplicationComponentProvider
    {
        static ApplicationComponentProvider()
        {
            ApplicationComponentProvider.Status = new Status();
            ApplicationComponentProvider.PageViewModelProvider = new PageViewModelProvider();
            ApplicationComponentProvider.CpapSourceValidator = new CpapSourceValidator();
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

        public static ICpapSourceValidator CpapSourceValidator
        {
            get;
#if DEBUG
            set;
#endif
        }
    }
}
