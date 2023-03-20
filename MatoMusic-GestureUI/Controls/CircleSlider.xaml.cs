using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace MatoMusic.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleSlider : ContentView
    {

        private double oldValue;
        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public CircleSlider()
        {
            InitializeComponent();
            this.BorderWidth = 15;
            RefreshMainRectPadding();
            this.PropertyChanged += CircleSlider_PropertyChanged;
        }

        private void RefreshMainRectPadding()
        {
            this._mainRectPadding = 15 - this.BorderWidth / 2;
        }

        private void CircleSlider_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsEnabled))
            {
                //处理是否激活
            }

        }

        public static readonly BindableProperty MaximumProperty =
    BindableProperty.Create("Maximum", typeof(double), typeof(CircleSlider), default(double), propertyChanged: (bindable, oldValue, newValue) =>
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
  BindableProperty.Create("Minimum", typeof(double), typeof(CircleSlider), default(double), propertyChanged: (bindable, oldValue, newValue) =>
  {
      var obj = (CircleSlider)bindable;
      obj.canvasView?.InvalidateSurface();

  });

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }


        public static readonly BindableProperty TintColorProperty =
BindableProperty.Create("TintColor", typeof(Color), typeof(CircleSlider), Colors.White, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleSlider)bindable;
    obj.ArcPaint.Color = obj.TintColor.ToSKColor();

});

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
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



        public static readonly BindableProperty ValueProperty =
  BindableProperty.Create("Value", typeof(double), typeof(CircleSlider), default(double), propertyChanged: (bindable, oldValue, newValue) =>
  {
      var obj = (CircleSlider)bindable;
      obj.canvasView?.InvalidateSurface();
      obj.ValueChanged?.Invoke(obj, new ValueChangedEventArgs(obj.oldValue, obj.Value));
      obj.oldValue = obj.Value;

  });

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        private float _borderWidth;

        public float BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                var old_borderWidth = _borderWidth;

                var span = value - old_borderWidth;

                SetStrokeWidth(span, old_borderWidth);

                _borderWidth = value;

                this.ArcPaint.StrokeWidth = _borderWidth;
                this.OutlinePaint.StrokeWidth = _borderWidth;
            }
        }

        private async void SetStrokeWidth(float span, float old_borderWidth)
        {
            if (span > 0)
            {
                for (int i = 0; i <= span; i++)
                {
                    await Task.Delay(10);
                    this.ArcPaint.StrokeWidth = old_borderWidth + i;
                    this.OutlinePaint.StrokeWidth = old_borderWidth + i;
                    RefreshMainRectPadding();
                }
            }
            else
            {
                for (int i = 0; i >= span; i--)
                {
                    await Task.Delay(10);
                    this.ArcPaint.StrokeWidth = old_borderWidth + i;
                    this.OutlinePaint.StrokeWidth = old_borderWidth + i;
                    RefreshMainRectPadding();

                }
            }

        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {

            var SumValue = Maximum - Minimum;


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKRect rect = new SKRect(_mainRectPadding, _mainRectPadding, info.Width - _mainRectPadding, info.Height - _mainRectPadding);
            float startAngle = -90;
            float sweepAngle = (float)((Value / SumValue) * 360);

            canvas.DrawOval(rect, OutlinePaint);

            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path, ArcPaint);
            }
        }

        private float _mainRectPadding;



        private SKPaint _outlinePaint;

        public SKPaint OutlinePaint
        {
            get
            {
                if (_outlinePaint == null)
                {
                    SKPaint outlinePaint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = BorderWidth,
                    };
                    _outlinePaint = outlinePaint;
                }
                return
                    _outlinePaint;
            }
            set { _outlinePaint = value; }
        }

        private SKPaint _arcPaint;

        public SKPaint ArcPaint
        {
            get
            {
                if (_arcPaint == null)
                {
                    SKPaint arcPaint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = BorderWidth,
                    };
                    _arcPaint = arcPaint;
                }

                return _arcPaint;
            }
            set { _arcPaint = value; }
        }

    }
}