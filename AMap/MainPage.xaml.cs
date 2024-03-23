using AMap.ViewModels;
using System;

namespace AMap.Views;

public partial class MainPage : ContentPage
{
    private MainPageViewModel MainPageViewModel => this.BindingContext as MainPageViewModel;

    public MainPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        this.Loaded+=MainPage_Loaded;
        this.BindingContext = new ViewModels.MainPageViewModel();
        MainPageViewModel.OnFinishedChooise+=MainPageViewModel_OnFinishedChooise;
        rootComponent.Parameters =
            new Dictionary<string, object>
            {
                { "MainPageViewModel", MainPageViewModel }
            };
    }

    private void MainPageViewModel_OnFinishedChooise(object sender, FinishedChooiseEvenArgs e)
    {
        this.SelectorPopup.IsVisible=false;
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
        MainPageViewModel.Init();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        this.ContentFrame.IsVisible=true;
        this.ContentButton.IsVisible=false;
        this.ContentLabel.IsVisible=false;

    }

    private void Entry_Completed(object sender, EventArgs e)
    {
        EditDone();
    }


    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        EditDone();

    }
    private void EditDone()
    {
        this.ContentFrame.IsVisible=false;
        this.ContentButton.IsVisible=true;
        this.ContentLabel.IsVisible=true;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.SelectorPopup.IsVisible=true;

    }
}