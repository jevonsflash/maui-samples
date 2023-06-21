using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Diagnostics;

namespace AnalogClock.Controls;

public partial class Clock1 : ContentView
{

    IDispatcherTimer _timer;
    public Clock1()
    {
        InitializeComponent();
        this.SizeChanged+=ContentView_SizeChanged;
        _timer=  Dispatcher.CreateTimer();
        _timer.Interval=TimeSpan.FromSeconds(1);
        _timer.Tick+=_timer_Tick;
        _timer.Start();
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
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawLine(0, 0, 0, -r*(float)0.62, strokePaint);
            canvas.Restore();

            // Second hand
            strokePaint.Color = SKColor.Parse("#657E3F");
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Second);
            strokePaint.StrokeWidth *=(float)0.5;
            strokePaint.Style=SKPaintStyle.Fill;

            canvas.DrawCircle(0, -r*(float)0.8, strokePaint.StrokeWidth, strokePaint);

            strokePaint.Color = SKColors.Black;
            strokePaint.StrokeWidth = 1;
            var dateStr = dateTime.ToString("M");
            var pathAngle = 20;
            var startAngle = 90-pathAngle;
            var sweepAngle = pathAngle*2;
            var rect = new SKRect(-r*(float)0.8, -r*(float)0.8, r*(float)0.8, r*(float)0.8);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawTextOnPath(dateStr, path, new SKPoint(), strokePaint);
            }


            canvas.Restore();
        }
    }

    private void ContentView_SizeChanged(object sender, EventArgs e)
    {

        var length = (float)Math.Min(this.Width, this.Height) * 0.95;
        var centerX = (float)this.Width / 2;
        var centerY = (float)this.Height / 2;
        var points = new List<Point>();
        var r = length / 2;
        var r2 = r * 1.1;
        var r3 = r * 0.9;
        //var r2 = r * 1.06;
        //var r3 = r * 1;
        var index = 0;
        var segments = 40;
        for (var i = 0; i < segments; i += 2)
        {
            var x = r * Math.Cos(i * 2 * Math.PI / segments) + centerX;
            var y = r * Math.Sin(i * 2 * Math.PI / segments) + centerY;

            points.Add(new Point((float)x, (float)y));
            var currentR = index++ % 2 == 0 ? r2 : r3;
            x = currentR * Math.Cos((i + 1) * 2 * Math.PI / segments) + centerX;
            y = currentR * Math.Sin((i + 1) * 2 * Math.PI / segments) + centerY;

            points.Add(new Point((float)x, (float)y));
        }
        MainPathFigure.StartPoint=points[0];

        points.Add(points[0]);

        for (var i = 0; i < points.Count - 2; i += 2)
        {
            var currentPoint = points[i];
            var centerPoint = points[i + 1];
            var nextPoint = points[i + 2];
            MainPathSegmentCollection.Add(new BezierSegment(
                currentPoint, centerPoint, nextPoint

          ));
        }
    }

}