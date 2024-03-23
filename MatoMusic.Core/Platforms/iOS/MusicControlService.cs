using AVFoundation;
using Foundation;
using MauiSample.Common.Helper;
using MatoMusic.Core.Interfaces;

namespace MatoMusic.Core
{
    public partial class MusicControlService : IMusicControlService
    {

        private NSError nserror = new NSError();

  

        private void OnFinishedPlaying(object sender, AVStatusEventArgs e)
        {
            //另开辟线程完成播放对象的Dispose
            new Thread(new ThreadStart(() =>
              {
                  OnPlayFinished?.Invoke(null, e.Status);
              })).Start();
        }


      
        private AVAudioPlayer CurrentIosPlayer;


        public partial async Task RebuildMusicInfos(Action callback)
        {
            musicInfos = await MusicInfoManager.GetQueueEntry();
            OnRebuildMusicInfosFinished?.Invoke(this, EventArgs.Empty);
            callback?.Invoke();
        }



        public partial double Duration()
        {
           
                if (CurrentIosPlayer == null)
                {

                    return 1.0;
                }
                return CurrentIosPlayer.Duration;

            
        }


        public partial double CurrentTime()
        {
           
                if (CurrentIosPlayer == null)
                {
                    return default;
                }
                return CurrentIosPlayer.CurrentTime;
            
        }


        public partial bool IsPlaying()
        {
           
                if (CurrentIosPlayer == null)
                {
                    return default;
                }
                return CurrentIosPlayer.Playing;
            
        }

        public partial bool IsInitFinished() { return CurrentIosPlayer != null; }

        public partial void SeekTo(double position)

        {
            if (!IsInitFinished()) { return; }
            CurrentIosPlayer.CurrentTime = position;
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
            var current = MusicInfos;
            var result = current.IndexOf(current.FirstOrDefault(c => c.Id == musicInfo.Id));
            return result;
        }

        public partial MusicInfo GetMusicByIndex(int index)
        {
            var result = MusicInfos[index];
            return result;
        }

        public partial async Task InitPlayer(MusicInfo musicInfo)
        {
            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
            AVAudioSession.SharedInstance().SetActive(true);
            if (CurrentIosPlayer != null)
            {
                Stop();
                CurrentIosPlayer.Dispose();
            }

            try
            {
                using (var fileStream = await FileSystem.Current.OpenAppPackageFileAsync(musicInfo.Url))
                {
                    var data = NSData.FromStream(fileStream);
                    CurrentIosPlayer = new AVAudioPlayer(data, "", out nserror);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CurrentIosPlayer = null;
                return;
            }
            //注册完成播放事件
            CurrentIosPlayer.FinishedPlaying -= new EventHandler<AVStatusEventArgs>(OnFinishedPlaying);
            CurrentIosPlayer.FinishedPlaying += new EventHandler<AVStatusEventArgs>(OnFinishedPlaying);


        }

        public partial void Play(MusicInfo currentMusic)
        {
            if (!IsInitFinished()) { return; }
            CurrentIosPlayer?.Play();
            OnPlayStatusChanged?.Invoke(this, true);
        }

        public partial void Stop()
        {
            if (!IsInitFinished()) { return; }

            if (CurrentIosPlayer.Playing)
            {
                CurrentIosPlayer.Stop();
                OnPlayStatusChanged?.Invoke(this, false);

            }
        }

        public partial void PauseOrResume()
        {
            if (!IsInitFinished()) { return; }

            var status = CurrentIosPlayer.Playing;
            PauseOrResume(status);
        }

        public partial void PauseOrResume(bool status)
        {
            if (!IsInitFinished()) { return; }

            if (status)
            {
                CurrentIosPlayer.Pause();
                OnPlayStatusChanged?.Invoke(this, false);
            }
            else
            {
                CurrentIosPlayer.Play();
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
            if (!IsInitFinished()) { return; }
            CurrentIosPlayer.NumberOfLoops = isRepeatOne ? nint.MaxValue : 0;
        }
    }
}