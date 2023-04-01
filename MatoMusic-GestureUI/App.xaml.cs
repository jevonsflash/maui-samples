using CommunityToolkit.Mvvm.DependencyInjection;
using MatoMusic.Core;
using MatoMusic.Core.Interfaces;
using MatoMusic.Core.Services;
using MatoMusic.View;
using MatoMusic.ViewModels;

namespace MatoMusic;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Ioc.Default.ConfigureServices(
                  new ServiceCollection()
                  //ViewModels
                  .AddSingleton<MusicRelatedService>()
                  .AddSingleton<NowPlayingPageViewModel>()
                  .AddSingleton<IMusicInfoManager, MockMusicInfoManager>()
                  .AddSingleton<IMusicControlService, MusicControlService>()
                  .BuildServiceProvider());
        MainPage = new NowPlayingPage();
    }
}
