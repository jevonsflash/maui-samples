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
    public partial class CircleProgressBar : CircleProgressBase
    {
        public CircleProgressBar()
        {
            InitializeComponent();
            this.labelView.TextColor = this.ProgressColor;
            this.ValueChanged+=CircleProgressBar_ValueChanged;

        }

        public static readonly BindableProperty MaximumProperty =
    BindableProperty.Create("Maximum", typeof(double), typeof(CircleProgressBar), 1.0, propertyChanged: (bindable, oldValue, newValue) =>
    {
        var obj = (CircleProgressBar)bindable;
        obj.canvasView?.InvalidateSurface();
    });

        public override double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly BindableProperty MinimumProperty =
BindableProperty.Create("Minimum", typeof(double), typeof(CircleProgressBar), 0.0, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleProgressBar)bindable;
    obj.canvasView?.InvalidateSurface();

});

        public override double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
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




        public static readonly BindableProperty ProgressColorProperty =
BindableProperty.Create("ProgressColor", typeof(Color), typeof(CircleProgressBar), Colors.Red, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleProgressBar)bindable;
    obj.ArcPaint.Color = obj.ProgressColor.ToSKColor();
    obj.labelView.TextColor = obj.ProgressColor;
});

        public override Color ProgressColor
        {
            get { return (Color)GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }



        public static readonly BindableProperty ProgressProperty =
  BindableProperty.Create("Progress", typeof(double), typeof(CircleProgressBar), 0.5, propertyChanged: (bindable, oldValue, newValue) =>
  {
      var obj = (CircleProgressBar)bindable;
      var valueChangedSpan = (double)oldValue - (double)newValue;
      if (Math.Abs(valueChangedSpan) > ANIMATE_THROTTLE)
      {
          obj.UpdateProgressWithAnimate();
          //Debug.WriteLine("valueChangedSpan is " + valueChangedSpan + " ,triggered animate");
      }
      else
      {
          obj.UpdateProgress();
          //Debug.WriteLine("valueChangedSpan is " + valueChangedSpan);
      }
  });

        public override double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }


        public static readonly BindableProperty LabelContentProperty =
        BindableProperty.Create("LabelContent", typeof(View), typeof(CircleProgressBar), default, propertyChanged: (bindable, oldValue, newValue) =>
        {
            var obj = (CircleProgressBar)bindable;
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




        public static readonly BindableProperty BorderWidthProperty =
BindableProperty.Create("BorderWidth", typeof(double), typeof(CircleProgressBar), 20.0, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleProgressBar)bindable;
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
BindableProperty.Create("AnimationLength", typeof(double), typeof(CircleProgressBar), 1000.0, propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (CircleProgressBar)bindable;


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


        private void CircleProgressBar_ValueChanged(object sender, double e)
        {
            this.labelView.Text = e.ToString(LABEL_FORMATE);
            this.canvasView?.InvalidateSurface();
        }
    }
}