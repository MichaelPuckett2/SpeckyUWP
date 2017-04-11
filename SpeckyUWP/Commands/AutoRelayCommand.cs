using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Specky.Commands
{
    public class AutoRelayCommand : ICommand
    {
        private readonly RelayCommandBindings relayCommandBindings;
        public event EventHandler CanExecuteChanged;

        public AutoRelayCommand(RelayCommandBindings relayCommandBindings)
        {
            this.relayCommandBindings = relayCommandBindings;

            relayCommandBindings.BindingModel.PropertyChanged += (s, e) =>
            {
                try
                {
                    if (relayCommandBindings.BindingProperties.Any(p => p == e.PropertyName))
                        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (COMException comException)
                {
                    Debug.WriteLine(string.Join(Environment.NewLine, 
                                                comException.Message, 
                                                comException.InnerException,
                                                comException.StackTrace));

                    Debug.WriteLine(string.Empty);
                }

            };
        }

        public bool CanExecute(object parameter) => (relayCommandBindings.CanExecuteChecks?.All(p => p.Invoke(parameter))).GetValueOrDefault();
        public void Execute(object parameter) => relayCommandBindings?.Execute?.Invoke(parameter);
    }
}