using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;

namespace AnalogClock.Controls;

public partial class Clock2 : ContentView
{

    IDispatcherTimer _timer;
    public Clock2()
    {
        InitializeComponent();
        this.SizeChanged+=Clock2_SizeChanged;
        _timer=  Dispatcher.CreateTimer();
        _timer.Interval=TimeSpan.FromSeconds(1);
        _timer.Tick+=_timer_Tick;
        _timer.Start();
    }

    private void Clock2_SizeChanged(object sender, EventArgs e)
    {
        this.ModulatedPath.Scale=(Math.Min(this.Width / 200f, this.Height / 200f));

    }

    private void _timer_Tick(object sender, EventArgs e)
    {
        this.canvasView?.InvalidateSurface();
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear();

        using (SKPaint strokePaint = new SKPaint())
        using (SKPaint fillPaint = new SKPaint())
        {
            strokePaint.Style = SKPaintStyle.Stroke;
            strokePaint.StrokeCap = SKStrokeCap.Round;

            fillPaint.Style = SKPaintStyle.Fill;
            fillPaint.Color = SKColors.Transparent;

            // Transform for 100-radius circle centered at origin
            var r = 100f;
            canvas.Translate(info.Width / 2f, info.Height / 2f);
            canvas.Scale(Math.Min(info.Width / 200f, info.Height / 200f));


            DateTime dateTime = DateTime.Now;

            // Hour hand
            strokePaint.Color = SKColor.Parse("#5E4000");
            strokePaint.StrokeWidth = 15;
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            canvas.DrawLine(0, 0, 0, -r*(float)0.4, strokePaint);
            canvas.Restore();

            // Minute hand
            strokePaint.Color = SKColor.Parse("#9C6D00");
            strokePaint.StrokeWidth = 5;
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawLine(0, 0, 0, -r*(float)0.8, strokePaint);
            canvas.Restore();

            // Second hand
            strokePaint.Color = SKColor.Parse("#657E3F");
            strokePaint.StrokeWidth = 2;
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Second);
            canvas.DrawLine(0, r*(float)0.1, 0, -r*(float)0.8, strokePaint);
            strokePaint.Style=SKPaintStyle.Fill;
            canvas.DrawCircle(0, 0, 5, strokePaint);

            canvas.Restore();
        }
    }

}