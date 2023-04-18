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
    private PitGrid _currentPit;

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();
        this.NewTabLabel.Text = FaIcons.IconPlus;
        this.RefreshLabel.Text = FaIcons.IconRefresh;
        this.CloseTabLabel.Text = FaIcons.IconTimes;
        Appearing += MainPage_Appearing;
        WeakReferenceMessenger.Default.Register<PanActionArgs, string>(this, TokenHelper.PanAction, PanActionHandler);

    }

    private void MainPage_Appearing(object sender, EventArgs e)
    {
        this.DefaultPanContainer.PitLayout = this.PitContentLayout.Children.Select(c => c as PitGrid).ToList();
        ShowLayout(1);
    }

    private void PanActionHandler(object recipient, PanActionArgs args)
    {
        var child = args.CurrentPit?.Children[0];

        _currentPit = args.CurrentPit;


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
                ShowLayout(1);

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





    private void DefaultPanContainer_OnOnTapped(object sender, EventArgs e)
    {

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
        else if (e.PropertyName == nameof(TranslationX))
        {
            var centerX = 0.0;
            if (_currentPit != null)
            {
                centerX = _currentPit.X + _currentPit.Width / 2;
            }
            this.MainStickyPan.OffsetX = this.DefaultPanContainer.Content.TranslationX + this.DefaultPanContainer.Content.Width / 2 - centerX;

        }
        else if (e.PropertyName == nameof(TranslationY))
        {
            var centerY = 0.0;
            if (_currentPit != null)
            {
                centerY = _currentPit.Y + _currentPit.Height / 2;
            }
            this.MainStickyPan.OffsetY = this.DefaultPanContainer.Content.TranslationY + this.DefaultPanContainer.Content.Height / 2 - centerY;
        }

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new StickPanSamplePage());
    }

    private void DefaultPanContainer_OnOnfinishedChoise(object sender, PitGrid e)
    {

    }



}

