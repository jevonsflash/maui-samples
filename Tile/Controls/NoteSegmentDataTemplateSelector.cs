
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tile.Controls
{
    public class NoteSegmentDataTemplateSelector : DataTemplateSelector
    {

        public object ResourcesContainer { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null)
            {
                return default;
            }
            if (item is INoteSegmentService)
            {
                var dataTemplateName = (item as INoteSegmentService).NoteSegment.Type;
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
