using Microsoft.Maui;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StickyTab.Controls;

public partial class StickyPan : ContentView
{

    private double C = 0.552284749831f;
    public StickyPan()
    {
        InitializeComponent();

        this.MainPath.Fill = (Brush)PanFillBrush;
        this.MainPath.Stroke = (Brush)PanStrokeBrush;
    }
    public double RadiusX => this.PanWidth / 2;
    public double RadiusY => this.PanHeight / 2;
    public Point Center => new Point(this.PanWidth / 2, this.PanHeight / 2);

    public double DifferenceX => RadiusX * C;
    public double DifferenceY => RadiusY * C;

    private View _panContent;
    private bool SpringbackAnimationRequired;
    private bool IsSpringbackAnimationRunning;

    public View PanContent
    {
        get { return _panContent; }
        set
        {

            _panContent = value;
            OnPropertyChanged();
            this.MainContent.Content = PanContent;
        }
    }



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



    public static readonly BindableProperty PanFillBrushProperty =
BindableProperty.Create("PanFillBrush", typeof(Brush), typeof(StickyPan), SolidColorBrush.Transparent, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;
    obj.MainPath.Fill = (Brush)newValue;

});

    public Brush PanFillBrush
    {
        get { return (Brush)GetValue(PanFillBrushProperty); }
        set
        {
            SetValue(PanFillBrushProperty, value);
            OnPropertyChanged();

        }
    }



    public static readonly BindableProperty PanStrokeBrushProperty =
