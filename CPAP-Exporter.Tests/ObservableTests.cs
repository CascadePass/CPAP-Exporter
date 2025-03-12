using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class ObservableTests
    {
        [TestMethod]
        public void Observable_PropertyChanged_RaisedOnPropertyChange()
        {
            var observable = new ObservableTestClass();
            bool eventRaised = false;
            observable.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(observable.PropertyData))
                {
                    eventRaised = true;
                }
            };

            observable.PropertyData = "New Value";

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void Observable_PropertyChanged_NotRaisedIfValueUnchanged()
        {
            var observable = new ObservableTestClass();
            bool eventRaised = false;
            observable.PropertyChanged += (sender, e) => eventRaised = true;

            observable.PropertyData = observable.PropertyData;

            Assert.IsFalse(eventRaised);
        }

        [TestMethod]
        public void Observable_PropertyChanged_RaisedForMultipleProperties()
        {
            var observable = new ObservableTestClass();
            var changedProperties = new List<string>();
            observable.PropertyChanged += (sender, e) => changedProperties.Add(e.PropertyName);

            observable.PropertyData = "New Value 1";
            observable.SecondProperty = "New Value 2";

            CollectionAssert.AreEqual(new[] { nameof(observable.PropertyData), nameof(observable.SecondProperty) }, changedProperties);
        }

        [TestMethod]
        public void Observable_PropertyChanged_HandlerReceivesCorrectPropertyName()
        {
            var observable = new ObservableTestClass();
            string propertyName = null;
            observable.PropertyChanged += (sender, e) => propertyName = e.PropertyName;

            observable.PropertyData = "New Value";

            Assert.AreEqual(nameof(observable.PropertyData), propertyName);
        }
    }

    public class ObservableTestClass : Observable
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
