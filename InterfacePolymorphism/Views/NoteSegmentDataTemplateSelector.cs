using Lession1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lession1.Views
{
    public class BlogDataTemplateSelector : DataTemplateSelector
    {

        public object ResourcesContainer { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null)
            {
                return default;
            }
            if (item is Blog)
            {
                var dataTemplateName = (item as Blog).Type;
                if (dataTemplateName == null) { return default; }
                if (ResourcesContainer == null)
                {
                    return Application.Current.Resources[dataTemplateName] as DataTemplate;
                }
                return (ResourcesContainer as VisualElement).Resources[dataTemplateName] as DataTemplate;

            }
            return default;

        }
    }
}
