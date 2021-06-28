using System.Windows;
using System.Windows.Controls;
using GothicModComposer.UI.Models;

namespace GothicModComposer.UI.Helpers
{
    public class DynamicDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null)
            {
                var model = item as IniOverride;

                return (DataTemplate)element.FindResource(model.DisplayAs + "Template");
            }

            return null;
        }
    }
}