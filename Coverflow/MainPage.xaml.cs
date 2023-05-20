using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Coverflow.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using Microsoft.Maui.Controls;
using System;

namespace Coverflow;

public partial class MainPage : ContentPage
{
    private int currentPos;

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();
        this.Loaded+=MainPage_Loaded;
        this.BindingContext=new MainPageViewModel();
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
        RenderTransform(currentPos);
    }





    private async void ToggleAnimation_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new BezierModulatedLayoutPage());

    }

    private void RenderTransform(int currentPos)
    {


        var info = this.BoxLayout;
        double xCenter = info.Width / 2;
        info.TranslateTo(0, 100, 0);
        uint duration = 400;
        var step = xCenter*0.12;
        var sb = new StringBuilder();
        var currentSlidePadding = this.BoxLayout.Width * 0.15;
        var rotateY = 65;
        var skewY = 25;
        var transY = 150;
        foreach (var bitmapLayout in this.BoxLayout.Children)
        {
            (bitmapLayout as VisualElement).AbortAnimation("AlbumArtImageAnimation");

            var pos = this.BoxLayout.Children.IndexOf(bitmapLayout);
            var bitmapObj = (bitmapLayout as Grid).Children.FirstOrDefault();
            var labelObj = (bitmapLayout as Grid).Children.LastOrDefault();
            double xBitmap;
            int zIndex;
            double targetScale;
            double targetRotateY;
            double targetSkewY;
            double targetTransY;
            if (pos < currentPos)
            {
                zIndex=pos;
                xBitmap = (double)(-(currentPos * step) + (pos * step)  - currentSlidePadding);
                targetRotateY=rotateY;
                targetSkewY=skewY;
                targetTransY=-transY;
                targetScale=0.8;
                (labelObj as Label).IsVisible=false;

            }
            else if (pos > currentPos)
            {
                zIndex=this.BoxLayout.Children.Count-pos;
                xBitmap = (double)(((pos - currentPos) * step)  + currentSlidePadding);
                targetRotateY=-rotateY;
                targetSkewY=-skewY;
                targetTransY=transY;
                targetScale=0.8;
                (labelObj as Label).IsVisible=false;

            }
            else
            {
                xBitmap =  0;
                zIndex=this.BoxLayout.Children.Count;
                targetRotateY=0;
                targetSkewY=0;
                targetTransY=0;
                targetScale=1;
                (labelObj as Label).IsVisible=true;

            }


            sb.AppendLine(pos.ToString() + ": " + xBitmap);

            (bitmapLayout as VisualElement).ZIndex = zIndex;


            //Use animation
            //(bitmapObj as RotationImage).RotateY=targetRotateY;
            //(bitmapObj as RotationImage).SkewY=targetSkewY;
            //(bitmapObj as RotationImage).TransY=targetTransY;

            Animation albumAnimation = new Animation();


            var originTranslationX = (bitmapLayout as VisualElement).TranslationX;
            var originScale = (bitmapLayout as VisualElement).Scale;
            var animation1 = new Animation(v => (bitmapLayout as VisualElement).TranslationX = v, originTranslationX, xBitmap, Easing.CubicInOut);
            var animation2 = new Animation(v => (bitmapLayout as VisualElement).Scale = v, originScale, targetScale, Easing.CubicInOut);


            if (targetSkewY!=(bitmapObj as RotationImage).SkewY)
            {
                var animation4 = new Animation(v => (bitmapObj as RotationImage).SkewY = v, (bitmapObj as RotationImage).SkewY, targetSkewY, Easing.CubicInOut);
                albumAnimation.Add(0, 1, animation4);

            }

            if (targetRotateY!=(bitmapObj as RotationImage).RotateY)
            {
                var animation3 = new Animation(v => (bitmapObj as RotationImage).RotateY = v, (bitmapObj as RotationImage).RotateY, targetRotateY, Easing.CubicInOut);
                albumAnimation.Add(0, 1, animation3);

            }

            if (targetTransY!=(bitmapObj as RotationImage).TransY)
            {
                var animation5 = new Animation(v => (bitmapObj as RotationImage).TransY = v, (bitmapObj as RotationImage).TransY, targetTransY, Easing.CubicInOut);
                albumAnimation.Add(0, 1, animation5);

            }
            albumAnimation.Add(0, 1, animation1);
            albumAnimation.Add(0, 1, animation2);

            albumAnimation.Commit((bitmapLayout as VisualElement), "AlbumArtImageAnimation", 16, duration);



        }
        //Debug.WriteLine(sb.ToString());
    }



    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        var currentPos = (int)Math.Floor(e.NewValue*  (this.BoxLayout.Children.Count-1));
        if (this.currentPos!=currentPos)
        {
            this.currentPos = currentPos;
            RenderTransform(this.currentPos);

        }
    }


    private void OnSwiped(object sender, SwipedEventArgs e)
    {
        this.currentPos=e.Direction==SwipeDirection.Right
            ? Math.Max(0, this.currentPos-1)
            : Math.Min(this.BoxLayout.Children.Count-1, this.currentPos+1);

        RenderTransform(this.currentPos);
        ProgressSlider.Value= this.currentPos/(this.BoxLayout.Children.Count-1);

    }
}

