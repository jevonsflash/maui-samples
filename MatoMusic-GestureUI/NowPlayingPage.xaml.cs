using System.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using MatoMusic.Common;
using MatoMusic.Controls;
using MauiSample.Common.Helper;
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
            this.ShuffleLabel.Text = FaIcons.IconRandom;
            this.RepeatOneLabel.Text = FaIcons.IconRepeat;
            this.SettingLabel.Text = FaIcons.IconCog;
            this.QueueLabel.Text = FaIcons.IconBars;
            this.FavouriteLabel.Text = FaIcons.IconHeart;
            this.PauseLabel.Text = FaIcons.IconPlay;
            this.PitTipLayout.Children.Clear();
            Appearing += NowPlayingPage_Appearing;
            WeakReferenceMessenger.Default.Register<PanActionArgs, string>(this, TokenHelper.PanAction, PanActionHandler);
        }


        private async void NowPlayingPage_Appearing(object sender, EventArgs e)
        {
            //DispatcherTimer.ClearCallBackFunction();
            SetupNormalMode();
            SetupFullScreenMode();
            await this.MusicRelatedViewModel.MusicRelatedService.InitAll();
            this.DefaultPanContainer.PitLayout=this.PitContentLayout.Children.Select(c => c as PitGrid).ToList();
        }

        private async void PanActionHandler(object recipient, PanActionArgs args)
        {
            var parentAnimation = new Animation();

            Animation scaleUpAnimation1;
            Animation scaleUpAnimation2;
            switch (args.PanType)
            {
                case PanType.Over:

                    scaleUpAnimation1 = new Animation(v => this.PitContentLayout.Opacity = v, PitContentLayout.Opacity, 0, Easing.CubicOut);
                    scaleUpAnimation2 = new Animation(v => this.TitleLayout.Opacity = v, TitleLayout.Opacity, 1, Easing.CubicOut);

                    parentAnimation.Add(0, 1, scaleUpAnimation1);
                    parentAnimation.Add(0, 1, scaleUpAnimation2);

                    parentAnimation.Commit(this, "RestoreAnimation", 16, 250);
                    MusicRelatedViewModel.EndFastSeeking();
                    if (_dispatcherTimer!=null)
                    {
                        _dispatcherTimer.Stop();

                    }
                    _runCount = 0;

                    this.TipTextLabel.Text = string.Empty;
                    this.TipLabel.Text = string.Empty;


                    SetupFullScreenMode();


                    break;
                case PanType.Start:

                    scaleUpAnimation1 = new Animation(v => this.PitContentLayout.Opacity = v, PitContentLayout.Opacity, 1, Easing.CubicOut);
                    scaleUpAnimation2 = new Animation(v => this.TitleLayout.Opacity = v, TitleLayout.Opacity, 0.2, Easing.CubicOut);

                    parentAnimation.Add(0, 1, scaleUpAnimation1);
                    parentAnimation.Add(0, 1, scaleUpAnimation2);

                    parentAnimation.Commit(this, "RestoreAnimation", 16, 250);

                    if (_dispatcherTimer2.IsRunning)
                    {
                        _dispatcherTimer2.Stop();
                    }
                    SetupNormalMode();

                    break;
                case PanType.In:

                    var child = args.CurrentPit?.Children[0];
                    if (child != null && child is Label)
                    {
                        var origin = (child as Label);
                        this.TipLabel.Text = origin.Text;
                    }

                    this.PitTipLayout.Children.Add(this.TipLabel);

                    switch (args.CurrentPit?.PitName)
                    {
                        case "LeftPit":

                            _dispatcherTimer =Dispatcher.CreateTimer();
                            _dispatcherTimer.Interval=new TimeSpan(0, 0, 2);

                            _dispatcherTimer.Tick+=   async (o, e) =>
                            {
                                this.TipLabel.Text = FaIcons.IconFastBackward;
                                this.TipTextLabel.Text = "快退";
                                _runCount++;
                                await MusicRelatedViewModel.StartFastSeeking(-2);


                            };
                            _dispatcherTimer.Start();
                            this.TipTextLabel.Text = "上一曲";

                            break;
                        case "TopPit":
                            this.TipTextLabel.Text = "库";


                            break;
                        case "LeftTopPit":
                            var strIsShuffle = MusicRelatedViewModel.IsShuffle ? "开" : "关";
                            this.TipTextLabel.Text = string.Format("{0} - {1}", "随机播放", strIsShuffle);
                            break;

                        case "RightTopPit":
                            var strIsRepeatOne = MusicRelatedViewModel.IsRepeatOne ? "开" : "关";
                            this.TipTextLabel.Text = string.Format("{0} - {1}", "单曲循环", strIsRepeatOne);
                            break;

                        case "RightPit":
                            _dispatcherTimer =Dispatcher.CreateTimer();
                            _dispatcherTimer.Interval=new TimeSpan(0, 0, 2);

                            _dispatcherTimer.Tick+=   async (o, e) =>
                            {
                                this.TipLabel.Text = FaIcons.IconFastForward;
                                this.TipTextLabel.Text = "快进";
                                _runCount++;
                                await MusicRelatedViewModel.StartFastSeeking(2);

                            };
                            _dispatcherTimer.Start();
                            this.TipTextLabel.Text = "下一曲";
                            break;
                        case "BottomPit":

                            this.TipTextLabel.Text = "队列";

                            break;
                        case "LeftBottomPit":
                            this.TipTextLabel.Text = "收藏";

                            break;
                        case "RightBottomPit":
                            this.TipTextLabel.Text = "设置";

                            break;
                        default:
                            break;
                    }


                    break;
                case PanType.Out:
                    this.PitTipLayout.Children.Clear();
                    if (this._runCount > 0)
                    {
                        MusicRelatedViewModel.EndFastSeeking();
                    }
                    if (_dispatcherTimer!=null)
                    {
                        _dispatcherTimer.Stop();

                    }
                    _runCount = 0;
                    this.TipTextLabel.Text = string.Empty;


                    break;
                default:
                    break;
            }

        }

        private void SetupFullScreenMode(int delay = 5)
        {
            _dispatcherTimer2 =Dispatcher.CreateTimer();
            _dispatcherTimer2.Interval=new TimeSpan(0, 0, delay);

            _dispatcherTimer2.Tick+=   (o, e) =>
            {

                this.MainCircleSlider.BorderWidth = 3;
                this.TitleLayout.FadeTo(0);

            };

            _dispatcherTimer2.Start();
        }

        private void SetupNormalMode()
        {
            this.MainCircleSlider.BorderWidth = 15;
            this.TitleLayout.FadeTo(1);
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
