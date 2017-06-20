using Specky.Commands;
using Specky.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Specky.UI.Interaction
{
    public class VisualStateCommand : DependencyObject
    {
        public const string DefaultState = "PrimaryCommandState";
        private const string VisualStatePropertyName = "VisualState";
        private const string StateControlPropertyName = "StateControl";

        public static void SetVisualState(ButtonBase frameworkElement, string value)
        {
            frameworkElement.SetValue(VisualStateProperty, value);            
        }

        public static string GetVisualState(ButtonBase frameworkElement)
        {
            return (string)frameworkElement.GetValue(VisualStateProperty);
        }

        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.RegisterAttached(VisualStatePropertyName, typeof(string), typeof(VisualStateCommand), new PropertyMetadata(DefaultState));


        public static string GetStateControl(ButtonBase button)
        {
            return (string)button.GetValue(StateControlProperty);
        }

        public static void SetStateControl(ButtonBase button, string value)
        {
            button.SetValue(StateControlProperty, value);
        }

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
                                                        button.Command = new RelayCommand(obj => setState(dialog, showState));
                                                    }                                                    
                                                })));

        private static void setState(Control dialog, string showState)
        {
            VisualStateManager.GoToState(dialog, showState, true);
        }
    }
}
