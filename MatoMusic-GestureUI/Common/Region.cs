using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Common
{
    public class Region
    {
        public Region()
        {
            
        }
        public Region(double sx, double ex, double sy, double ey, string name = "") : this()
        {

            StartX = sx;
            EndX = ex;
            StartY = sy;
            EndY = ey;
            Name = name;
        }

        public string Name { get; set; }
        public double StartX { get; set; }
        public double EndX { get; set; }
        public double StartY { get; set; }
        public double EndY { get; set; }
    }
}
