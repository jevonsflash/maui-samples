using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace StickyTab;

public partial class FlexibleBoxSamplePage : ContentPage
{
    private PitGrid _currentPit;
    private static readonly Random rnd = new Random();

    public PitGrid CurrentPitView { get; set; }

    public FlexibleBoxSamplePage()
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

        var originX = PitContentLayout.Width / 2;
        var originY = PitContentLayout.Height / 2;

        var targetX = rnd.Next(-(int)TargetSize.Width, (int)TargetSize.Width) + (int)TargetSize.Width / 2 + originX;
        var targetY = rnd.Next(-(int)(TargetSize.Height * 1.5), 0) + (int)TargetSize.Height / 2 + originY;

        var offsetX = targetX - originX;
        var offsetY = targetY - originY;



        var angle = Math.Atan2(offsetX, offsetY);

        var line = new Line(originX, originY, targetX, targetY);
        line.Fill = LinearGradientBrush.Black;
        line.Stroke = LinearGradientBrush.Red;
        line.StrokeThickness = 2;
        this.PitContentLayout.Add(line);
        var rotation = -angle * 180 / Math.PI;
        TestCircle.Rotation = rotation;
        var k = 2 * Math.Sqrt(Math.Pow(targetX - originX, 2) + Math.Pow(targetY - originY, 2));

        var originHeight = TestCircle.Height;
        var widthAnimation1 = new Animation(v => TestCircle.HeightRequest = v, originHeight, originHeight * 2, easing);
        var widthAnimation2 = new Animation(v => TestCircle.HeightRequest = v, originHeight * 2, originHeight, easing);

        //补偿效果
        var offsetAnimation3 = new Animation(v => TestCircle.TranslationY = v, TestCircle.TranslationY, TestCircle.TranslationY - originHeight / 2, easing);
        var offsetAnimation4 = new Animation(v => TestCircle.TranslationY = v, TestCircle.TranslationY - originHeight / 2, TestCircle.TranslationY, easing);

        scaleAnimation.Add(0.2, 0.5, offsetAnimation3);
        scaleAnimation.Add(0.5, 0.8, offsetAnimation4);



        scaleAnimation.Add(0.2, 0.5, widthAnimation1);
        scaleAnimation.Add(0.5, 0.8, widthAnimation2);

        Debug.WriteLine($"originX:{originX},originY:{originY}");
        Debug.WriteLine($"targetX:{targetX},targetY:{targetY}");
        Debug.WriteLine($"offsetX:{offsetX},offsetY:{offsetY}");

        Debug.WriteLine($"angle:{angle},rotation:{rotation}");

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

