using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Core.Models
{
    public class MenuCellInfo
    {
        public MenuCellInfo()
        {
            Enable = true;
        }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Enable { get; set; }
    }
}
