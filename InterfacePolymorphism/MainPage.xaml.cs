using Lession1.ViewModels;

namespace Lession1;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        this.BindingContext=new MainPageViewModel();
    }

    private void StateTrigger_IsActiveChanged(object sender, EventArgs e)
    {

    }
}

