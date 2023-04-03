using CommunityToolkit.Mvvm.DependencyInjection;


namespace HoldAndSpeak;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Ioc.Default.ConfigureServices(
                  new ServiceCollection()
                  //ViewModels
                  .BuildServiceProvider());
        MainPage = new MainPage();
    }
}
