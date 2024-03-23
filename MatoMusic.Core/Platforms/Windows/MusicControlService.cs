using MatoMusic.Core.Interfaces;
using MauiSample.Common.Helper;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace MatoMusic.Core
{
    public partial class MusicControlService : IMusicControlService
    {

        private MediaPlayer _currentWindowsPlayer;

        private MediaPlayer CurrentWindowsPlayer
        {

            set { _currentWindowsPlayer = value; }
            get
            {
                if (_currentWindowsPlayer == null)
                {

                    _currentWindowsPlayer = new MediaPlayer();



                }
                return _currentWindowsPlayer;
            }
        }

        private void Cl_OnComplete(object sender, MediaPlayer e)
        {
            OnPlayFinished?.Invoke(null, true);
        }


        public partial async Task RebuildMusicInfos(Action callback)
        {
            musicInfos = await MusicInfoManager.GetQueueEntry();
            OnRebuildMusicInfosFinished?.Invoke(this, EventArgs.Empty);
            callback?.Invoke();
        }



        public partial double Duration() { return CurrentWindowsPlayer.PlaybackSession.NaturalDuration.TotalSeconds; }


        public partial double CurrentTime() { return CurrentWindowsPlayer.PlaybackSession.Position.TotalSeconds; }


        public partial bool IsPlaying() { return GetIsPlaying(CurrentWindowsPlayer.PlaybackSession.PlaybackState); }


        private bool GetIsPlaying(MediaPlaybackState status)
        {
            var result = false;
            switch (status)
            {
                case MediaPlaybackState.None:
                case MediaPlaybackState.Opening:
                case MediaPlaybackState.Buffering:
                case MediaPlaybackState.Paused:
                    result = false;
                    break;
                case MediaPlaybackState.Playing:
                    result = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
            return result;
        }

        public partial bool IsInitFinished() { return true; }


        public partial void SeekTo(double position)

        {
            CurrentWindowsPlayer.Position = new TimeSpan(0, 0, 0, (int)position);

        }

        public partial MusicInfo GetNextMusic(MusicInfo current, bool isShuffle)
        {
            MusicInfo currentMusicInfo = null;
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

            if (MusicInfos.Count != 0)
            {
                currentMusicInfo = MusicInfos[index];
            }
            return currentMusicInfo;
        }

        public partial MusicInfo GetPreMusic(MusicInfo current, bool isShuffle)
        {
            MusicInfo currentMusicInfo = null;
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

            if (MusicInfos.Count != 0)
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
            CurrentWindowsPlayer.CurrentStateChanged -= CurrentPlayer_CurrentStateChanged;

            CurrentWindowsPlayer.Dispose();
            CurrentWindowsPlayer = new MediaPlayer();

            var results = StorageFile.GetFileFromPathAsync(Path.Combine(FileSystem.Current.AppDataDirectory, musicInfo.Url));

            StorageFile file = await results;
            while (results.Status != Windows.Foundation.AsyncStatus.Completed)
            {

            }
            CurrentWindowsPlayer.Source =
                MediaSource.CreateFromStream(await file.OpenAsync(FileAccessMode.Read), file.ContentType);
            CurrentWindowsPlayer.CurrentStateChanged += CurrentPlayer_CurrentStateChanged; ;


        }

        private void CurrentPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Paused || sender.CurrentState == MediaPlayerState.Stopped)
            {
                OnPlayStatusChanged?.Invoke(this, false);

            }
            else if (sender.CurrentState == MediaPlayerState.Playing)
            {
                OnPlayStatusChanged?.Invoke(this, true);

            }
        }

        public partial void Play(MusicInfo currentMusic)
        {
            if (currentMusic != null)
            {
                CurrentWindowsPlayer?.Play();
            }
        }

        public partial void Stop()
        {
            if (GetIsPlaying(CurrentWindowsPlayer.PlaybackSession.PlaybackState))
            {
                CurrentWindowsPlayer.Dispose();
            }
        }

        public partial void PauseOrResume()
        {

            var status = GetIsPlaying(CurrentWindowsPlayer.PlaybackSession.PlaybackState);
            PauseOrResume(status);
        }

        public partial void PauseOrResume(bool status)
        {

            if (status)
            {
                CurrentWindowsPlayer.Pause();

            }
            else
            {
                CurrentWindowsPlayer.Play();
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
            try
            {
                var resultContent = ShuffleMap[newItemIndex];
                return resultContent;

            }
            catch (Exception e)
            {
                return ShuffleMap[0];
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
            if (CurrentWindowsPlayer != null)
            {
                CurrentWindowsPlayer.IsLoopingEnabled = isRepeatOne;
            }
        }

    }
}
