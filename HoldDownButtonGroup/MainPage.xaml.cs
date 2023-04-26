using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using StickyTab.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace StickyTab;

public partial class MainPage : ContentPage
{
    private const int bubbleSize = 20;
    private const int bubbleCnt = 30;
    private static readonly Random rnd = new Random();

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();

    }



    private Animation InitAnimation(FlexibleBox element, Size targetSize, bool isOnTop = true)
    {
        var offsetAnimation = new Animation();

        if (targetSize == default)
        {
            targetSize = element.DesiredSize;

        }
        var easing = Easing.Linear;

        var originX = PitContentLayout.Width / 2;
        var originY = PitContentLayout.Height / 2;

        var targetX = rnd.Next(-(int)targetSize.Width, (int)targetSize.Width) + (int)targetSize.Width / 2 + originX;
        var targetY = isOnTop ? rnd.Next(-(int)(targetSize.Height * 1.5), 0) + (int)targetSize.Height / 2 + originY :
             rnd.Next(0, (int)(targetSize.Height * 1.5)) + (int)targetSize.Height / 2 + originY
            ;


        var offsetX = targetX - originX;
        var offsetY = targetY - originY;


        var offsetAnimation1 = new Animation(v => element.TranslationX = v, originX - targetSize.Width / 2, targetX - targetSize.Width / 2, easing);
        var offsetAnimation2 = new Animation(v => element.TranslationY = v, originY - targetSize.Height / 2, targetY - targetSize.Height / 2, easing);

        offsetAnimation.Add(0.2, 0.8, offsetAnimation1);
        offsetAnimation.Add(0.2, 0.8, offsetAnimation2);
        offsetAnimation.Add(0, 1, element.BoxAnimation);

        offsetAnimation.Finished = () =>
        {
            foreach (var item in this.PitContentLayout.Children)
            {
                if (item is FlexibleBox)
                {
                    Init(item as FlexibleBox);
                }
            }
        };

        return offsetAnimation;
    }




    private void DefaultPanContainer_OnOnTapped(object sender, EventArgs e)
    {

    }



    private async void Button_Clicked(object sender, EventArgs e)
    {
        //await App.Current.MainPage.Navigation.PushAsync(new ColorAnimation());
    }

    private void DefaultPanContainer_OnOnfinishedChoise(object sender, PitGrid e)
    {

    }

    private void ToggleAnimation_Clicked(object sender, EventArgs e)
    {
        this.StartAnimation();
    }

    private void ContentPage_SizeChanged(object sender, EventArgs e)
    {

        foreach (var item in this.PitContentLayout.Children)
        {
            if (item is FlexibleBox)
            {
                Init(item as FlexibleBox);
            }
        }


    }

    private void StartAnimation()
    {
        Content.AbortAnimation("ReshapeAnimations");
        var offsetAnimationGroup = new Animation();

        foreach (var item in this.PitContentLayout.Children)
        {
            if (item is FlexibleBox)
            {
                var isOntop = this.PitContentLayout.Children.IndexOf(item) > this.PitContentLayout.Children.Count / 2;
                var currentAnimation = InitAnimation(item as FlexibleBox, new Size(30, 30), isOntop);
                offsetAnimationGroup.Add(0, 1, currentAnimation);


            }
        }
        offsetAnimationGroup.Commit(this, "ReshapeAnimations", 16, 400);

    }

    private void Init(VisualElement element)
    {
        var TargetSize = element.DesiredSize;
        var originX = (PitContentLayout.Width - TargetSize.Width) / 2;
        var originY = (PitContentLayout.Height - TargetSize.Height) / 2;

        element.TranslationX = originX;
        element.TranslationY = originY;
        element.Rotation = 0;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        for (int i = 0; i < bubbleCnt; i++)
        {
            var currentBox = new FlexibleBox();
            currentBox.FillColor = i % 2 == 0 ? SolidColorBrush.Red : SolidColorBrush.Transparent;
            currentBox.BorderColor = SolidColorBrush.Red;
            currentBox.HeightRequest = bubbleSize;
            currentBox.WidthRequest = bubbleSize;
            currentBox.HorizontalOptions = LayoutOptions.Start;
            currentBox.VerticalOptions = LayoutOptions.Start;
            this.PitContentLayout.Add(currentBox);
        }
    }
}

