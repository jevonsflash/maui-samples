using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Text;

namespace MultitaskingCardList;

public partial class BezierModulatedLayoutPage : ContentPage
{
    public BezierModulatedLayoutPage()
    {
        InitializeComponent();
        Loaded += BezierModulatedLayoutPage_Loaded;
        SizeChanged += BezierModulatedLayoutPage_SizeChanged;
    }

    private void BezierModulatedLayoutPage_SizeChanged(object sender, EventArgs e)
    {
        RenderTransform();
    }

    private void RenderTransform()
    {
        var layoutWidth = this.BoxLayout.DesiredSize.Width;
        if (this.BezeirPoints == null)
        {
            return;
        }
        var sb = new StringBuilder();
        foreach (var item in this.BoxLayout.Children)
        {

            if (item is VisualElement)
            {
                var progress = this.Modulate((item as VisualElement).X, new double[] { 0, layoutWidth }, new double[] { 0, 1 });
                (item as VisualElement).ScaleTo(Modulate(progress, new double[] { 0, 1 }, new double[] { 0.61, 0.72 }), 0);
                //(item as VisualElement).FadeTo(Modulate(progress, new double[] { 0.2, 0.54 }, new double[] { 0, 1 }), 0);
                var modulatedX = Modulate(1 - GetMappingY(progress), new double[] { 0, 1 }, new double[] { 0, layoutWidth });
                var offsetX = modulatedX - (item as VisualElement).X;
                (item as VisualElement).TranslateTo(offsetX, 0, 0);
                this.BoxViewsXList.ItemsSource = GetBoxViewsX();
                sb.AppendLine($"{BoxLayout.Children.IndexOf(item)}:offsetX-{offsetX}");
            }
        }
        this.Tip.Text = sb.ToString();
    }

    private void BezierModulatedLayoutPage_Loaded(object sender, EventArgs e)
    {
        BezeirPoints = new List<Point>();

        var bezeirPointSubdivs = 999;
        var Xs = GetBoxViewsX();

        this.BoxViewsXList.ItemsSource = Xs;


        var p0 = new Point(0, 1);
        var p1 = new Point(0.1, 0.9988);
        var p2 = new Point(0.175, 0.9955);


        var p3 = new Point(0.4, 0.99);
        var p4 = new Point(0.575, 0.92);
        var p5 = new Point(0.7, 0.88);

        var p6 = new Point(0.775, 0.71);
        var p7 = new Point(0.9, 0.4);
        var p8 = new Point(1, 0);

        this.BezierSegments = new Point[][] {

            new Point[]{p0,p1,p2},
            new Point[]{p2,p3,p4},
            new Point[]{p4,p5,p6},
            new Point[] {p6,p7,p8}
       };

        for (int i = 0; i < this.BezierSegments.Length; i++)
        {
            for (int j = 0; j < bezeirPointSubdivs; j++)
            {
                var bezeirPointX = Math.Pow(1 - (double)j / bezeirPointSubdivs, 2) * BezierSegments[i][0].X + 2 * (double)j / bezeirPointSubdivs * (1 - (double)j / bezeirPointSubdivs) * BezierSegments[i][1].X + Math.Pow((double)j / bezeirPointSubdivs, 2) * BezierSegments[i][2].X;
                var bezeirPointY = Math.Pow(1 - (double)j / bezeirPointSubdivs, 2) * BezierSegments[i][0].Y + 2 * (double)j / bezeirPointSubdivs * (1 - (double)j / bezeirPointSubdivs) * BezierSegments[i][1].Y + Math.Pow((double)j / bezeirPointSubdivs, 2) * BezierSegments[i][2].Y;
                BezeirPoints.Add(new Point(bezeirPointX, bezeirPointY));

            }
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

    public double GetMappingY(double x)
    {
        foreach (var item in BezeirPoints)
        {
            if (x < item.X)
            {
                return item.Y;
            }
        }
        return default;
    }

    public List<Point> BezeirPoints { get; set; }
    public Point[][] BezierSegments { get; set; }

    private List<double> GetBoxViewsX()
    {
        List<double> boxViewsX = new List<double>();
        foreach (var item in this.BoxLayout.Children)
        {
            if (item is VisualElement)
            {
                boxViewsX.Add((item as VisualElement).X);
            }
        }
        return boxViewsX;
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            RenderTransform();

        }
        else
        {
            foreach (var item in this.BoxLayout.Children)
            {

                if (item is VisualElement)
                {
                    (item as VisualElement).ScaleTo(1, 100);
                    //(item as VisualElement).FadeTo(1, 100);             
                    (item as VisualElement).TranslateTo(0, 0, 100);

                }
            }
        }
    }
}