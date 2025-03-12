using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CascadePass.CPAPExporter.UI.Tests
{
    [TestClass]
    public class DelegateCommandTests
    {
        [TestMethod]
        public void DelegateCommand_Execute_IsCalled()
        {
            bool executeCalled = false;
            Action execute = () => executeCalled = true;
            var command = new DelegateCommand(execute);

            command.Execute(null);

            Assert.IsTrue(executeCalled);
        }

        [TestMethod]
        public void DelegateCommand_CanExecute_ReturnsTrue()
        {
            Func<bool> canExecute = () => true;
            var command = new DelegateCommand(() => { }, canExecute);

            bool result = command.CanExecute(null);

            Assert.IsTrue(result);
        }

        //[TestMethod]
        public void DelegateCommand_CanExecuteChanged_IsRaised()
        {
            bool eventRaised = false;
            var command = new DelegateCommand(() => { });
            command.CanExecuteChanged += (s, e) => eventRaised = true;

            CommandManager.InvalidateRequerySuggested();

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void DelegateCommand_Execute_PassesParameter()
        {
            object parameter = new object();
            object receivedParameter = null;
            Action<object> execute = p => receivedParameter = p;
            var command = new DelegateCommand(execute);

            command.Execute(parameter);

            Assert.AreEqual(parameter, receivedParameter);
        }

        [TestMethod]
        public void DelegateCommand_CanExecute_PassesParameter()
        {
            // Arrange
            object parameter = new object();
            object receivedParameter = null;
            Func<object, bool> canExecute = p => { receivedParameter = p; return true; };
            var command = new DelegateCommand(_ => { }, canExecute);

            // Act
            command.CanExecute(parameter);

            // Assert
            Assert.AreEqual(parameter, receivedParameter);
        }

    }
}
