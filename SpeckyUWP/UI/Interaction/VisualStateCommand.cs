using Specky.Commands;
using Specky.UI.Extensions;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Specky.UI.Interaction
{
    [Bindable]
    public class VisualStateCommand : DependencyObject
    {
        public const string DefaultState = "PrimaryCommandState";
        private const string VisualStatePropertyName = "VisualState";
        private const string StateControlPropertyName = "StateControl";
        private const string DataContextPropertyName = "DataContext";
        private const string CommandPropertyName = "Command";
        private const string CommandParamaterPropertyName = "CommandParameter";

        public static void SetVisualState(ButtonBase frameworkElement, string value)
            => frameworkElement.SetValue(VisualStateProperty, value);
        public static string GetVisualState(ButtonBase frameworkElement)
            => (string)frameworkElement.GetValue(VisualStateProperty);
        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.RegisterAttached(VisualStatePropertyName, typeof(string), typeof(VisualStateCommand), new PropertyMetadata(DefaultState));


        public static string GetStateControl(ButtonBase button)
            => (string)button.GetValue(StateControlProperty);
        public static void SetStateControl(ButtonBase button, string value)
            => button.SetValue(StateControlProperty, value);
        public static readonly DependencyProperty StateControlProperty =
            DependencyProperty.RegisterAttached(StateControlPropertyName,
                                                typeof(string),
                                                typeof(VisualStateCommand),
                                                new PropertyMetadata(string.Empty, new PropertyChangedCallback((s, e) =>
                                                {
                                                    var button = s as ButtonBase;

                                                    button.Loaded += (ss, ee) => setUp();
                                                    setUp();

                                                    void setUp()
                                                    {
                                                        var dialogName = (string)e.NewValue;

                                                        if (string.IsNullOrEmpty(dialogName))
                                                            return;

                                                        var dialog = button.FindControl(dialogName);

                                                        if (dialog == null)
                                                            return;

                                                        var showState = GetVisualState(button);
                                                        var dataContext = GetDataContext(button);
                                                        var originalCommand = button.Command;                                                 
                                                        button.Command = new RelayCommand(obj => setStateControl(button, dialog, showState, dataContext, originalCommand, button.CommandParameter));
                                                    }                                                    
                                                })));

        private static void setStateControl(ButtonBase button, Control dialog, string showState, object dataContext, ICommand originalCommand, object commandParameter)
        {
            SetDataContext(dialog, dataContext);
            SetCommand(dialog, originalCommand);
            SetCommandParameter(dialog, commandParameter);
            VisualStateManager.GoToState(dialog, showState, true);
        }

        public static void SetDataContext(UIElement frameworkElement, object value)
            => frameworkElement.SetValue(DataContextProperty, value);
        public static object GetDataContext(UIElement frameworkElement)
            => frameworkElement.GetValue(DataContextProperty);
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.RegisterAttached(DataContextPropertyName, typeof(object), typeof(VisualStateCommand), new PropertyMetadata(null));

        public static void SetCommand(UIElement frameworkElement, ICommand value)
            => frameworkElement.SetValue(CommandProperty, value);
        public static ICommand GetCommand(UIElement frameworkElement)
            => (ICommand)frameworkElement.GetValue(CommandProperty);
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(CommandPropertyName, typeof(ICommand), typeof(VisualStateCommand), new PropertyMetadata(null));

        public static void SetCommandParameter(UIElement frameworkElement, object value)
            => frameworkElement.SetValue(CommandProperty, value);
        public static object GetCommandParameter(UIElement frameworkElement)
            => frameworkElement.GetValue(CommandParameterProperty);
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached(CommandParamaterPropertyName, typeof(object), typeof(VisualStateCommand), new PropertyMetadata(null));
    }
}
