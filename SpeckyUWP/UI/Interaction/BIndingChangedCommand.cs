using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SpeckyUWP.UI.Interaction
{
    [Bindable]
    public class BindingChangedCommand : DependencyObject
    {
        private const string BindingCommandPropertyName = "BindingCommand";
        private const string BindingValuePropertyName = "BindingValue";
        private const string BindingCommandParameterPropertyName = "BindingCommandParameter";
        private const string IgnoreInitialChangePropertyName = "IgnoreInitialChange";

        public static ICommand GetBindingCommand(DependencyObject obj)
            => (ICommand)obj.GetValue(BindingCommandProperty);

        public static void SetBindingCommand(DependencyObject obj, ICommand value)
            => obj.SetValue(BindingCommandProperty, value);

        /// <summary>
        /// The command triggered when the BindingValue changes.
        /// </summary>
        public static readonly DependencyProperty BindingCommandProperty =
            DependencyProperty.RegisterAttached(BindingCommandPropertyName,
                                                typeof(ICommand),
                                                typeof(BindingChangedCommand),
                                                new PropertyMetadata(null));


        public static object GetBindingCommandParameter(DependencyObject obj)
            => obj.GetValue(BindingCommandParameterProperty);

        public static void SetBindingCommandParameter(DependencyObject obj, object value)
            => obj.SetValue(BindingCommandParameterProperty, value);

        /// <summary>
        /// The command parameter used when the command is triggered.
        /// </summary>
        public static readonly DependencyProperty BindingCommandParameterProperty =
            DependencyProperty.RegisterAttached(BindingCommandParameterPropertyName, typeof(object), typeof(BindingChangedCommand), new PropertyMetadata(null));
        

        public static object GetBindingValue(DependencyObject obj)
            => (obj.GetValue(BindingValueProperty));

        public static void SetBindingValue(DependencyObject obj, object value)
            => obj.SetValue(BindingValueProperty, value);

        /// <summary>
        /// The value used to triggered the command when changed.
        /// </summary>
        public static readonly DependencyProperty BindingValueProperty =
            DependencyProperty.RegisterAttached(BindingValuePropertyName, typeof(object), typeof(BindingChangedCommand), new PropertyMetadata(null, new PropertyChangedCallback((s, e) =>
            {
                var dependencyObject = s as DependencyObject;
                var command = GetBindingCommand(dependencyObject);
                var commandParameter = GetBindingCommandParameter(dependencyObject);
                var ignoreInitialChange = GetIgnoreInitialChange(dependencyObject);

                if (ignoreInitialChange)
                {
                    SetIgnoreInitialChange(dependencyObject, false);
                    return;
                }

                if (command != null && command.CanExecute(commandParameter))
                    command.Execute(commandParameter);
            })));
        

        public static bool GetIgnoreInitialChange(DependencyObject obj)
            => (bool)obj.GetValue(IgnoreInitialChangeProperty);

        public static void SetIgnoreInitialChange(DependencyObject obj, bool value)
            => obj.SetValue(IgnoreInitialChangeProperty, value);

        /// <summary>
        /// Ignores the initial binding when set to true.
        /// This value will automatically be set to false on initial binding change if set to true.
        /// </summary>
        public static readonly DependencyProperty IgnoreInitialChangeProperty =
            DependencyProperty.RegisterAttached(IgnoreInitialChangePropertyName, typeof(bool), typeof(BindingChangedCommand), new PropertyMetadata(false));
    }
}
