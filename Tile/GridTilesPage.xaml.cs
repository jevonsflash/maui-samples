using CommunityToolkit.Maui.Extensions;
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

public partial class GridTilesPage : ContentPage
{
    public GridTilesPage()
    {
        InitializeComponent();
        this.BindingContext=new GridTilesPageViewModel();
        NavigationPage.SetHasNavigationBar(this, false);

    }

}