BindableProperty.Create("PanStrokeBrush", typeof(Brush), typeof(StickyPan), SolidColorBrush.Red, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (StickyPan)bindable;
    obj.MainPath.Stroke = (Brush)newValue;

});

    public Brush PanStrokeBrush
    {
        get { return (Brush)GetValue(PanStrokeBrushProperty); }
        set
        {
            SetValue(PanStrokeBrushProperty, value);
            OnPropertyChanged();

        }
    }


    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
        ReRender();
    }

    private void Animate( Action<double, bool> finished = null)
    {
        var PanScaleAnimationLength = 1000;
        Content.AbortAnimation("ReshapeAnimations");
        var scaleAnimation = new Animation();


        var adjustX = (this.Width - PanWidth) / 2;
        var adjustY = (this.Height - PanHeight) / 2;

        Point p0Target = new Point(PanWidth / 2 + adjustX, adjustY);
        Point p1Target = new Point(this.PanWidth + adjustX, this.PanHeight / 2 + adjustY);
        Point p2Target = new Point(this.PanWidth / 2 + adjustX, this.PanHeight + adjustY);
        Point p3Target = new Point(adjustX, this.PanHeight / 2 + adjustY);

        Point p0Origin = this.figure1.StartPoint;
        Point p1Origin = this.arc1.Point3;
        Point p2Origin = this.arc2.Point3;
        Point p3Origin = this.arc3.Point3;


        var animateAction = (double r) =>
        {

            Point p0 = new Point((p0Target.X - p0Origin.X) * r + p0Origin.X, (p0Target.Y - p0Origin.Y) * r + p0Origin.Y);
            Point p1 = new Point((p1Target.X - p1Origin.X) * r + p1Origin.X, (p1Target.Y - p1Origin.Y) * r + p1Origin.Y);
            Point p2 = new Point((p2Target.X - p2Origin.X) * r + p2Origin.X, (p2Target.Y - p2Origin.Y) * r + p2Origin.Y);
            Point p3 = new Point((p3Target.X - p3Origin.X) * r + p3Origin.X, (p3Target.Y - p3Origin.Y) * r + p3Origin.Y);

            Point h1 = new Point(p0.X - DifferenceX, p0.Y);
            Point h2 = new Point(p0.X + DifferenceX, p0.Y);
            Point h3 = new Point(p1.X, p1.Y - DifferenceY);
            Point h4 = new Point(p1.X, p1.Y + DifferenceY);
            Point h5 = new Point(p2.X + DifferenceX, p2.Y);
            Point h6 = new Point(p2.X - DifferenceX, p2.Y);
            Point h7 = new Point(p3.X, p3.Y + DifferenceY);
            Point h8 = new Point(p3.X, p3.Y - DifferenceY);


            this.figure1.StartPoint = p0;
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
        };

        var scaleUpAnimation0 = new Animation(animateAction, 0, 1);

        scaleAnimation.Add(0, 1, scaleUpAnimation0);
        scaleAnimation.Commit(this, "ReshapeAnimations", 16, (uint)PanScaleAnimationLength, finished: finished);

    }

    private void ReRender()
    {
        if (IsSpringbackAnimationRunning)
        {
            return;
        }

        var _offsetX = OffsetX;
        //超过容忍度则将不黏连
        if (OffsetX <= -(this.Width - PanWidth) / 2 || OffsetX > (this.Width - PanWidth) / 2)
        {
            _offsetX = 0;
            if (SpringbackAnimationRequired)
            {
                IsSpringbackAnimationRunning = true;
                this.Animate( (d, b) =>
                {
                    SpringbackAnimationRequired = false;
                    IsSpringbackAnimationRunning = false;
                });
                return;
            }
        }
        else
        {
            SpringbackAnimationRequired = true;
            Debug.WriteLine("SpringbackAnimationRequired set to true!");
        }

        var _offsetY = OffsetY;
        //超过容忍度则将不黏连
        if (OffsetY <= -(this.Height - PanHeight) / 2 || OffsetY > (this.Height - PanHeight) / 2)
        {
            _offsetY = 0;
            if (SpringbackAnimationRequired)
            {
                IsSpringbackAnimationRunning = true;
                this.Animate( (d, b) =>
                {
                    SpringbackAnimationRequired = false;
                    IsSpringbackAnimationRunning = false;
                });
                return;
            }
        }
        else
        {
            SpringbackAnimationRequired = true;
            Debug.WriteLine("SpringbackAnimationRequired set to true!");
        }

        var adjustX = (this.Width - PanWidth) / 2 - _offsetX;
        var adjustY = (this.Height - PanHeight) / 2 - _offsetY;

        Point p0 = new Point(PanWidth / 2 + adjustX, adjustY);
        Point p1 = new Point(this.PanWidth + adjustX, this.PanHeight / 2 + adjustY);
        Point p2 = new Point(this.PanWidth / 2 + adjustX, this.PanHeight + adjustY);
        Point p3 = new Point(adjustX, this.PanHeight / 2 + adjustY);


        var dx = _offsetX * 0.8 + _offsetY * 0.4;
        var dy = _offsetX * 0.4 + _offsetY * 0.8;
        if (OffsetX != 0)
        {
            if (dx > 0)
            {
                p1 = p1.Offset(dx, 0);

            }
            else
            {
                p3 = p3.Offset(dx, 0);
            }
            p0 = p0.Offset(0, Math.Abs(dy));
            p2 = p2.Offset(0, -Math.Abs(dy));
        }

        else if (OffsetY != 0)
        {
            if (dy > 0)
            {
                p2 = p2.Offset(0, dy);
            }

            else
            {
                p0 = p0.Offset(0, dy);
            }
            p1 = p1.Offset(-Math.Abs(dx), 0);
            p3 = p3.Offset(Math.Abs(dx), 0);

        }



        Point h1 = new Point(p0.X - DifferenceX, p0.Y);
        Point h2 = new Point(p0.X + DifferenceX, p0.Y);
        Point h3 = new Point(p1.X, p1.Y - DifferenceY);
        Point h4 = new Point(p1.X, p1.Y + DifferenceY);
        Point h5 = new Point(p2.X + DifferenceX, p2.Y);
        Point h6 = new Point(p2.X - DifferenceX, p2.Y);
        Point h7 = new Point(p3.X, p3.Y + DifferenceY);
        Point h8 = new Point(p3.X, p3.Y - DifferenceY);


        this.figure1.StartPoint = p0;
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