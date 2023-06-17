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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var html = this.MainCircleWidget.GetHtmlText();
        await DisplayAlert("GetHtml()", html, "OK");


    }
}

