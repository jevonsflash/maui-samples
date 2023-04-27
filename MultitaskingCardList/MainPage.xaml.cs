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

namespace MultitaskingCardList;

public partial class MainPage : ContentPage
{
    private const int bubbleSize = 20;
    private const int bubbleCnt = 30;
    private static readonly Random rnd = new Random();

    public PitGrid CurrentPitView { get; set; }

    public MainPage()
    {
        InitializeComponent();
        this.BindingContext=new MainPageViewModel();
        
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


    private void ContentPage_SizeChanged(object sender, EventArgs e)
    {



    }

    private async void ToggleAnimation_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new BezierModulatedLayoutPage());

    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }
}

