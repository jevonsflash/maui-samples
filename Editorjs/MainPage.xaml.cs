using Editorjs.ViewModels;
using System;

namespace Editorjs.Views;

public partial class MainPage : ContentPage
{
    private MainPageViewModel MainPageViewModel => this.BindingContext as MainPageViewModel;

    public MainPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        this.Loaded+=MainPage_Loaded;
        this.BindingContext = new ViewModels.MainPageViewModel();

    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}