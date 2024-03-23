using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using MauiSample.Common.Helper;
using MatoMusic.Core.Interfaces;
using System.Diagnostics;
using System.IO;

namespace MatoMusic.Core
{
    public class CompleteListener : Service, MediaPlayer.IOnCompletionListener
    {
        public event EventHandler<MediaPlayer> OnComplete;

        public void OnCompletion(MediaPlayer mp)
        {
            OnComplete?.Invoke(this, mp);

        }

        public override IBinder OnBind(Intent intent)
        {
            return null;

        }
    }
    public partial class MusicControlService : IMusicControlService
    {

        private MediaPlayer _currentAndroidPlayer;

        private MediaPlayer CurrentAndroidPlayer
        {

            set { _currentAndroidPlayer = value; }
            get
            {
                if (_currentAndroidPlayer == null)
                {

                    _currentAndroidPlayer = new MediaPlayer();
                    var cl = new CompleteListener();
                    _currentAndroidPlayer.SetOnCompletionListener(cl);
                    cl.OnComplete += Cl_OnComplete;
                }
                return _currentAndroidPlayer;
            }
        }

        private void Cl_OnComplete(object sender, MediaPlayer e)
        {
            if (!CurrentAndroidPlayer.Looping && e.Duration > 0.0)
            {
                OnPlayFinished?.Invoke(null, true);

            }
        }


        public partial async Task RebuildMusicInfos(Action callback)
        {
            musicInfos = await MusicInfoManager.GetQueueEntry();
            OnRebuildMusicInfosFinished?.Invoke(this, EventArgs.Empty);
            callback?.Invoke();
        }



        public partial double Duration() { return CurrentAndroidPlayer.Duration; }


        public partial double CurrentTime() { return CurrentAndroidPlayer.CurrentPosition; }


        public partial bool IsPlaying() { return CurrentAndroidPlayer.IsPlaying; }


        public partial bool IsInitFinished() { return true; }


        public partial void SeekTo(double position)
        {
            CurrentAndroidPlayer.SeekTo((int)position * 1000);
        }

        public partial MusicInfo GetNextMusic(MusicInfo current, bool isShuffle)
        {
            MusicInfo currentMusicInfo = null;
            if (current == null)
            {
                return null;
            }
            var index = GetMusicIndex(current);

            if (!isShuffle)
            {
                if (index + 1 > LastIndex)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
            else
            {
                index = GetShuffleMusicIndex(index, 1);
            }

            if (MusicInfos.Count != 0 && index != -1)
            {
                currentMusicInfo = MusicInfos[index];
            }
            return currentMusicInfo;
        }

        public partial MusicInfo GetPreMusic(MusicInfo current, bool isShuffle)
        {
            MusicInfo currentMusicInfo = null;

            if (current == null)
            {
                return null;
            }
            var index = GetMusicIndex(current);
            if (!isShuffle)
            {
                if (index - 1 < 0)
                {
                    index = LastIndex;
                }
                else
                {
                    index--;
                }
            }
            else
            {
                index = GetShuffleMusicIndex(index, -1);
            }

            if (MusicInfos.Count != 0 && index != -1)
            {
                currentMusicInfo = MusicInfos[index];
            }

            return currentMusicInfo;
        }

        public partial int GetMusicIndex(MusicInfo musicInfo)
        {
            var result = MusicInfos.IndexOf(MusicInfos.FirstOrDefault(c => c.Id == musicInfo.Id));
            return result;
        }

        public partial MusicInfo GetMusicByIndex(int index)
        {
            var result = MusicInfos[index];
            return result;
        }

        public partial async Task InitPlayer(MusicInfo musicInfo)
        {

            CurrentAndroidPlayer.Reset();

            var fd = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!.Assets!.OpenFd(musicInfo.Url);
            CurrentAndroidPlayer.SetDataSource(fd);

            Thread.Sleep(500);
            CurrentAndroidPlayer.Prepare();
        }

        public partial void Play(MusicInfo currentMusic)
        {
            if (currentMusic != null)
            {
                CurrentAndroidPlayer?.Start();
            }
        }

        public partial void Stop()
        {
            if (CurrentAndroidPlayer.IsPlaying)
            {
                CurrentAndroidPlayer.SeekTo(0);
                CurrentAndroidPlayer.Pause();

            }
        }

        public partial void PauseOrResume()
        {

            var status = CurrentAndroidPlayer.IsPlaying;
            PauseOrResume(status);
        }

        public partial void PauseOrResume(bool status)
        {

            if (status)
            {
                CurrentAndroidPlayer.Pause();
                OnPlayStatusChanged?.Invoke(this, false);
            }
            else
            {
                CurrentAndroidPlayer.Start();
                OnPlayStatusChanged?.Invoke(this, true);
            }

        }

        private partial int GetShuffleMusicIndex(int originItem, int increment)
        {
            var originItemIndex = 0;

            foreach (var item in ShuffleMap)
            {
                if (originItem == item)
                {
                    break;
                }
                originItemIndex++;
            }
            var newItemIndex = originItemIndex + increment;
            if (newItemIndex < 0)
            {
                newItemIndex = LastIndex;
            }
            if (newItemIndex > LastIndex)
            {
                newItemIndex = 0;
            }
            var shuffleMapCount = shuffleMap.Count();

            var musicInfosCount = MusicInfos.Count();

            if (shuffleMapCount != musicInfosCount)
            {
                shuffleMap = CommonHelper.GetRandomArry(0, LastIndex);
                shuffleMapCount = shuffleMap.Count();
            }

            if (shuffleMapCount > 0 && newItemIndex < shuffleMapCount)
            {
                var resultContent = ShuffleMap[newItemIndex];
                return resultContent;
            }
            else
            {
                return -1;
            }
        }

        public partial Task UpdateShuffleMap()
        {
            return Task.Run(() =>
            {
                shuffleMap = CommonHelper.GetRandomArry(0, LastIndex);
                return;
            });
        }

        public partial void SetRepeatOneStatus(bool isRepeatOne)
        {
            if (CurrentAndroidPlayer != null)
            {
                CurrentAndroidPlayer.Looping = isRepeatOne;
            }
        }
    }
}
