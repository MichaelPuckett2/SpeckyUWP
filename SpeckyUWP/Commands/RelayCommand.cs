using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace SpeckyUWP.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
            canExecute = (obj) => true;
        }

        /// <summary>
        /// Constructs a relay command to execute a specific method where CanExecute updating is handled internally via binding to type of INotifyPropertyChanged.
        /// </summary>
        /// <param name="execute">The method to execute when the command is called.</param>
        /// <param name="bindingModel">The model to bind to when watching for changing values that determine CanExecute result.</param>
        /// <param name="canExecutePropertyName">The property name watched on the binding model.</param>
        /// <param name="oppositeValue">If true CanExecute will return the opposite value being watched.</param>
        public RelayCommand(Action<object> execute, INotifyPropertyChanged bindingModel, Dictionary<string, object> propertyNameValue)
        {
            this.execute = execute;

            canExecute = (obj) => propertyNameValue.All(p => (bindingModel.GetType().GetProperty(p.Key)?.GetValue(bindingModel)?.Equals(p.Value)).GetValueOrDefault()); 

            bindingModel.PropertyChanged += (s, e) =>
            {
                if (propertyNameValue.Any(p => p.Key == e.PropertyName))
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public bool CanExecute(object parameter)
            => canExecute.Invoke(parameter);

        public void Execute(object parameter)
            => execute.Invoke(parameter);

        public void Update()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
