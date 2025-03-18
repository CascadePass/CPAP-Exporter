namespace CascadePass.CPAPExporter.UI.Tests
{
    public class MockObservable : Observable
    {
        private string propertyData, secondProperty;

        public string PropertyData {
            get => this.propertyData;
            set => this.SetPropertyValue(ref this.propertyData, value, nameof(this.PropertyData));
        }

        public string SecondProperty
        {
            get => this.secondProperty;
            set => this.SetPropertyValue(ref this.secondProperty, value, nameof(this.SecondProperty));
        }
    }
}
