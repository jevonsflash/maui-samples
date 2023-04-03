namespace HoldAndSpeak;

public partial class ColorAnimation : ContentPage
{
    public ColorAnimation()
    {
        InitializeComponent();
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {

        var toColor = (Color)Application.Current.Resources["PhoneAccentBrush"];
        var fromColor = (Color)Application.Current.Resources["PhoneChromeBrush"];

        double value = e.NewValue;
        var t = value / 100;
        this.TalkBox.Color = GetColor(t, fromColor, toColor);
        displayLabel.Text = String.Format("Current t is {0}, color is {1}", t.ToString("f2"), this.TalkBox.Color.ToArgbHex());
    }

    private Color GetColor(double t, Color fromColor, Color toColor)
    {
        return Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                           fromColor.Green + t * (toColor.Green - fromColor.Green),
                           fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                           fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));
    }

}