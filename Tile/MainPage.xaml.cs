﻿using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using Microsoft.Maui.Controls;
using Tile.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Tile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        this.BindingContext=new MainPageViewModel();
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private async void HorizontalTilesPage_Clicked(object sender, EventArgs e)
    {

        await App.Current.MainPage.Navigation.PushAsync(new HorizontalTilesPage());

    }

    private async void GridTilesPage_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new GridTilesPage());

    }
}

