using System.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using MatoMusic.Common;
using MauiSample.Common.Helper;
using MatoMusic.Core.Interfaces;
using MatoMusic.ViewModels;
using MauiSample.Common.Controls;
using MauiSample.Common.Helper;
using MauiSample.Controls;

namespace MatoMusic.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NowPlayingPage : ContentPage
    {

        public IMusicInfoManager MusicInfoManager => Ioc.Default.GetRequiredService<IMusicInfoManager>();
        public NowPlayingPageViewModel MusicRelatedViewModel => this.BindingContext as NowPlayingPageViewModel;
        private Animation rotateAnimation;

        public NowPlayingPage()
        {
            InitializeComponent();
            this.BindingContext=Ioc.Default.GetRequiredService<NowPlayingPageViewModel>();

            Appearing += NowPlayingPage_Appearing;
            WeakReferenceMessenger.Default.Register<HorizontalPanActionArgs, string>(this, TokenHelper.PanAction, PanActionHandler);
        }


        private async void NowPlayingPage_Appearing(object sender, EventArgs e)
        {
            //DispatcherTimer.ClearCallBackFunction();
            await this.MusicRelatedViewModel.MusicRelatedService.InitAll();
            this.MusicRelatedViewModel.PropertyChanged+=MusicRelatedViewModel_PropertyChanged;
            this.DefaultPanContainer.PitLayout=this.PitContentLayout.Children.Select(c => c as PitGrid).ToList();


        }

        private async void MusicRelatedViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(MusicRelatedViewModel.IsPlaying))
            {
                if (MusicRelatedViewModel.IsPlaying)
                {
                    await this.AlbumNeedle.RotateTo(0, 300);
                    this.StartAlbumArtRotation();
                }
                else
                {
                    await this.AlbumNeedle.RotateTo(-30, 300);
                    this.StopAlbumArtRotation();

                }

            }
        }

        private async void PanActionHandler(object recipient, HorizontalPanActionArgs args)
        {
            switch (args.PanType)
            {
                case HorizontalPanType.Over:

                    if (MusicRelatedViewModel.IsPlaying)
                    {
                        await this.AlbumNeedle.RotateTo(0, 300);
                        this.StartAlbumArtRotation();
                    }


                    break;
                case HorizontalPanType.Start:

                    if (MusicRelatedViewModel.IsPlaying)
                    {
                        await this.AlbumNeedle.RotateTo(-30, 300);
                        this.StopAlbumArtRotation();
                    }
                    break;
                case HorizontalPanType.In:

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
                case HorizontalPanType.Out:



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
                    MusicRelatedViewModel.NextAction(null);
                    if (MusicRelatedViewModel.IsPlaying)
                    {
                        StartAlbumArtRotation();

                    }
                    break;

                case "MiddlePit":
                    // CommonHelper.GoNavigate("LibraryPage");

                    break;



                case "RightPit":

                    MusicRelatedViewModel.PreAction(null);
                    if (MusicRelatedViewModel.IsPlaying)
                    {
                        StartAlbumArtRotation();

                    }


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
            }
        }

        private void StopAlbumArtRotation()
        {
            this.AlbumArtImage.CancelAnimations();
            if (this.rotateAnimation!=null)
            {
                this.rotateAnimation.Dispose();
            }

        }

        private void StartAlbumArtRotation()
        {
            this.AlbumArtImage.AbortAnimation("AlbumArtImageAnimation");
            rotateAnimation = new Animation(v => this.AlbumArtImage.Rotation = v, this.AlbumArtImage.Rotation, this.AlbumArtImage.Rotation+ 360);
            rotateAnimation.Commit(this, "AlbumArtImageAnimation", 16, 20*1000, repeat: () => true);
        }

        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Width))
            {
                this.DefaultPanContainer.PositionX = (this.PitContentLayout.Width - (sender as Grid).Width) / 2;
                (App.Current as App).PanContainerWidth=  this.DefaultPanContainer.Width;

            }
            else if (e.PropertyName == nameof(Height))
            {
                this.DefaultPanContainer.PositionY = (this.PitContentLayout.Height - (sender as Grid).Height) / 2;

            }
        }
    }

}
