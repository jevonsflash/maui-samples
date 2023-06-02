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

namespace HoldDownButtonGroup;

public partial class CircleProgressBarSamplePage : ContentPage
{
    private PitGrid _currentPit;
    private static readonly Random rnd = new Random();

    public PitGrid CurrentPitView { get; set; }

    public CircleProgressBarSamplePage()
    {
        InitializeComponent();

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

