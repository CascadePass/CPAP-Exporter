namespace CascadePass.CPAPExporter.UI.Tests.ViewModels
{
    public class MockValidator : IValidatable
    {
        public MockValidator(bool returnValue)
        {
            this.DesiredReturnValue = returnValue;
        }

        public bool DesiredReturnValue { get; set; }

        public bool Validate()
        {
            return this.DesiredReturnValue;
        }
    }
}
