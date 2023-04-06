using System.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using MatoMusic.Common;
using MatoMusic.Core.Helper;
using MatoMusic.Core.Interfaces;
using MatoMusic.ViewModels;
using MauiSample.Common.Common;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;

namespace MatoMusic.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NowPlayingPage : ContentPage
    {
        private IDispatcherTimer _dispatcherTimer;
        private IDispatcherTimer _dispatcherTimer2;
        public IMusicInfoManager MusicInfoManager => Ioc.Default.GetRequiredService<IMusicInfoManager>();
        public NowPlayingPageViewModel MusicRelatedViewModel => this.BindingContext as NowPlayingPageViewModel;
        private int _runCount = 0;

        public NowPlayingPage()
        {
            InitializeComponent();
            this.BindingContext=Ioc.Default.GetRequiredService<NowPlayingPageViewModel>();
            this.RewLabel.Text = FaIcons.IconBackward;
            this.FfLabel.Text = FaIcons.IconForward;
            this.LibLabel.Text = FaIcons.IconSquareO;
            this.SettingLabel.Text = FaIcons.IconCog;
            this.QueueLabel.Text = FaIcons.IconBars;
            this.FavouriteLabel.Text = FaIcons.IconHeart;
            Appearing += NowPlayingPage_Appearing;
            WeakReferenceMessenger.Default.Register<PanActionArgs, string>(this, TokenHelper.PanAction, PanActionHandler);
        }


        private async void NowPlayingPage_Appearing(object sender, EventArgs e)
        {
            //DispatcherTimer.ClearCallBackFunction();
            await this.MusicRelatedViewModel.MusicRelatedService.InitAll();
            this.DefaultPanContainer.PitLayout=this.PitContentLayout.Children.Select(c => c as PitGrid).ToList();
        }

        private async void PanActionHandler(object recipient, PanActionArgs args)
        {
            switch (args.PanType)
            {
                case PanType.Over:

                    MusicRelatedViewModel.EndFastSeeking();
                    if (_dispatcherTimer!=null)
                    {
                        _dispatcherTimer.Stop();

                    }
                    _runCount = 0;

                    break;
                case PanType.Start:


                    if (_dispatcherTimer2.IsRunning)
                    {
                        _dispatcherTimer2.Stop();
                    }

                    break;
                case PanType.In:

                    switch (args.CurrentPit?.PitName)
                    {
                        case "LeftPit":

                            break;
                        case "TopPit":

                            break;
                        default:
                            break;
                    }


                    break;
                case PanType.Out:

                    if (this._runCount > 0)
                    {
                        MusicRelatedViewModel.EndFastSeeking();
                    }
                    if (_dispatcherTimer!=null)
                    {
                        _dispatcherTimer.Stop();

                    }
                    _runCount = 0;


                    break;
                default:
                    break;
            }

        }


        private void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (MusicRelatedViewModel.Canplay)
            {
                MusicRelatedViewModel.ChangeProgess(e.NewValue);
            }
        }



        private void DefaultPanContainer_OnOnfinishedChoise(object sender, PitGrid e)
        {
            CurrentPitView = e;
            switch (CurrentPitView.PitName)
            {
                case "LeftPit":

                    if (this._runCount > 0)
                    {
                        MusicRelatedViewModel.EndFastSeeking();
                        if (_dispatcherTimer!=null)
                        {
                            _dispatcherTimer.Stop();

                        }
                        _runCount = 0;

                    }
                    else
                    {
                        MusicRelatedViewModel.PreAction(null);
                    }

                    break;

                case "TopPit":
                    // CommonHelper.GoNavigate("LibraryPage");

                    break;

                case "LeftTopPit":
                    MusicRelatedViewModel.ShuffleAction(null);

                    break;

                case "RightTopPit":
                    MusicRelatedViewModel.RepeatOneAction(null);

                    break;

                case "RightPit":
                    if (this._runCount > 0)
                    {
                        MusicRelatedViewModel.EndFastSeeking();
                        if (_dispatcherTimer!=null)
                        {
                            _dispatcherTimer.Stop();

                        }
                        _runCount = 0;

                    }
                    else
                    {
                        MusicRelatedViewModel.NextAction(null);
                    }

                    break;
                case "BottomPit":
                    // CommonHelper.GoNavigate("QueuePage");

                    break;
                case "LeftBottomPit":



                    break;
                case "RightBottomPit":
                    // CommonHelper.GoNavigate("AboutPage");

                    break;
                default:
                    break;
            }
        }

        public PitGrid CurrentPitView { get; set; }

        private async void DefaultPanContainer_OnOnTapped(object sender, EventArgs e)
        {
            if (MusicRelatedViewModel.CurrentMusic != null)
            {
                MusicRelatedViewModel.PlayAction(null);

            }
            else
            {
                var isSucc = await MusicInfoManager.GetMusicInfos();
                if (!isSucc.IsSucess)
                {
                    CommonHelper.ShowNoAuthorized();


                }
                var musicInfos = isSucc.Result;
                if (musicInfos.Count > 0)
                {


                    var result = await MusicInfoManager.CreateQueueEntrys(musicInfos);
                    await this.MusicRelatedViewModel.RebuildMusicInfos();
                    if (result)
                    {
                        MusicRelatedViewModel.ChangeMusic(musicInfos[0]);
                    }

                }
                else
                {
                    // CommonHelper.GoNavigate("LibraryPage");

                }

            }
        }

        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Width))
            {
                this.DefaultPanContainer.PositionX = (this.PitContentLayout.Width - (sender as Grid).Width) / 2;
            }
            else if (e.PropertyName == nameof(Height))
            {
                this.DefaultPanContainer.PositionY = (this.PitContentLayout.Height - (sender as Grid).Height) / 2;

            }
        }
    }

}
