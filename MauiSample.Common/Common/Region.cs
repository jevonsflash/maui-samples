﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSample.Common.Common
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

        public double Width  => EndX - StartX;
        public double Height => EndY - StartY;



        public bool Contains(Region content)
        {
            var translationX = 0;
            var translationY = 0;
            var isXin = translationX >= this.StartX - content.Width / 2 && translationX <= this.EndX - content.Width / 2;
            var isYin = translationY >= this.StartY - content.Height / 2 && translationY <= this.EndY - content.Height / 2;
            return isXin && isYin;
        }

        public bool Contains(Point point)
        {
            var translationX = 0;
            var translationY = 0;
            var isXin = translationX >= this.StartX - point.X && translationX <= this.EndX - point.X;
            var isYin = translationY >= this.StartY - point.Y && translationY <= this.EndY - point.Y;
            return isXin && isYin;
        }
    }


}
