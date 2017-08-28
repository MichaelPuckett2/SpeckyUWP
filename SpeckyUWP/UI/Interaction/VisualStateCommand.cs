using SpeckyUWP.Commands;
using SpeckyUWP.UI.Extensions;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace SpeckyUWP.UI.Interaction
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
        private const string CheckedVisualStatePropertyName = "CheckedVisualState";
        private const string NotCheckedVisualStatePropertyName = "NotCheckedVisualState";

        public static void SetVisualState(ButtonBase frameworkElement, string value)
            => frameworkElement.SetValue(VisualStateProperty, value);
        public static string GetVisualState(ButtonBase frameworkElement)
            => (string)frameworkElement.GetValue(VisualStateProperty);
        /// <summary>
        /// The name of the VisualState to set when the button command is triggered.
        /// Note: This will remove the original button command, if any exists, and send it to the StateControl under the property VisualStateCommand.Command which can be bound to as needed.
        /// </summary>
        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.RegisterAttached(VisualStatePropertyName, typeof(string), typeof(VisualStateCommand), new PropertyMetadata(DefaultState));

        public static string GetStateControl(ButtonBase button)
            => (string)button.GetValue(StateControlProperty);
        public static void SetStateControl(ButtonBase button, string value)
            => button.SetValue(StateControlProperty, value);
        /// <summary>
        /// The name of the control that owns the VisualStates.
        /// </summary>
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

                                                        var dataContext = GetDataContext(button);
                                                        var originalCommand = button.Command;
                                                        button.Command = new RelayCommand(obj => setStateControl(button, dialog, dataContext, originalCommand, button.CommandParameter));
                                                    }
                                                })));

        private static string getToggleButtonState(ToggleButton toggleButton)
            => (toggleButton.IsChecked).GetValueOrDefault() ? GetCheckedVisualState(toggleButton) 
                                                            : GetNotCheckedVisualState(toggleButton);

        #region ToggleButtonAttachedProperties

        public static string GetCheckedVisualState(ToggleButton toggleButton)
            => (string)toggleButton.GetValue(CheckedVisualStateProperty);
        public static void SetCheckedVisualState(ToggleButton toggleButton, string value)
            => toggleButton.SetValue(CheckedVisualStateProperty, value);
        /// <summary>
        /// The name of the VisualState to set when IsChecked=True.
        /// </summary>
        public static readonly DependencyProperty CheckedVisualStateProperty =
            DependencyProperty.RegisterAttached(CheckedVisualStatePropertyName, typeof(string), typeof(VisualStateCommand), new PropertyMetadata(string.Empty));

        public static string GetNotCheckedVisualState(ToggleButton toggleButton)
            => (string)toggleButton.GetValue(NotCheckedVisualStateProperty);
        public static void SetNotCheckedVisualState(ToggleButton toggleButton, string value)
            => toggleButton.SetValue(NotCheckedVisualStateProperty, value);
        /// <summary>
        /// The name of the VisualState to set when IsChecked=False.
        /// </summary>
        public static readonly DependencyProperty NotCheckedVisualStateProperty =
            DependencyProperty.RegisterAttached(NotCheckedVisualStatePropertyName, typeof(string), typeof(VisualStateCommand), new PropertyMetadata(string.Empty));

        #endregion

        private static void setStateControl(ButtonBase button, Control dialog, object dataContext, ICommand originalCommand, object commandParameter)
        {
            string showState = string.Empty;

            if (button is ToggleButton toggleButton)
                showState = getToggleButtonState(toggleButton);

            if (string.IsNullOrEmpty(showState))
                showState = GetVisualState(button);

            SetDataContext(dialog, dataContext);
            SetCommand(dialog, originalCommand);
            SetCommandParameter(dialog, commandParameter);
            VisualStateManager.GoToState(dialog, showState, true);
        }

        public static void SetDataContext(UIElement frameworkElement, object value)
            => frameworkElement.SetValue(DataContextProperty, value);
        public static object GetDataContext(UIElement frameworkElement)
            => frameworkElement.GetValue(DataContextProperty);
        /// <summary>
        /// The VisualStateCommand.DataContext.  It is set by the button that is pressed and can be bound by the StateControl for use.  This allows for dynamically passing DataContext to the StateControl.
        /// </summary>
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
        /// <summary>
        /// The CommandParameter used by the button sending the command and passed to the StateControl for binding as needed.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached(CommandParamaterPropertyName, typeof(object), typeof(VisualStateCommand), new PropertyMetadata(null));
    }
}

/*  EXAMPLE OF USEAGE

 <!-- In this example the Page 'page' is the StateControl.  It is set via VisualStateCommand.StateControl by name and contains the VisualStates we wish to Command. -->
 <Page x:Name="page"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      xmlns:SpeckyInteraction="using:Specky.UI.Interaction"
      x:Class="SpecktUI_UWP_Tester.MainPage"
      mc:Ignorable="d">

    <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        <VisualStateManager.VisualStateGroups>
            <VisualStateGroup x:Name="DialogGroup">
                <VisualState x:Name="ShowDialogState">
                    <VisualState.Setters>
                        <Setter Target="textBlock.Visibility"
                                Value="Visible" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="HideDialogState">
                    <VisualState.Setters>
                        <Setter Target="textBlock.Visibility"
                                Value="Collapsed" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>

        <Grid Name="textBlock"
                    Visibility="Collapsed"
                    Width="300"
                    Height="120">
            <Rectangle Fill="Red" />
            <TextBlock Text="{Binding (SpeckyInteraction:VisualStateCommand.DataContext), ElementName=page}"
                       HorizontalAlignment="Center"
                       VerticalAlignment="Center" />
        </Grid>

        <StackPanel VerticalAlignment="Center">
            <Button Content="Show Dialog 1"
                    SpeckyInteraction:VisualStateCommand.StateControl="page" <!-- The page control we wish to set the VisualState on -->
                    SpeckyInteraction:VisualStateCommand.VisualState="ShowDialogState" <!-- The VisualState we wish to set on the page -->
                    SpeckyInteraction:VisualStateCommand.DataContext="Button 1"/>  <!-- The DataContext we wish to pass to the page when we set this VisualState -->

            <Button Content="Show Dialog 2"
                    SpeckyInteraction:VisualStateCommand.StateControl="page"
                    SpeckyInteraction:VisualStateCommand.VisualState="ShowDialogState"
                    SpeckyInteraction:VisualStateCommand.DataContext="Button 2" />

            <Button Content="Close Dialog"
                    SpeckyInteraction:VisualStateCommand.StateControl="page"
                    SpeckyInteraction:VisualStateCommand.VisualState="HideDialogState" />

            <CheckBox Content="Show or Hide"
                      SpeckyInteraction:VisualStateCommand.StateControl="page"
                      SpeckyInteraction:VisualStateCommand.CheckedVisualState="ShowDialogState"
                      SpeckyInteraction:VisualStateCommand.NotCheckedVisualState="HideDialogState"
                      SpeckyInteraction:VisualStateCommand.DataContext="CheckBox" />
        </StackPanel>

    </Grid>
</Page>

*/
