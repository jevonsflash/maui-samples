using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace StickyTab;

public partial class MainPage : ContentPage
{
    private PitGrid _currentDefaultPit;
    private PitGrid _currentNewsPit;
    private Label tipLabel;

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
        this.NewsPanContainer.PitLayout = this.NewsPitContentLayout.Children.Select(c => c as PitGrid).ToList();
        ShowLayout(1);
    }

    private void PanActionHandler(object recipient, PanActionArgs args)
    {
        if (args.Sender==this.DefaultPanContainer)
        {
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
                    tipLabel = args.CurrentPit?.Children.LastOrDefault() as Label;
                    if (tipLabel!=null)
                    {
                        tipLabel.FadeTo(0);
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
                    tipLabel = args.CurrentPit?.Children.LastOrDefault() as Label;
                    if (tipLabel!=null)
                    {
                        tipLabel.FadeTo(1);
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
                    tipLabel.FadeTo(0);
                    ShowLayout(0);
                    break;
                case PanType.Start:
                    ShowLayout();
                    break;
            }
            Debug.WriteLine(args.CurrentPit?.PitName+"Triggled:"+args.PanType);
            _currentDefaultPit = args.CurrentPit;
        }
        else if (args.Sender==this.NewsPanContainer)
        {
            switch (args.PanType)
            {


                case PanType.In:
                    switch (args.CurrentPit?.PitName)
                    {
                        case "News1Pit":
                            this.NewsTipLabel.Text="头条";
                            break;

                        case "News2Pit":
                            this.NewsTipLabel.Text="体育";
                            break;

                        case "News3Pit":

                            this.NewsTipLabel.Text="财经";
                            break;
                        case "News4Pit":

                            this.NewsTipLabel.Text="科技";
                            break;

                        default:
                            break;
                    }

                    break;
                default: break;
            }

            Debug.WriteLine(args.CurrentPit?.PitName+"Triggled:"+args.PanType);
            _currentNewsPit = args.CurrentPit;
        }



    }

    private void ShowLayout(double opacity = 1)
    {
        var length = opacity==1 ? 250 : 0;
        this.DefaultPanContainer.FadeTo(opacity, (uint)length);
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
            if (_currentDefaultPit != null)
            {
                centerX = _currentDefaultPit.X + _currentDefaultPit.Width / 2;
            }
            this.MainStickyPan.OffsetX = this.DefaultPanContainer.Content.TranslationX + this.DefaultPanContainer.Content.Width / 2 - centerX;

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new StickPanSamplePage());
    }

    private void BindableObject_OnPropertyChanged2(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Height))
        {
            this.NewsPanContainer.PositionY = (this.NewsPitContentLayout.Height - (sender as Grid).Height) / 2;

        }
        else if (e.PropertyName == nameof(Width))
        {
            this.NewsPanContainer.PositionX = (this.NewsPitContentLayout.Width - (sender as Grid).Width) / 2;
        }
        else if (e.PropertyName == nameof(TranslationY))
        {
            var centerY = 0.0;
            if (_currentNewsPit != null)
            {
                centerY = _currentNewsPit.Y + _currentNewsPit.Height / 2;
            }
            this.NewsStickyPan.OffsetY = this.NewsPanContainer.Content.TranslationY + this.NewsPanContainer.Content.Height / 2 - centerY;
        }
    }
}

