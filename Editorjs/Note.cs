using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editorjs
{
    public class Note 
    {

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }

        public string PreViewContent { get; set; }

        public bool IsEditable { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }
    }
}

