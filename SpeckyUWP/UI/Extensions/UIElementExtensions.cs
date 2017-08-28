using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpeckyUWP.UI.Extensions
{
    public static partial class UIElementExtensions
    {
        public static IEnumerable<UIElement> GetInnerElements(this UIElement element)
        {
            var uiElements = getSingleElement();

            switch (element)
            {
                case Panel panel:
                    foreach (var child in panel.Children)
                        uiElements = uiElements.Concat(GetInnerElements(child));
                    break;
                case UserControl userControl:
                    uiElements = uiElements.Concat(GetInnerElements(userControl.Content));
                    break;
                case ContentControl contentControl:
                    if (contentControl.Content is UIElement uiElement)
                        uiElements = uiElements.Concat(GetInnerElements(uiElement));
                    break;
            }

            return uiElements;

            IEnumerable<UIElement> getSingleElement()
            {
                yield return element;
            }
        }

        public static Control FindControl(this UIElement element, string controlName)
        {
            var innerElements = Window.Current.Content.GetInnerElements();
            
            if (!innerElements.Contains(element))
                return null;

            return Window.Current
                         .Content
                         .GetInnerElements()
                         .OfType<Control>()
                         .Where(control => control.Name == controlName)
                         .FirstOrDefault();        
        }
    }
}
