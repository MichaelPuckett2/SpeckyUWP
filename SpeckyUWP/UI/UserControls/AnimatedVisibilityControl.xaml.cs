using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SpeckyUWP.UI.UserControls
{
    public sealed partial class AnimatedVisibilityControl : UserControl
    {
        public AnimatedVisibilityControl() => InitializeComponent();

        public new Visibility Visibility
        {
            get => (Visibility)GetValue(VisibilityProperty); set => SetValue(VisibilityProperty, value);
        }

        public static readonly new DependencyProperty VisibilityProperty =
            DependencyProperty.Register(nameof(Visibility), typeof(Visibility), typeof(AnimatedVisibilityControl), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback((s, e) =>
            {
                var animatedVisibilityControl = s as AnimatedVisibilityControl;
                var visibility = (Visibility)e.NewValue;

                switch (visibility)
                {
                    case Visibility.Visible:
                        if (animatedVisibilityControl.HookBackButtonOnVisible)
                            SystemNavigationManager.GetForCurrentView().BackRequested += animatedVisibilityControlBackRequested;

                        animatedVisibilityControl.SetBaseVisibility(visibility);
                        animatedVisibilityControl.VisibleStoryboard?.Begin();
                        break;
                    case Visibility.Collapsed:
                        SystemNavigationManager.GetForCurrentView().BackRequested -= animatedVisibilityControlBackRequested;

                        if (animatedVisibilityControl.CollapsingStoryboard != null)
                            animatedVisibilityControl.CollapsingStoryboard.Completed += collapsingStoryboardCompleted;

                        animatedVisibilityControl.CollapsingStoryboard.Begin();
                        break;
                    default:
                        break;
                }

                void collapsingStoryboardCompleted(object sender, object ee)
                {
                    animatedVisibilityControl.CollapsingStoryboard.Completed -= collapsingStoryboardCompleted;
                    animatedVisibilityControl.SetBaseVisibility(visibility);
                }

                void animatedVisibilityControlBackRequested(object sender, BackRequestedEventArgs backRequestedEvent)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= animatedVisibilityControlBackRequested;
                    animatedVisibilityControl.Visibility = Visibility.Collapsed;
                    backRequestedEvent.Handled = true;
                }
            })));

        private void SetBaseVisibility(Visibility visibility) => base.Visibility = visibility;

        /// <summary>
        /// Storyboard to trigger when the Visibility is set to Visible.
        /// </summary>
        public Storyboard VisibleStoryboard
        {
            get => (Storyboard)GetValue(VisibleStoryboardProperty); set => SetValue(VisibleStoryboardProperty, value);
        }

        public static readonly DependencyProperty VisibleStoryboardProperty =
            DependencyProperty.Register(nameof(VisibleStoryboard), typeof(Storyboard), typeof(AnimatedVisibilityControl), new PropertyMetadata(null));

        /// <summary>
        /// Storyboard to trigger when the Visibility is set to Collapse; but before it has collapsed.
        /// </summary>
        public Storyboard CollapsingStoryboard
        {
            get => (Storyboard)GetValue(CollapsingStoryboardProperty); set => SetValue(CollapsingStoryboardProperty, value);
        }

        public static readonly DependencyProperty CollapsingStoryboardProperty =
            DependencyProperty.Register(nameof(CollapsingStoryboard), typeof(Storyboard), typeof(AnimatedVisibilityControl), new PropertyMetadata(null));

        public bool HookBackButtonOnVisible
        {
            get => (bool)GetValue(HookBackButtonOnVisibleProperty); set => SetValue(HookBackButtonOnVisibleProperty, value);
        }

        public static readonly DependencyProperty HookBackButtonOnVisibleProperty =
            DependencyProperty.Register(nameof(HookBackButtonOnVisible), typeof(bool), typeof(AnimatedVisibilityControl), new PropertyMetadata(true));
    }
}
