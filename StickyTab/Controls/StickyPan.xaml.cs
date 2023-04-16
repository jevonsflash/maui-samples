using Microsoft.Maui;
using System.Runtime.CompilerServices;

namespace StickyTab.Controls;

public partial class StickyPan : ContentView
{

    private double C = 0.552284749831f;
    public StickyPan()
    {
        InitializeComponent();
    }
    public double RadiusX => this.Width/2;
    public double RadiusY => this.Height/2;
    public Point Center => new Point(this.Width/2, this.Height/2);

    public double DifferenceX => RadiusX * C;
    public double DifferenceY => RadiusY * C;


    public static readonly BindableProperty OffsetYProperty =
BindableProperty.Create("OffsetY", typeof(double), typeof(StickyPan), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;





});

    public double OffsetY
    {
        get { return (double)GetValue(OffsetYProperty); }
        set
        {
            SetValue(OffsetYProperty, value);
            OnPropertyChanged();

        }
    }


    public static readonly BindableProperty OffsetXProperty =
BindableProperty.Create("OffsetX", typeof(double), typeof(StickyPan), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;
    var dx = obj.OffsetX*0.8;
    var dy = obj.OffsetX*0.4;
    if (dx>0)
    {
        obj.arc1.Point3= obj.arc1.Point3.Offset(dx, 0);

    }
    else
    {
        obj.arc3.Point3= obj.arc3.Point3.Offset(dx, 0);
    }

    obj.figure1.StartPoint= obj.figure1.StartPoint.Offset(0, Math.Abs(dy));

    obj.arc2.Point3 = obj.arc2.Point3.Offset(0, -Math.Abs(dy));
    obj.arc4.Point3 = obj.arc4.Point3.Offset(0, Math.Abs(dy));

});

    public double OffsetX
    {
        get { return (double)GetValue(OffsetXProperty); }
        set
        {
            SetValue(OffsetXProperty, value);
            OnPropertyChanged();

        }
    }




    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
        Point p0 = new Point(Width/2, 0);
        Point h1 = new Point(Width/2-DifferenceX, 0);
        Point h2 = new Point(this.Width/2+DifferenceX, 0);
        Point h3 = new Point(this.Width, this.Height/2-  DifferenceY);
        Point p1 = new Point(this.Width, this.Height/2);
        Point h4 = new Point(this.Width, this.Height/2+DifferenceY);
        Point h5 = new Point(this.Width/2+DifferenceX, this.Height);
        Point p2 = new Point(this.Width/2, this.Height);
        Point h6 = new Point(this.Width/2-DifferenceX, this.Height);
        Point h7 = new Point(0, this.Height/2+DifferenceY);
        Point p3 = new Point(0, this.Height/2);
        Point h8 = new Point(0, this.Height/2-DifferenceY);


        var dx = 400*0.8;
        var dy = 400*0.4;
        if (dx>0)
        {
            p1= p1.Offset(dx, 0);

        }
        else
        {
            p3= p3.Offset(dx, 0);
        }

        p0= p0.Offset(0, Math.Abs(dy));

        p2 = p2.Offset(0, -Math.Abs(dy));
        

        this.figure2.StartPoint=p0;
        this.poly1.Points=new PointCollection() {  h2, h3, h4, h5, h6, h7, h8 , h1 };

        this.figure1.StartPoint =  p0;
        this.arc1.Point1 = h2;
        this.arc1.Point2 = h3;
        this.arc1.Point3 = p1;


        this.arc2.Point1 = h4;
        this.arc2.Point2 = h5;
        this.arc2.Point3 = p2;

        this.arc3.Point1 = h6;
        this.arc3.Point2 = h7;
        this.arc3.Point3 = p3;

        this.arc4.Point1 = h8;
        this.arc4.Point2 = h1;

        this.arc4.Point3 = p0;
    }
}