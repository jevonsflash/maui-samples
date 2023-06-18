using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace CircleWidget.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleSlider : CircleProgressBase
    {

        private const double DEFAULT_THUMB_SIZE = 20.0;

        public CircleSlider()
        {
            InitializeComponent();
            this.ValueChanged +=CircleSlider_PropertyChanged; ;
            this.labelView.TextColor = this.ProgressColor;
            ThumbContent.HeightRequest=DEFAULT_THUMB_SIZE;
            ThumbContent.WidthRequest=DEFAULT_THUMB_SIZE;
            ThumbBorderLayout.CornerRadius=DEFAULT_THUMB_SIZE;

        }

        public static readonly BindableProperty LabelContentProperty =
        BindableProperty.Create("LabelContent", typeof(View), typeof(CircleSlider), default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var obj = (CircleSlider)bindable;
            if (newValue is not null)
            {
                obj.MainContent.Content=newValue as View;
                obj.labelView.IsVisible=false;
            }
            else
            {
                obj.labelView.IsVisible=true;
            }
        });

        public override View LabelContent
        {
            get { return (View)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }

        public static readonly BindableProperty MaximumProperty =
    BindableProperty.Create("Maximum", typeof(double), typeof(CircleSlider), 1.0, propertyChanged: (bindable, oldValue, newValue) =>
    {
        var obj = (CircleSlider)bindable;
        obj.canvasView?.InvalidateSurface();
    });

        public override double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly BindableProperty MinimumProperty =
  BindableProperty.Create("Minimum", typeof(double), typeof(CircleSlider), 0.0, propertyChanged: (bindable, oldValue, newValue) =>
  {
      var obj = (CircleSlider)bindable;
      obj.canvasView?.InvalidateSurface();

  });

        public override double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }


        public static readonly BindableProperty ProgressColorProperty =
BindableProperty.Create("ProgressColor", typeof(Color), typeof(CircleSlider), Colors.Red, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;
    obj.ArcPaint.Color = obj.ProgressColor.ToSKColor();
    obj.labelView.TextColor = obj.ProgressColor;
});

        public override Color ProgressColor
        {
            get { return (Color)GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }


        public static readonly BindableProperty ContainerColorProperty =
BindableProperty.Create("ContainerColor", typeof(Color), typeof(CircleSlider), Colors.White, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;
    obj.OutlinePaint.Color = obj.ContainerColor.ToSKColor();

});

        public override Color ContainerColor
        {
            get { return (Color)GetValue(ContainerColorProperty); }
            set { SetValue(ContainerColorProperty, value); }
        }



        public static readonly BindableProperty ProgressProperty =
  BindableProperty.Create("Progress", typeof(double), typeof(CircleSlider), 0.5, defaultBindingMode:BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
  {
      var obj = (CircleSlider)bindable;
      var valueChangedSpan = (double)oldValue - (double)newValue;
      if (Math.Abs(valueChangedSpan) > ANIMATE_THROTTLE)
      {
          obj.UpdateProgressWithAnimate();
          Debug.WriteLine("valueChangedSpan is " + valueChangedSpan + " ,triggered animate");
      }
      else
      {
          obj.UpdateProgress();
          Debug.WriteLine("valueChangedSpan is " + valueChangedSpan);
      }
  });

        public override double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }




        public static readonly BindableProperty BorderWidthProperty =
BindableProperty.Create("BorderWidth", typeof(double), typeof(CircleSlider), 20.0, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;
    obj.RefreshMainRectPadding();
    obj.ArcPaint.StrokeWidth = Convert.ToSingle(newValue);
    obj.OutlinePaint.StrokeWidth = Convert.ToSingle(newValue);
});

        public override double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }




        public static readonly BindableProperty AnimationLengthProperty =
BindableProperty.Create("AnimationLength", typeof(double), typeof(CircleSlider), 1000.0, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;


});

        public override double AnimationLength
        {
            get { return (double)GetValue(AnimationLengthProperty); }
            set
            {
                SetValue(AnimationLengthProperty, value);
                OnPropertyChanged();

            }
        }


        public static readonly BindableProperty ThumbSizeProperty =
BindableProperty.Create("ThumbSize", typeof(double), typeof(CircleSlider), DEFAULT_THUMB_SIZE, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;
    obj.ThumbContent.HeightRequest=(double)newValue;
    obj.ThumbContent.WidthRequest=(double)newValue;
    obj.ThumbBorderLayout.CornerRadius=(double)newValue;

});

        public double ThumbSize
        {
            get { return (double)GetValue(ThumbSizeProperty); }
            set { SetValue(ThumbSizeProperty, value); }
        }

        public static readonly BindableProperty ThumbColorProperty =
BindableProperty.Create("ThumbColor", typeof(Color), typeof(CircleSlider), Colors.White, propertyChanged: (bindable, oldValue, newValue) =>
{
var obj = (CircleSlider)bindable;
obj.ThumbBorder.Background = obj.ThumbColor;

});

        public Color ThumbColor
        {
            get { return (Color)GetValue(ThumbColorProperty); }
            set { SetValue(ThumbColorProperty, value); }
        }



        protected override void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKRect rect = new SKRect(_mainRectPadding, _mainRectPadding, info.Width - _mainRectPadding, info.Height - _mainRectPadding);
            float startAngle = -90;
            float sweepAngle = (float)((_realtimeProgress / SumValue) * 360);

            canvas.DrawOval(rect, OutlinePaint);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);

                canvas.DrawPath(path, ArcPaint);
            }
            var thumbX = Math.Sin(sweepAngle * Math.PI / 180) * (this.Width/2-1.25*this._mainRectPadding);
            var thumbY = Math.Cos(sweepAngle * Math.PI / 180) * (this.Height / 2-1.25*this._mainRectPadding);

            this.ThumbContent.TranslationX=thumbX;
            this.ThumbContent.TranslationY=-thumbY;

        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var thumb = sender as ContentView;
            var PositionX = thumb.TranslationX+e.TotalX;
            var PositionY = thumb.TranslationY+e.TotalY;

            //for test 
            //this.test.TranslationX = thumb.TranslationX+e.TotalX;
            //this.test.TranslationY = thumb.TranslationY+e.TotalY;

            var sweepAngle = AngleNormalize(Math.Atan2(PositionX, -PositionY)*180/Math.PI);

            var targetProgress = sweepAngle*SumValue/360;
            this.Progress=targetProgress;

        }
        private double AngleNormalize(double value)
        {
            double twoPi = 360;
            while (value <= -180) value += twoPi;
            while (value >   180) value -= twoPi;
            value= (value + twoPi) % twoPi;
            return value;
        }


        private void CircleSlider_PropertyChanged(object sender, double e)
        {
            this.labelView.Text = e.ToString(LABEL_FORMATE);
            this.canvasView?.InvalidateSurface();
        }

    }
}