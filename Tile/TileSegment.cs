using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tile
{
    public class TileSegment 
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
        public string Icon { get; set; }
        public Color Color { get; set; }
    }
}
