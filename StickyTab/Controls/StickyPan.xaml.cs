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
    public double RadiusX => this.PanWidth/2;
    public double RadiusY => this.PanHeight/2;
    public Point Center => new Point(this.PanWidth/2, this.PanHeight/2);

    public double DifferenceX => RadiusX * C;
    public double DifferenceY => RadiusY * C;


    public static readonly BindableProperty OffsetYProperty =
BindableProperty.Create("OffsetY", typeof(double), typeof(StickyPan), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;

    obj.ReRender();




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
    obj.ReRender();

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


    public static readonly BindableProperty PanWidthProperty =
BindableProperty.Create("PanWidth", typeof(double), typeof(StickyPan), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;
    obj.ReRender();

});

    public double PanWidth
    {
        get { return (double)GetValue(PanWidthProperty); }
        set
        {
            SetValue(PanWidthProperty, value);
            OnPropertyChanged();

        }
    }



    public static readonly BindableProperty PanHeightProperty =
BindableProperty.Create("PanHeight", typeof(double), typeof(StickyPan), default(double), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;
    obj.ReRender();

});

    public double PanHeight
    {
        get { return (double)GetValue(PanHeightProperty); }
        set
        {
            SetValue(PanHeightProperty, value);
            OnPropertyChanged();

        }
    }



    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
        ReRender();
    }

    private void ReRender()
    {
        var _offsetX = OffsetX;
        //超过容忍度则将不黏连
        if (OffsetX<=-(this.Width-PanWidth)/2||OffsetX>(this.Width-PanWidth)/2)
        {
            _offsetX=0;
        }

        var _offsetY = OffsetY;
        //超过容忍度则将不黏连
        if (OffsetY<=-(this.Height-PanHeight)/2||OffsetY>(this.Height-PanHeight)/2)
        {
            _offsetY=0;
        }

        var adjustX = (this.Width-PanWidth)/2-_offsetX;
        var adjustY = (this.Height-PanHeight)/2-_offsetY;

        Point p0 = new Point(PanWidth/2+adjustX, adjustY);
        Point p1 = new Point(this.PanWidth+adjustX, this.PanHeight/2+adjustY);
        Point p2 = new Point(this.PanWidth/2+adjustX, this.PanHeight+adjustY);
        Point p3 = new Point(adjustX, this.PanHeight/2+adjustY);


        var dx = _offsetX*0.8+_offsetY*0.4;
        var dy = _offsetX*0.4+_offsetY*0.8;
        if (dx>0)
        {
            p1= p1.Offset(dx, 0);
            p0= p0.Offset(0, Math.Abs(dy));
            p2 = p2.Offset(0, -Math.Abs(dy));
        }
        else if (dx<0)
        {
            p3= p3.Offset(dx, 0);
            p0= p0.Offset(0, Math.Abs(dy));
            p2 = p2.Offset(0, -Math.Abs(dy));
        }
        else
        {
            if (dy>0)
            {
                //p2= p2.Offset(0, dy);
            }
        }



        Point h1 = new Point(p0.X-DifferenceX, p0.Y);
        Point h2 = new Point(p0.X+DifferenceX, p0.Y);
        Point h3 = new Point(p1.X, p1.Y-  DifferenceY);
        Point h4 = new Point(p1.X, p1.Y +DifferenceY);
        Point h5 = new Point(p2.X+DifferenceX, p2.Y);
        Point h6 = new Point(p2.X-DifferenceX, p2.Y);
        Point h7 = new Point(p3.X, p3.Y+DifferenceY);
        Point h8 = new Point(p3.X, p3.Y-DifferenceY);


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