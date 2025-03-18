namespace CascadePass.CPAPExporter.UI.Tests
{
    public class MockPageViewModel : PageViewModel
    {
        private string propertyValue;

        public MockPageViewModel() : this("Test Page", "This is a mock test page.")
        {
        }

        public MockPageViewModel(string pageTitle, string pageDescription) : base(pageTitle, pageDescription)
        {
        }

        public string Value
        {
            get => this.propertyValue;
            set => this.SetPropertyValue(ref this.propertyValue, value, nameof(this.Value));
        }
    }
}
