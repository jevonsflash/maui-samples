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





    private void RenderTransform()
    {
        var info = this.BoxLayout;
        double xCenter = info.Width / 2;
        double yCenter = info.Height / 2;


        var step = xCenter*0.15;
        var currentSlide = (int)this.BoxLayout.Children.Count/2;
        var sb = new StringBuilder();
        //var currentSlidePadding = layoutWidth * 0.7;
        var currentSlidePadding = 0;


        foreach (var bitmap in this.BoxLayout.Children)
        {

            var pos = this.BoxLayout.Children.IndexOf(bitmap);

            double xBitmap;
            if (pos < currentSlide)
            {
                xBitmap = (double)(xCenter - (currentSlide * step) + (pos * step) - (bitmap.Width / 2) - currentSlidePadding);

            }
            else if (pos > currentSlide)
            {
                xBitmap = (double)(xCenter + ((pos - currentSlide) * step) - (bitmap.Width / 2) + currentSlidePadding);
            }
            else
            {
                xBitmap = xCenter - (info.Width / 2);
            }

            double yBitmap = yCenter - info.Height / 2;

            (bitmap as VisualElement).TranslateTo(xBitmap, yBitmap, 400);


        }

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



    private void RadioButton_CheckedChanged(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            RenderTransform();

        }
        else
        {

        }
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {

    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        this.MainRotationImage.RotateY=e.NewValue;
    }
}