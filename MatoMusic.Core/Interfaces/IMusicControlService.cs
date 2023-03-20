using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Core.Interfaces
{
    public interface IMusicControlService
    {
        event EventHandler<bool> OnPlayFinished;
        event EventHandler OnRebuildMusicInfosFinished;
        event EventHandler<double> OnProgressChanged;
        event EventHandler<bool> OnPlayStatusChanged;
        public IMusicInfoManager MusicInfoManager { get; set; }

        int[] ShuffleMap { get; }
        List<MusicInfo> MusicInfos { get; }
        int LastIndex { get; }
        Task RebuildMusicInfos(Action callback);
        void SeekTo(double position);
        MusicInfo GetNextMusic(MusicInfo current, bool isShuffle);
        MusicInfo GetPreMusic(MusicInfo current, bool isShuffle);
        int GetMusicIndex(MusicInfo musicInfo);
        MusicInfo GetMusicByIndex(int index);
        Task InitPlayer(MusicInfo musicInfo);
        void Play(MusicInfo currentMusic);
        void Stop();
        void PauseOrResume();
        void PauseOrResume(bool status);
        Task UpdateShuffleMap();
        void SetRepeatOneStatus(bool isRepeatOne);
        double Duration();
        double CurrentTime();
        bool IsPlaying();
        bool IsInitFinished();
    }
}
