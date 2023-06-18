using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CircleWidget;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.slider.Value = 20;
    }

    private void Button_Clicked2(object sender, EventArgs e)
    {
        this.slider.Value = 50;

    }
    private void Button_Clicked3(object sender, EventArgs e)
    {

        this.slider.Value = 80;
    }
}

