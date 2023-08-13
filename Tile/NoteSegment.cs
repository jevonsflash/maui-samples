using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tile
{
    public class NoteSegment 
    {
        public NoteSegment()
        {

        }

        public long Id { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int Rank { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }
    }
}
