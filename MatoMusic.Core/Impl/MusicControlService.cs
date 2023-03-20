using MatoMusic.Core.Helper;
using MatoMusic.Core.Interfaces;



namespace MatoMusic.Core
{
    public partial class MusicControlService : IMusicControlService
    {
        public IMusicInfoManager MusicInfoManager { get; set; }

        public event EventHandler<bool> OnPlayFinished;

        public event EventHandler OnRebuildMusicInfosFinished;

        public event EventHandler<double> OnProgressChanged;

        public event EventHandler<bool> OnPlayStatusChanged;

        public MusicControlService(IMusicInfoManager musicInfoManager)
        {
            this.MusicInfoManager=musicInfoManager;
        }


        private int[] shuffleMap;

        public int[] ShuffleMap
        {
            get
            {
                if (shuffleMap == null || shuffleMap.Length == 0)
                {
                    shuffleMap = CommonHelper.GetRandomArry(0, LastIndex);
                }
                return shuffleMap;
            }
        }



        private List<MusicInfo> musicInfos;

        public List<MusicInfo> MusicInfos
        {
            get
            {
                if (musicInfos == null || musicInfos.Count == 0)
                {
                    musicInfos = new List<MusicInfo>();
                }
                return musicInfos;
            }
        }




        public partial Task RebuildMusicInfos(Action callback);


        public int LastIndex { get { return MusicInfos.FindLastIndex(c => true); } }


        public partial double Duration();


        public partial double CurrentTime();


        public partial bool IsPlaying();


        public partial bool IsInitFinished();


        public partial void SeekTo(double position);


        public partial MusicInfo GetNextMusic(MusicInfo current, bool isShuffle);


        public partial MusicInfo GetPreMusic(MusicInfo current, bool isShuffle);


        public partial int GetMusicIndex(MusicInfo musicInfo);

        public partial MusicInfo GetMusicByIndex(int index);


        public partial Task InitPlayer(MusicInfo musicInfo);


        public partial void Play(MusicInfo currentMusic);



        public partial void Stop();



        public partial void PauseOrResume();



        public partial void PauseOrResume(bool status);




        public partial Task UpdateShuffleMap();



        public partial void SetRepeatOneStatus(bool isRepeatOne);

        private partial int GetShuffleMusicIndex(int originItem, int increment);

    }


}
