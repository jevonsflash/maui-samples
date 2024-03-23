using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiSample.Common.Helper;
using MatoMusic.Core.Interfaces;
using MatoMusic.Core.Services;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace MatoMusic.Core.ViewModel
{
    public abstract class MusicRelatedViewModel : ObservableObject
    {
        private bool _isFastSeeking = false;
        public event EventHandler OnMusicChanged;

        public MusicRelatedViewModel()
        {
            this.PlayCommand = new Command(PlayAction, CanPlayExcute);
            this.PreCommand = new Command(PreAction, CanPlayExcute);
            this.NextCommand = new Command(NextAction, CanPlayExcute);
            this.RepeatOneCommand = new Command(RepeatOneAction, CanPlayExcute);
            this.ShuffleCommand = new Command(ShuffleAction, CanPlayExcute);
            this.FavouriteCommand = new Command(FavouriteAction, CanPlayExcute);
            this.PropertyChanged += DetailPageViewModel_PropertyChanged;
        }

        private void Delegate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //this.DetailPageViewModel_PropertyChanged(this, e);
            this.OnPropertyChanged(e.PropertyName);
        }

        public MusicInfo CurrentMusic
        {
            get => MusicRelatedService.CurrentMusic;
            set
            {
                MusicRelatedService.CurrentMusic = value;
                MusicControlService.Play(MusicRelatedService.CurrentMusic);

            }
        }

        public MusicInfo NextMusic
        {
            get => MusicRelatedService.NextMusic;
            set => MusicRelatedService.NextMusic = value;
        }

        public MusicInfo PreviewMusic
        {
            get => MusicRelatedService.PreviewMusic;
            set => MusicRelatedService.PreviewMusic = value;
        }

        public List<MusicInfo> Musics
        {
            get => MusicRelatedService.Musics;
            set => MusicRelatedService.Musics = value;
        }

        public bool Canplay
        {
            get => MusicRelatedService.Canplay;
        }

        public bool IsPlaying
        {
            get => MusicRelatedService.IsPlaying;
            set => MusicRelatedService.IsPlaying = value;
        }

        public bool IsShuffle
        {
            get => MusicRelatedService.IsShuffle;
            set => MusicRelatedService.IsShuffle = value;
        }

        public bool IsRepeatOne
        {
            get => MusicRelatedService.IsRepeatOne;
            set => MusicRelatedService.IsRepeatOne = value;
        }

        public double CurrentTime
        {
            get => MusicRelatedService.CurrentTime;
            set => MusicRelatedService.CurrentTime = value;
        }

        public double Duration
        {
            get => MusicRelatedService.Duration;
            set => MusicRelatedService.Duration = value;
        }

        public async Task RebuildMusicInfos(Action callback = null)
        {
            await this.MusicControlService.RebuildMusicInfos(callback);
        }



        private void FavouriteAction(object obj)
        {
            CurrentMusic.IsFavourite = !CurrentMusic.IsFavourite;
            if (CurrentMusic.IsFavourite)
            {
                CommonHelper.Alert("我最喜爱");

            }
        }


        public bool CanPlayExcute(object obj)
        {
            var result = MusicRelatedService.Canplay;
            return result;
        }

        public bool CanPlayAllExcute(object obj)
        {
            var result = MusicRelatedService.CanplayAll;
            return result;
        }

        #region methods
        private void MusicControlService_OnMusicChanged(Object sender, bool e)
        {
            NextAction(null);
        }


        private void DetailPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentMusic))
            {

                OnMusicChanged?.Invoke(this, EventArgs.Empty);
                RaiseCanPlayExecuteChanged();
            }

            else if (e.PropertyName == nameof(IsShuffle))
            {

                this.MusicRelatedService.DefalutIsShuffle=IsShuffle;

            }

            else if (e.PropertyName == nameof(IsRepeatOne))
            {
                this.MusicRelatedService.IsRepeatOne=IsRepeatOne;
            }

        }

        /// <summary>
        /// 可播放状态改变通知
        /// </summary>
        private void RaiseCanPlayExecuteChanged()
        {
            PlayCommand.ChangeCanExecute();
            PreCommand.ChangeCanExecute();
            NextCommand.ChangeCanExecute();
            ShuffleCommand.ChangeCanExecute();
            RepeatOneCommand.ChangeCanExecute();
            FavouriteCommand.ChangeCanExecute();
        }



        /// <summary>
        /// 下一曲
        /// </summary>
        /// <param name="obj"></param>
        public void NextAction(object obj)
        {
            var next = MusicControlService.GetNextMusic(this.CurrentMusic, IsShuffle);
            if (next != null)
            {
                this.CurrentMusic = next;

            }
        }

        /// <summary>
        /// 上一曲
        /// </summary>
        /// <param name="obj"></param>
        public void PreAction(object obj)
        {
            var pre = MusicControlService.GetPreMusic(this.CurrentMusic, IsShuffle);
            if (pre != null)
            {
                this.CurrentMusic = pre;

            }
        }

        /// <summary>
        /// 播放/暂停
        /// </summary>
        /// <param name="obj"></param>
        public void PlayAction(object obj)
        {
            MusicControlService.PauseOrResume();
        }

        /// <summary>
        /// 随机/顺序播放
        /// </summary>
        /// <param name="obj"></param>
        public void ShuffleAction(object obj)
        {
            this.IsShuffle = !this.IsShuffle;
        }

        /// <summary>
        /// 单曲循环/全列表播放
        /// </summary>
        /// <param name="obj"></param>
        public void RepeatOneAction(object obj)
        {
            this.IsRepeatOne = !this.IsRepeatOne;

        }

        /// <summary>
        /// 更改当前进度
        /// </summary>
        /// <param name="progress"></param>
        public void ChangeProgess(double progress)
        {
            if (Math.Abs(progress - MusicRelatedService.GetPlatformSpecificTime(MusicControlService.CurrentTime())) > 2.0)
            {
                MusicControlService.SeekTo(progress);
            }
        }

        public async Task StartFastSeeking(int increment)
        {
            var seekingDuration = 0;
            _isFastSeeking = true;
            await Task.Run((Action)(() =>
            {

                while (_isFastSeeking)
                {
                    var currentTime = MusicRelatedService.GetPlatformSpecificTime(MusicControlService.CurrentTime());

                    var seekingSpan = 0;

                    if (seekingDuration >= 0 && seekingDuration < 6)
                    {
                        seekingSpan = increment;
                    }
                    else if (seekingDuration >= 6 && seekingDuration < 12)
                    {
                        seekingSpan = increment * 4;
                    }
                    else if (seekingDuration >= 12)
                    {
                        seekingSpan = increment * 8;
                    }
                    this.MusicControlService.SeekTo(currentTime + seekingSpan);
                    Task.Delay(500).Wait();
                    Debug.WriteLine((string)("Fastseeking:" + currentTime + "\n SeekingDuration:" + seekingDuration));
                    seekingDuration++;
                }
            }));



        }

        public void EndFastSeeking()
        {
            this._isFastSeeking = false;
        }


        /// <summary>
        /// 以MusicInfo对象切歌
        /// </summary>
        /// <param name="musicInfo"></param>
        public virtual void ChangeMusic(MusicInfo musicInfo)
        {
            this.CurrentMusic = musicInfo;
        }

        /// <summary>
        /// 以歌曲标题切歌
        /// </summary>
        /// <param name="title"></param>
        public void ChangeMusic(string title)
        {
            this.CurrentMusic = Musics.FirstOrDefault(c => c.Title == title);
        }


        #endregion
        private IMusicInfoManager _musicInfoManager;

        public IMusicInfoManager MusicInfoManager
        {
            get
            {
                if (_musicInfoManager==null)
                {
                    _musicInfoManager=Ioc.Default.GetRequiredService<IMusicInfoManager>();
                }
                return _musicInfoManager;
            }

        }

        private IMusicControlService _musicControlService;

        public IMusicControlService MusicControlService
        {
            get
            {
                if (_musicControlService==null)
                {
                    _musicControlService = Ioc.Default.GetRequiredService<IMusicControlService>();
                    _musicControlService.OnPlayFinished += MusicControlService_OnMusicChanged;
                }
                return _musicControlService;
            }

        }

        private MusicRelatedService _musicRelatedService;

        public MusicRelatedService MusicRelatedService
        {
            get
            {
                if (_musicRelatedService==null)
                {
                    _musicRelatedService = Ioc.Default.GetRequiredService<MusicRelatedService>();
                    _musicRelatedService.PropertyChanged += this.Delegate_PropertyChanged;
                }
                return _musicRelatedService;
            }

        }
        public Command PlayCommand { get; set; }

        public Command PreCommand { get; set; }

        public Command NextCommand { get; set; }

        public Command ShuffleCommand { get; set; }

        public Command RepeatOneCommand { get; set; }

        public Command FavouriteCommand { get; set; }
    }
}

