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

namespace Coverflow;

public partial class MainPage : ContentPage
{

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
         
    }

    private void DefaultPanContainer_OnOnfinishedChoise(object sender, PitGrid e)
    {

    }


    private async void ContentPage_SizeChanged(object sender, EventArgs e)
    {
        var layoutWidth = this.MainLayout.DesiredSize.Width;

        var scrollY = this.MainScroller.ScrollY;
        var posX = this.MainScroller.ContentSize.Width-layoutWidth;
        await this.MainScroller.ScrollToAsync(posX, scrollY, false).ContinueWith((t) =>
        {
            RenderTransform(this.MainScroller.ScrollX);
        });

    }

    private async void ToggleAnimation_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PushAsync(new BezierModulatedLayoutPage());

    }

    private void RenderTransform(double scrollX)
    {
        var layoutWidth = this.MainLayout.DesiredSize.Width;
        if (this.BezeirPoints == null)
        {
            return;
        }


    }


    private void ContentPage_Loaded(object sender, EventArgs e)
    {


        BezeirPoints = new List<Point>();

        var bezeirPointSubdivs = 999;



        var p0 = new Point(0, 1);
        var p1 = new Point(0.1, 0.9988);
        var p2 = new Point(0.175, 0.9955);


        var p3 = new Point(0.4, 0.99);
        var p4 = new Point(0.575, 0.92);
        var p5 = new Point(0.7, 0.88);

        var p6 = new Point(0.775, 0.71);
        var p7 = new Point(0.9, 0.4);
        var p8 = new Point(1, 0);

        this.BezierSegments = new Point[][] {

            new Point[]{p0,p1,p2},
            new Point[]{p2,p3,p4},
            new Point[]{p4,p5,p6},
            new Point[] {p6,p7,p8}
       };

        for (int i = 0; i < this.BezierSegments.Length; i++)
        {
            for (int j = 0; j < bezeirPointSubdivs; j++)
            {
                var bezeirPointX = Math.Pow(1 - (double)j / bezeirPointSubdivs, 2) * BezierSegments[i][0].X + 2 * (double)j / bezeirPointSubdivs * (1 - (double)j / bezeirPointSubdivs) * BezierSegments[i][1].X + Math.Pow((double)j / bezeirPointSubdivs, 2) * BezierSegments[i][2].X;
                var bezeirPointY = Math.Pow(1 - (double)j / bezeirPointSubdivs, 2) * BezierSegments[i][0].Y + 2 * (double)j / bezeirPointSubdivs * (1 - (double)j / bezeirPointSubdivs) * BezierSegments[i][1].Y + Math.Pow((double)j / bezeirPointSubdivs, 2) * BezierSegments[i][2].Y;
                BezeirPoints.Add(new Point(bezeirPointX, bezeirPointY));

            }
        }
    }

    public double Modulate(double value, double[] source, double[] target)
    {
        if (source.Length != 2 || target.Length != 2)
        {
            throw new ArgumentOutOfRangeException();
        }

        var start = source[0];
        var end = source[1];
        var targetStart = target[0];
        var targetEnd = target[1];
        if (value < start || value > end)
        {
            return value;
        }
        var k = (value - start) / (end - start);
        var result = k * (targetEnd - targetStart) + targetStart;
        return result;
    }

    public double GetMappingY(double x)
    {
        foreach (var item in BezeirPoints)
        {
            if (x < item.X)
            {
                return item.Y;
            }
        }
        return default;
    }

    public List<Point> BezeirPoints { get; set; }
    public Point[][] BezierSegments { get; set; }


    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        RenderTransform(e.ScrollX);


    }
}

