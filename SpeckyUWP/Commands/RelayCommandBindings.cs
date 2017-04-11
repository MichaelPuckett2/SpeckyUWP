using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Specky.Commands
{
    public sealed class RelayCommandBindings
    {
        public Action<object> Execute { get; set; }
        public IEnumerable<Predicate<object>> CanExecuteChecks { get; set; }
        public INotifyPropertyChanged BindingModel { get; set; }
        public IEnumerable<string> BindingProperties { get; set; }
        public RelayCommandBindings CopyBindings()
            => new RelayCommandBindings()
            {
                BindingModel = BindingModel,
                BindingProperties = BindingProperties,
                CanExecuteChecks = CanExecuteChecks,
                Execute = Execute
            };
    }
}