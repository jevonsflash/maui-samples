using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using System.ComponentModel;
using System.Diagnostics;

namespace StickyTab;

public partial class MainPage : ContentPage
{

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();
        Appearing += MainPage_Appearing;
        WeakReferenceMessenger.Default.Register<PanActionArgs, string>(this, TokenHelper.PanAction, PanActionHandler);

    }

    private void MainPage_Appearing(object sender, EventArgs e)
    {
        this.DefaultPanContainer.PitLayout = this.PitContentLayout.Children.Select(c => c as PitGrid).ToList();
        ShowLayout(0);
    }

    private void PanActionHandler(object recipient, PanActionArgs args)
    {
        var child = args.CurrentPit?.Children[0];
        switch (args.PanType)
        {

            case PanType.Out:
                switch (args.CurrentPit?.PitName)
                {
                    case "CancelPit":
                        break;

                    case "SendPit":
                        break;

                    case "TransliterationPit":
                        break;

                    default:
                        break;
                }
                break;
            case PanType.In:
                var parentAnimation = new Animation();

                Color toColor = default;
                double translationX = default;
                double width = default;
                switch (args.CurrentPit?.PitName)
                {
                    case "CancelPit":
                        break;

                    case "SendPit":
                        break;

                    case "TransliterationPit":

                        break;

                    default:
                        break;
                }

                break;
            case PanType.Over:
                switch (args.CurrentPit?.PitName)
                {
                    case "CancelPit":
                        break;

                    case "SendPit":
                        break;

                    case "TransliterationPit":
                        break;

                    default:
                        break;
                }
                ShowLayout(0);

                break;
            case PanType.Start:
                ShowLayout();
                break;
        }




    }

    private void ShowLayout(double opacity = 1)
    {
        this.PitContentLayout.FadeTo(opacity);
    }





    private Color GetColor(double t, Color fromColor, Color toColor)
    {
        return Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                           fromColor.Green + t * (toColor.Green - fromColor.Green),
                           fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                           fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));
    }


    private void DefaultPanContainer_OnOnTapped(object sender, EventArgs e)
    {

    }

    private void DefaultPanContainer_OnOnfinishedChoise(object sender, PitGrid e)
    {
        CurrentPitView = e;
    }

    private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Width))
        {
            this.DefaultPanContainer.PositionX = (this.PitContentLayout.Width - (sender as Grid).Width) / 2;
        }
        else if (e.PropertyName == nameof(Height))
        {
            this.DefaultPanContainer.PositionY = (this.PitContentLayout.Height - (sender as Grid).Height) / 2;

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        //await App.Current.MainPage.Navigation.PushAsync(new ColorAnimation());
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetX = e.NewValue;
        OffsetXPositiveLabel.Text = String.Format("Current OffsetX is {0}", e.NewValue.ToString("f2"));

    }

    private void OnSliderValueChanged2(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetX = -e.NewValue;
        OffsetXNegativeLabel.Text = String.Format("Current OffsetX is -{0}", e.NewValue.ToString("f2"));

    }

    private void OnSliderValueChanged3(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetY = e.NewValue;
        OffsetYPositiveLabel.Text = String.Format("Current OffsetY is {0}", e.NewValue.ToString("f2"));

    }

    private void OnSliderValueChanged4(object sender, ValueChangedEventArgs e)
    {
        this.MainStickyPan.OffsetY =-e.NewValue;
        OffsetYPositiveLabel.Text = String.Format("Current OffsetY is -{0}", e.NewValue.ToString("f2"));

    }
}

