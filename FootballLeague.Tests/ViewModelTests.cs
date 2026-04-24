using NUnit.Framework;
using System.ComponentModel;
using FootballLeague.ViewModels;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ViewModelTests
    {
        [Test]
        public void ViewModelBase_PropertyChanged_RaisesEvent()
        {
            // Arrange
            var viewModel = new TestViewModel();
            bool eventRaised = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(TestViewModel.TestProperty))
                    eventRaised = true;
            };

            // Act
            viewModel.TestProperty = "New Value";

            // Assert
            Assert.That(eventRaised, Is.True);
        }

        [Test]
        public void RelayCommand_CanExecute_ReturnsTrue()
        {
            // Arrange
            var command = new RelayCommand(_ => { });

            // Act
            var canExecute = command.CanExecute(null);

            // Assert
            Assert.That(canExecute, Is.True);
        }

        [Test]
        public void RelayCommand_Execute_CallsAction()
        {
            // Arrange
            bool actionCalled = false;
            var command = new RelayCommand(_ => actionCalled = true);

            // Act
            command.Execute(null);

            // Assert
            Assert.That(actionCalled, Is.True);
        }

        [Test]
        public void RelayCommand_WithCanExecute_RespectsCondition()
        {
            // Arrange
            bool canExecuteValue = false;
            var command = new RelayCommand(_ => { }, _ => canExecuteValue);

            // Act
            var result1 = command.CanExecute(null);
            canExecuteValue = true;
            var result2 = command.CanExecute(null);

            // Assert
            Assert.That(result1, Is.False);
            Assert.That(result2, Is.True);
        }

        private class TestViewModel : ViewModelBase
        {
            private string _testProperty = string.Empty;
            public string TestProperty
            {
                get => _testProperty;
                set => SetProperty(ref _testProperty, value);
            }
        }
    }
}
