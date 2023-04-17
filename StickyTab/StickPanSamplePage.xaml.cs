namespace StickyTab;

public partial class StickPanSamplePage : ContentPage
{
	public StickPanSamplePage()
	{
		InitializeComponent();
	}

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetX = e.NewValue;
        this.MainStickyPan.TranslationX = e.NewValue;
        OffsetXPositiveLabel.Text = String.Format("Current OffsetX is {0}", e.NewValue.ToString("f2"));

    }

    private void OnSliderValueChanged2(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetX = -e.NewValue;
        this.MainStickyPan.TranslationX = -e.NewValue;
        OffsetXNegativeLabel.Text = String.Format("Current OffsetX is -{0}", e.NewValue.ToString("f2"));

    }

    private void OnSliderValueChanged3(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetY = e.NewValue;
        this.MainStickyPan.TranslationY = e.NewValue;
        OffsetYPositiveLabel.Text = String.Format("Current OffsetY is {0}", e.NewValue.ToString("f2"));

    }

    private void OnSliderValueChanged4(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetY = -e.NewValue;
        this.MainStickyPan.TranslationY = -e.NewValue;
        OffsetYPositiveLabel.Text = String.Format("Current OffsetY is -{0}", e.NewValue.ToString("f2"));

    }
}