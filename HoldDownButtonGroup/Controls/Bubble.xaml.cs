using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HoldDownButtonGroup.Controls;

public partial class Bubble : ContentView
{
    public Bubble()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty FillColorProperty =
BindableProperty.Create("FillColor", typeof(Brush), typeof(Bubble), default(Brush), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (Bubble)bindable;
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
BindableProperty.Create("BorderColor", typeof(Brush), typeof(Bubble), default(Brush), propertyChanged: (bindable, oldValue, newValue) =>
{
    var obj = (Bubble)bindable;
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
        var scaleUpAnimation1 = new Animation(v => MainBox.Scale = v, 1, 0);


        scaleAnimation.Add(0, 0.2, scaleUpAnimation0);
        scaleAnimation.Add(0.8, 1, scaleUpAnimation1);

        scaleAnimation.Finished = () =>
        {
            this.MainBox.Scale = 0;
        };

        return scaleAnimation;

    }


}