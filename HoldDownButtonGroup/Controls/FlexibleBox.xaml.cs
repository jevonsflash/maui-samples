using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StickyTab.Controls;

public partial class FlexibleBox : ContentView
{
    public FlexibleBox()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty FillColorProperty =
BindableProperty.Create("FillColor", typeof(Brush), typeof(FlexibleBox), default(Brush), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (FlexibleBox)bindable;
    obj.MainBox.Fill = (Brush)newValue;

});

    public Brush FillColor
    {
        get { return (Brush)GetValue(FillColorProperty); }
        set
        {
            SetValue(FillColorProperty, value);
            OnPropertyChanged();

        }
    }

    public static readonly BindableProperty BorderColorProperty =
BindableProperty.Create("BorderColor", typeof(Brush), typeof(FlexibleBox), default(Brush), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (FlexibleBox)bindable;
    obj.MainBox.Stroke = (Brush)newValue;

});

    public Brush BorderColor
    {
        get { return (Brush)GetValue(BorderColorProperty); }
        set
        {
            SetValue(BorderColorProperty, value);
            OnPropertyChanged();

        }
    }

    public Animation BoxAnimation => GetAnimation();

    public Animation GetAnimation()
    {

        var scaleAnimation = new Animation();

        var scaleUpAnimation0 = new Animation(v => MainBox.Scale = v, 0, 1);
        var scaleUpAnimation1 = new Animation(v => MainBox.Scale = v, 1, 0.1);


        scaleAnimation.Add(0, 0.2, scaleUpAnimation0);
        scaleAnimation.Add(0.8, 1, scaleUpAnimation1);

        scaleAnimation.Finished = () =>
        {
            this.MainBox.Scale = 1;
        };

        return scaleAnimation;

    }


}