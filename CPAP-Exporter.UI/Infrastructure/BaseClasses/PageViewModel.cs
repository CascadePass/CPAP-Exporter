namespace CascadePass.CPAPExporter
{
    public class PageViewModel : ViewModel, IValidatable
    {
        #region Fields

        private bool isBusy, isBannerVisible;
        private string title, pageDescription;
        private ExportParameters exportParameters;
        private IValidatable validationProvider;
        private object statusContent;
        private DateTime becameVisible;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the current page determines that the UI should advance to the next page.
        /// </summary>
        /// <remarks>
        /// Subscribers to this event should handle the logic required to transition to the next page
        /// in the user interface. Ensure that event handlers are properly unsubscribed to avoid memory leaks.
        /// </remarks>
        public event EventHandler<EventArgs> AdvancePage;

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
#if DEBUG
            set => this.SetPropertyValue(ref this.validationProvider, value, nameof(this.ValidationProvider));
#endif
        }

        public ExportParameters ExportParameters
        {
            get => this.exportParameters;
            set => this.SetPropertyValue(ref this.exportParameters, value, nameof(this.ExportParameters));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ViewModel is in a busy state.
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

        public DateTime BecameVisible
        {
            get => this.becameVisible;
            set => this.SetPropertyValue(ref this.becameVisible, value, nameof(this.BecameVisible));
        }

        public object StatusContent
        {
            get => this.statusContent;
            set => this.SetPropertyValue(ref this.statusContent, value, nameof(this.StatusContent));
        }

        #endregion

        #region Methods

        public virtual bool Validate()
        {
            return true;
        }

        #region Events

        /// <summary>
        /// Raises the <see cref="AdvancePage"/> event with default parameters.
        /// </summary>
        /// <remarks>
        /// This method raises the event using <see cref="EventArgs.Empty"/> as the event arguments.
        /// Use this when no additional data needs to be provided to subscribers.
        /// </remarks>
        internal void OnAdvancePage()
        {
            this.AdvancePage?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="AdvancePage"/> event with the specified sender and event arguments.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eventArgs">An <see cref="EventArgs"/> instance containing additional information for subscribers.</param>
        /// <remarks>
        /// This method allows for custom event arguments to be passed to subscribers.
        /// </remarks>
        internal void OnAdvancePage(object sender, EventArgs eventArgs)
        {
            this.AdvancePage?.Invoke(sender, eventArgs);
        }

        #endregion

        #endregion
    }
}
