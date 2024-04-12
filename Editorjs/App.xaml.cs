using CommunityToolkit.Mvvm.DependencyInjection;
using Editorjs.Views;

namespace Editorjs;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Ioc.Default.ConfigureServices(
                  new ServiceCollection()
                  //ViewModels
                  .BuildServiceProvider());
        MainPage = new NavigationPage(new MainPage()) ;
    }
}
