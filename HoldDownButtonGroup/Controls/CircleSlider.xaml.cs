using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace HoldDownButtonGroup.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleSlider : ContentView, IProgress
    {
        private const int ANIMATE_THROTTLE = 10;
        //https://learn.microsoft.com/zh-cn/dotnet/api/system.double.tostring?view=net-7.0
        private const string LABEL_FORMATE = "0";
        double _realtimeProgress;
        private float _mainRectPadding;
        public CircleSlider()
        {
            InitializeComponent();
            this.PropertyChanged += CircleProgressBar_PropertyChanged;
            this.labelView.TextColor = this.ProgressColor;
            this.Orientation=ScrollOrientation.Both;
        }

        private void RefreshMainRectPadding()
        {
            //边界补偿
            this._mainRectPadding = (float)(this.BorderWidth / 2);
            //this._mainRectPadding = (float)(this.BorderWidth);
        }

        private void CircleProgressBar_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsEnabled))
            {
                this.ArcPaint.Color = IsEnabled ? Colors.Gray.ToSKColor() : this.ProgressColor.ToSKColor();
            }

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

        public View LabelContent
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

        public double Maximum
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

        public double Minimum
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

        public Color ProgressColor
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

        public Color ContainerColor
        {
            get { return (Color)GetValue(ContainerColorProperty); }
            set { SetValue(ContainerColorProperty, value); }
        }



        public static readonly BindableProperty ProgressProperty =
  BindableProperty.Create("Progress", typeof(double), typeof(CircleSlider), 0.5, propertyChanged: (bindable, oldValue, newValue) =>
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

        public double Progress
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

        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }




        public static readonly BindableProperty AnimationLengthProperty =
BindableProperty.Create("AnimationLength", typeof(double), typeof(CircleSlider), 1000.0, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;


});

        public double AnimationLength
        {
            get { return (double)GetValue(AnimationLengthProperty); }
            set
            {
                SetValue(AnimationLengthProperty, value);
                OnPropertyChanged();

            }
        }

        private void UpdateProgress()
        {
            this._realtimeProgress = this.Progress;
            this.labelView.Text = this.Progress.ToString(LABEL_FORMATE);
            this.canvasView?.InvalidateSurface();
            
        }

        private void UpdateProgressWithAnimate(Action<double, bool> finished = null)
        {
            this.AbortAnimation("ReshapeAnimations");
            var scaleAnimation = new Animation();


            double progressTarget = this.Progress;

            double progressOrigin = this._realtimeProgress;

            var animateAction = (double r) =>
            {
                this._realtimeProgress = r;
                this.labelView.Text = r.ToString(LABEL_FORMATE);
                this.canvasView?.InvalidateSurface();
            };

            var mySpringOut = (double x) => (x - 1) * (x - 1) * ((5f + 1) * (x - 1) + 5) + 1;
            var scaleUpAnimation0 = new Animation(animateAction, progressOrigin, progressTarget, mySpringOut);
            scaleAnimation.Add(0, 1, scaleUpAnimation0);
            scaleAnimation.Commit(this, "ReshapeAnimations", 16, (uint)this.AnimationLength, finished: finished);

        }
        public double SumValue => Maximum - Minimum;


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
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

            var thumbX = Math.Sin(sweepAngle * Math.PI / 180) * this.Width/2;
            var thumbY = Math.Cos(sweepAngle * Math.PI / 180) * this.Height / 2;

            this.ThumbContent.TranslationX=thumbX;
            this.ThumbContent.TranslationY=-thumbY;

        }




        private SKPaint _outlinePaint;

        public SKPaint OutlinePaint
        {
            get
            {
                if (_outlinePaint == null)
                {
                    RefreshMainRectPadding();
                    SKPaint outlinePaint = new SKPaint
                    {
                        Color = this.ContainerColor.ToSKColor(),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = (float)BorderWidth,
                    };
                    _outlinePaint = outlinePaint;
                }
                return _outlinePaint;
            }
        }

        private SKPaint _arcPaint;

        public SKPaint ArcPaint
        {
            get
            {
                if (_arcPaint == null)
                {
                    RefreshMainRectPadding();
                    SKPaint arcPaint = new SKPaint
                    {
                        Color = this.ProgressColor.ToSKColor(),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = (float)BorderWidth,
                        StrokeCap = SKStrokeCap.Round,
                    };
                    _arcPaint = arcPaint;
                }

                return _arcPaint;
            }
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var thumb = sender as ContentView;
            var PositionX = thumb.TranslationX+e.TotalX;
            var PositionY = thumb.TranslationY+e.TotalY;

            this.test.TranslationX = thumb.TranslationX+e.TotalX;
            this.test.TranslationY = thumb.TranslationY+e.TotalY;

            var sweepAngle = AngleNormalize(Math.Atan2(PositionX, -PositionY)*180/Math.PI);

            var targetProgress = sweepAngle*SumValue/360;
            this.Progress=targetProgress;
            //UpdateProgress();
            Debug.WriteLine(PositionX+","+PositionY+","+sweepAngle);

        }
        private double AngleNormalize(double value)
        {
            double twoPi = 360;
            while (value <= -180) value += twoPi;
            while (value >   180) value -= twoPi;
            value= (value + twoPi) % twoPi;
            return value;
        }

        public ScrollOrientation Orientation { get; set; }

        private void PanGestureRecognizer_PanUpdated2(object sender, PanUpdatedEventArgs e)
        {
            Debug.WriteLine(e.TotalX+","+ e.TotalY);
        }
    }
}