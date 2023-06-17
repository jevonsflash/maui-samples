using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using HoldDownButtonGroup.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;

namespace HoldDownButtonGroup;

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

    private async void CircleSliderSamplePage_Clicked_1(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new CircleSliderSamplePage());

    }


    private async void CircleProgressBarSamplePage_Clicked_1(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new CircleProgressBarSamplePage());

    }
}

