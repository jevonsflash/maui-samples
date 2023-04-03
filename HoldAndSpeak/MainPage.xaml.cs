using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using System.ComponentModel;
using System.Diagnostics;

namespace HoldAndSpeak;

public partial class MainPage : ContentPage
{

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();
        this.SendLabel.Text = FaIcons.IconRss;
        this.CancelLabel.Text = FaIcons.IconTimes;
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
                        DeActive(this.CancelBox, this.CancelLabel);
                        break;

                    case "SendPit":
                        DeActive(this.SendBox, this.SendLabel);
                        break;

                    case "TransliterationPit":
                        DeActive(this.TransliterationBox, this.TransliterationLabel);
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
                        Active(this.CancelBox, this.CancelLabel, Colors.White, Colors.Black);

                        this.TalkBox.AbortAnimation("TalkBoxAnimations");

                        toColor = (Color)Application.Current.Resources["PhoneChromeBrush"];
                        translationX = -175;
                        width = 150;
                        break;

                    case "SendPit":
                        Active(this.SendBox, this.SendLabel, Colors.Gray, Colors.Black, 1.0);
                        toColor = (Color)Application.Current.Resources["PhoneAccentBrush"];
                        translationX = 0;
                        width = 200;

                        break;

                    case "TransliterationPit":
                        Active(this.TransliterationBox, this.TransliterationLabel, Colors.White, Colors.Black);
                        toColor = (Color)Application.Current.Resources["PhoneAccentBrush"];
                        translationX = 100;
                        width = 300;

                        break;

                    default:
                        break;
                }

                var fromColor = this.TalkBox.Color;

                var animation2 = new Animation(t => this.TalkBox.Color = GetColor(t, fromColor, toColor), 0, 1, Easing.SpringOut);
                var animation4 = new Animation(v => this.TalkBoxLayout.TranslationX = v, this.TalkBoxLayout.TranslationX, translationX);
                var animation5 = new Animation(v => this.TalkBox.WidthRequest = v, this.TalkBox.Width, width);


                parentAnimation.Add(0, 1, animation2);
                parentAnimation.Add(0, 1, animation4);
                parentAnimation.Add(0, 1, animation5);

                parentAnimation.Commit(this, "TalkBoxAnimations", 16, 300);

                break;
            case PanType.Over:
                switch (args.CurrentPit?.PitName)
                {
                    case "CancelPit":
                        DeActive(this.CancelBox, this.CancelLabel);
                        break;

                    case "SendPit":
                        DeActive(this.SendBox, this.SendLabel);
                        break;

                    case "TransliterationPit":
                        DeActive(this.TransliterationBox, this.TransliterationLabel);
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
        this.TalkBoxLayout.FadeTo(opacity);
    }

    private void Active(BoxView currentContent, Label text, Color toColor, Color txtToColor, double scaleTo = 1.5)
    {
        currentContent.AbortAnimation("ActivateFunctionAnimations");
        var parentAnimation = new Animation();


        var txtFromColor = text.TextColor;

        var animation2 = new Animation(t => text.TextColor = GetColor(t, txtFromColor, txtToColor), 0, 1, Easing.SpringOut);



        var fromColor = currentContent.Color;

        var animation4 = new Animation(t => currentContent.Color = GetColor(t, fromColor, toColor), 0, 1, Easing.SpringOut);
        var animation5 = new Animation(v => currentContent.Scale = v, currentContent.Scale, scaleTo);


        parentAnimation.Add(0, 1, animation2);
        parentAnimation.Add(0, 1, animation4);
        parentAnimation.Add(0, 1, animation5);

        parentAnimation.Commit(this, "ActivateFunctionAnimations", 16, 300);
    }



    private void DeActive(BoxView currentContent, Label text)
    {
        currentContent.AbortAnimation("DeactivateFunctionAnimations");
        var parentAnimation = new Animation();


        var txtFromColor = text.TextColor;
        var txtToColor = (Color)Application.Current.Resources["PhoneContrastForegroundBrush"];

        var animation2 = new Animation(t => text.TextColor = GetColor(t, txtFromColor, txtToColor), 0, 1, Easing.SpringOut);



        var fromColor = currentContent.Color;
        var toColor = (Color)Application.Current.Resources["PhoneContrastBackgroundBrush"];

        var animation4 = new Animation(t => currentContent.Color = GetColor(t, fromColor, toColor), 0, 1, Easing.SpringOut);
        var animation5 = new Animation(v => currentContent.Scale = v, currentContent.Scale, 1.0);


        parentAnimation.Add(0, 1, animation2);
        parentAnimation.Add(0, 1, animation4);
        parentAnimation.Add(0, 1, animation5);

        parentAnimation.Commit(this, "DeactivateFunctionAnimations", 16, 300);
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
}

