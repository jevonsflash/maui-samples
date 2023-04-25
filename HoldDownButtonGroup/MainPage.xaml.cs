using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace StickyTab;

public partial class MainPage : ContentPage
{
    private PitGrid _currentPit;
    private static readonly Random rnd = new Random();

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();

    }



    private void StartAnimation()
    {
        Init();
        Content.AbortAnimation("ReshapeAnimations");

        var scaleAnimation = new Animation();

        var scaleUpAnimation0 = new Animation(v => TestCircle.Scale = v, 0, 1);
        var scaleUpAnimation1 = new Animation(v => TestCircle.Scale = v, 1, 0.6);


        //scaleAnimation.Add(0, 0.2, scaleUpAnimation0);
        //scaleAnimation.Add(0.8, 1, scaleUpAnimation1);


        var TargetSize = TestCircle.DesiredSize;
        var easing = Easing.Linear;

        var originX = (PitContentLayout.Width - TargetSize.Width) / 2;
        var originY = (PitContentLayout.Height - TargetSize.Height) / 2;

        var targetX = rnd.Next(-(int)TargetSize.Width, (int)TargetSize.Width) + (int)TargetSize.Width / 2+originX;
        var targetY = rnd.Next(-(int)(TargetSize.Height * 1.5), 0) + (int)TargetSize.Height / 2+originY;

        var offsetAnimation1 = new Animation(v => TestCircle.TranslationX = v, originX, targetX, easing);
        var offsetAnimation2 = new Animation(v => TestCircle.TranslationY = v, originY, targetY, easing);

        //scaleAnimation.Add(0.2, 0.8, offsetAnimation1);
        //scaleAnimation.Add(0.2, 0.8, offsetAnimation2);
        
       var angle= Math.Atan2(targetY - originY, targetX - originX);

       
        TestCircle.Rotation =  -angle*180/Math.PI;
        var k = 2*Math.Sqrt(Math.Pow(targetX-originX, 2)+ Math.Pow(targetY-originY, 2));

        var originWidth = TestCircle.Width;
        var widthAnimation1 = new Animation(v => TestCircle.WidthRequest = v, originWidth, originWidth*2, easing);
        var widthAnimation2 = new Animation(v => TestCircle.WidthRequest = v, originWidth*2, originWidth, easing);

        scaleAnimation.Add(0.2, 0.5, widthAnimation1);
        scaleAnimation.Add(0.5, 0.8, widthAnimation2);



        scaleAnimation.Commit(this, "ReshapeAnimations", 16, 10000);


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
        Init();
    }

    private void Init()
    {
        var TargetSize = TestCircle.DesiredSize;
        var originX = (PitContentLayout.Width - TargetSize.Width) / 2;
        var originY = (PitContentLayout.Height - TargetSize.Height) / 2;

        TestCircle.TranslationX = originX;
        TestCircle.TranslationY = originY;
        TestCircle.Rotation = 0;
    }
}

