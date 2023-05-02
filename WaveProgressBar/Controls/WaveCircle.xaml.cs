using Microsoft.Maui.Controls.Shapes;
using System.Numerics;

namespace WaveProgressBar.Controls;

public partial class WaveCircle : ContentView
{
    public WaveCircle()
    {
        InitializeComponent();

    }

    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
        
        var length = (float)Math.Min(this.Width, this.Height) * 0.95;
        var centerX = (float)this.Width / 2;
        var centerY = (float)this.Height / 2;

        var points = new List<Point>();
        var r = length / 2;
        var r2 = r * 1.16;
        var r3 = r * 0.851;
        //var r2 = r * 1.06;
        //var r3 = r * 1;
        var index = 0;
        var segments = 60;
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