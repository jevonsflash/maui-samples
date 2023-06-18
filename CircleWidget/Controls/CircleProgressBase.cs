using SkiaSharp.Views.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleWidget.Controls
{
    public abstract class CircleProgressBase : ContentView, IProgress
    {

        public event EventHandler<double> ValueChanged;

        protected const int ANIMATE_THROTTLE = 10;
        //https://learn.microsoft.com/zh-cn/dotnet/api/system.double.tostring?view=net-7.0
        protected const string LABEL_FORMATE = "0";
        protected double _realtimeProgress;
        protected float _mainRectPadding;
        public CircleProgressBase()
        {
            this.PropertyChanged += CircleProgressBar_PropertyChanged;

        }

        protected virtual void RefreshMainRectPadding()
        {
            //边界补偿
            this._mainRectPadding = (float)(this.BorderWidth / 2);
            this.Padding = this._mainRectPadding;
            //this._mainRectPadding = (float)(this.BorderWidth);
        }

        protected virtual void CircleProgressBar_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsEnabled))
            {
                this.ArcPaint.Color = IsEnabled ? Colors.Gray.ToSKColor() : this.ProgressColor.ToSKColor();
            }
            else if (e.PropertyName == nameof(Maximum) ||e.PropertyName == nameof(Minimum))
            {
                this.OnPropertyChanged(nameof(SumValue));
            }
            else if (e.PropertyName == nameof(BorderWidth))
            {
                this.RefreshMainRectPadding();
            }
        }


        public double SumValue => Maximum - Minimum;


        public abstract double Maximum { get; set; }
        public abstract double Minimum { get; set; }
        public abstract Color ContainerColor { get; set; }
        public abstract Color ProgressColor { get; set; }

        public abstract double Progress { get; set; }
        public abstract double AnimationLength { get; set; }
        public abstract double BorderWidth { get; set; }
        public abstract View LabelContent { get; set; }

        protected virtual void UpdateProgress()
        {
            this._realtimeProgress = this.Progress;
            ValueChanged?.Invoke(this, this._realtimeProgress);
        }

        protected virtual void UpdateProgressWithAnimate(Action<double, bool> finished = null)
        {
            this.AbortAnimation("ReshapeAnimations");
            var scaleAnimation = new Animation();


            double progressTarget = this.Progress;

            double progressOrigin = this._realtimeProgress;

            var animateAction = (double r) =>
            {
                this._realtimeProgress = r;
                ValueChanged?.Invoke(this, this._realtimeProgress);
            };

            var myEasing = (double x) => {
                if (x < 1 / 2.75f)
                {
                    return 7.5625f * x * x;
                }
                if (x < 2 / 2.75f)
                {
                    x -= 1.5f / 2.75f;
                    return 7.5625f * x * x + .75f;
                }
                if (x < 2.5f / 2.75f)
                {
                    x -= 2.25f / 2.75f;
                    return 7.5625f * x * x + .9375f;
                }
                x -= 2.625f / 2.75f;
                return 7.5625f * x * x + .984375f;
            };
            var scaleUpAnimation0 = new Animation(animateAction, progressOrigin, progressTarget, myEasing);
            scaleAnimation.Add(0, 1, scaleUpAnimation0);
            scaleAnimation.Commit(this, "ReshapeAnimations", 16, (uint)this.AnimationLength, finished: finished);

        }



        protected virtual void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
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
        }




        protected SKPaint _outlinePaint;

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

        protected SKPaint _arcPaint;

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
    }
}
