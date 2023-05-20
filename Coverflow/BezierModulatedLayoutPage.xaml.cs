using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using SkiaSharp.Views;
using SkiaSharp.Views.Maui;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coverflow;

public partial class BezierModulatedLayoutPage : ContentPage
{

    public BezierModulatedLayoutPage()
    {
        InitializeComponent();
        Loaded+=BezierModulatedLayoutPage_Loaded;
    }

    private void BezierModulatedLayoutPage_Loaded(object sender, EventArgs e)
    {

    }






    public double Modulate(double value, double[] source, double[] target)
    {
        if (source.Length != 2 || target.Length != 2)
        {
            throw new ArgumentOutOfRangeException();
        }

        var start = source[0];
        var end = source[1];
        var targetStart = target[0];
        var targetEnd = target[1];
        if (value < start || value > end)
        {
            return value;
        }
        var k = (value - start) / (end - start);
        var result = k * (targetEnd - targetStart) + targetStart;
        return result;
    }



    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {

    }

}