namespace CascadePass.CPAPExporter
{
    public class PageViewModel : ViewModel, IValidatable
    {
        #region Fields

        private bool isBusy, isBannerVisible;
        private string title, pageDescription;
        private ExportParameters exportParameters;
        private IValidatable validationProvider;

        #endregion

        #region Constructor

        public PageViewModel(string pageTitle, string pageDescription)
        {
            this.validationProvider = this;
            this.Title = pageTitle;
            this.PageDescription = pageDescription;
        }

        #endregion

        #region Properties

        public IValidatable ValidationProvider
        {
            get => this.validationProvider;
//#if DEBUG
            set => this.SetPropertyValue(ref this.validationProvider, value, nameof(this.ValidationProvider));
//#endif
        }

        public ExportParameters ExportParameters
        {
            get => this.exportParameters;
            set => this.SetPropertyValue(ref this.exportParameters, value, nameof(this.ExportParameters));
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the UI in a busy state.
        /// </summary>
        public bool IsBusy
        {
            get => this.isBusy;
            set => this.SetPropertyValue(ref this.isBusy, value, nameof(this.IsBusy));
        }

        public bool IsBannerVisible
        {
            get => this.isBannerVisible;
            set => this.SetPropertyValue(ref this.isBannerVisible, value, nameof(this.IsBannerVisible));
        }

        public bool IsValid => this.ValidationProvider.Validate();

        public string Title
        {
            get => this.title;
            set => this.SetPropertyValue(ref this.title, value, nameof(this.Title));
        }

        public string PageDescription
        {
            get => this.pageDescription;
            set => this.SetPropertyValue(ref this.pageDescription, value, nameof(this.PageDescription));
        }

        #endregion

        #region Methods

        public virtual bool Validate()
        {
            return true;
        }

        #endregion
    }
}
