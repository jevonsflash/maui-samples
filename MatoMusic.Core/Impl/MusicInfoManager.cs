using System.Collections.ObjectModel;
using MatoMusic.Core.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;
using MatoMusic.Core.Models.Entities;
using Microsoft.International.Converters.PinYinConverter;
using System.Text.RegularExpressions;
using MatoMusic.Common;
using MatoMusic.Core.Interfaces;

namespace MatoMusic.Core
{
    public partial class MusicInfoManager : IMusicInfoManager
    {
        private const int MyFavouriteIndex = 1;

        List<MusicInfo> _musicInfos;
        private List<Queue> queueRepository;
        private List<PlaylistItem> playlistItemRepository;
        private List<Playlist> playlistRepository;

        public MusicInfoManager()
        {
            this.queueRepository = new List<Queue>();
            this.playlistItemRepository = new List<PlaylistItem>();
            this.playlistRepository = new List<Playlist>();
        }

        public async Task<bool> PermissionAuthorization<T>(T permission) where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();

            if (status == PermissionStatus.Granted)
                return true;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {

                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return false;
            }

            if (permission.ShouldShowRationale())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await permission.RequestAsync();

            return status == PermissionStatus.Granted;
        }

        public async Task<bool> MediaLibraryAuthorization()
        {
            var mediaPermission = await PermissionAuthorization(new Permissions.Media());
            var rdPermission = await PermissionAuthorization(new Permissions.StorageRead());
            var wtPermission = await PermissionAuthorization(new Permissions.StorageWrite());

            return mediaPermission && rdPermission && wtPermission;

            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            var status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var status3 = await Permissions.CheckStatusAsync<Permissions.Media>();

            if (status == PermissionStatus.Granted)
                return true;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return false;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();
            return status == PermissionStatus.Granted;

        }

        public async Task<List<PlaylistInfo>> GetPlaylistInfo()
        {
            return await Task.Run(async () =>
            {
                List<Playlist> playlist = await this.GetPlaylist();
                var playlistInfo = playlist.Select(c => new PlaylistInfo()
                {
                    Id = c.Id,
                    GroupHeader = GetGroupHeader(c.Title),
                    Title = c.Title,
                    IsHidden = c.IsHidden,
                    IsRemovable = c.IsRemovable,
                    Musics = new ObservableCollection<MusicInfo>(GetPlaylistEntry(c.Id).Result)
                }).ToList();
                return playlistInfo;
            });
        }



        public async Task<AlphaGroupedObservableCollection<AlbumInfo>> GetAlphaGroupedAlbumInfo()
        {
            AlphaGroupedObservableCollection<AlbumInfo> result = new AlphaGroupedObservableCollection<AlbumInfo>();
            List<AlbumInfo> list;
            var isSucc = await GetAlbumInfos();
            if (!isSucc.IsSucess)
            {
                //CommonHelper.ShowNoAuthorized();

            }
            list = isSucc.Result;
            list.ForEach(c =>
            {
                result.Add(c, c.GroupHeader);

            });
            result.Root = result.Root.Where(c => c.HasItems).ToList();
            return result;

        }


        public async Task<AlphaGroupedObservableCollection<ArtistInfo>> GetAlphaGroupedArtistInfo()
        {
            AlphaGroupedObservableCollection<ArtistInfo> result = new AlphaGroupedObservableCollection<ArtistInfo>();
            List<ArtistInfo> list;
            var isSucc = await GetArtistInfos();
            if (!isSucc.IsSucess)
            {
                //CommonHelper.ShowNoAuthorized();

            }
            list = isSucc.Result;
            list.ForEach(c =>
            {
                result.Add(c, c.GroupHeader);

            });
            result.Root = result.Root.Where(c => c.HasItems).ToList();
            return result;
        }


        public async Task<AlphaGroupedObservableCollection<MusicInfo>> GetAlphaGroupedMusicInfo()
        {

            AlphaGroupedObservableCollection<MusicInfo> result = new AlphaGroupedObservableCollection<MusicInfo>();
            List<MusicInfo> list;
            var isSucc = await GetMusicInfos();
            if (!isSucc.IsSucess)
            {
                //CommonHelper.ShowNoAuthorized();

            }
            list = isSucc.Result;

            list.ForEach(c =>
            {
                result.Add(c, c.GroupHeader);

            });
            result.Root = result.Root.Where(c => c.HasItems).ToList();
            return result;

        }

        public async Task<InfoResult<List<MusicInfo>>> GetMusicInfos()
        {
            List<MusicInfo> musicInfos;
            musicInfos = new List<MusicInfo>()
            {
                new MusicInfo()
                {
                    Id = 0,
                    Title = "No No",
                    Url = "No No.mp3",
                    Duration = 1000*60*3,

                    AlbumTitle = "测试专辑",
                    Artist ="测试艺术家",
                    AlbumArt = "p1.jpg",
                    GroupHeader = GetGroupHeader("No No"),
                    IsFavourite = GetIsMyFavouriteContains("No No").Result,
                    IsInitFinished = true
                },
                new MusicInfo()
                {
                    Id = 1,
                    Title = "Puzzle of My Heart",
                    Url = "Puzzle of My Heart.mp3",
                    Duration = 1000*60*3,

                    AlbumTitle = "测试专辑",
                    Artist ="测试艺术家",
                    AlbumArt = "p2.jpg",
                    GroupHeader = GetGroupHeader("Puzzle of My Heart"),
                    IsFavourite = GetIsMyFavouriteContains("Puzzle of My Heart").Result,
                    IsInitFinished = true
                },
                 new MusicInfo()
                {
                    Id = 2,
                    Title = "Safe",
                    Url = "Safe.mp3",
                    Duration = 1000*60*3,

                    AlbumTitle = "测试专辑",
                    Artist ="测试艺术家",
                    AlbumArt = "p3.jpg",
                    GroupHeader = GetGroupHeader("Safe"),
                    IsFavourite = GetIsMyFavouriteContains("Safe").Result,
                    IsInitFinished = true
                },
                  new MusicInfo()
                {
                    Id = 3,
                    Title = "What About Now",
                    Url = "What About Now.mp3",
                    Duration = 1000*60*3,

                    AlbumTitle = "测试专辑",
                    Artist ="测试艺术家",
                    AlbumArt = "p4.jpg",
                    GroupHeader = GetGroupHeader("What About Now"),
                    IsFavourite = GetIsMyFavouriteContains("What About Now").Result,
                    IsInitFinished = true
                }

            };
            var result = true;

            return new InfoResult<List<MusicInfo>>(result, musicInfos);

        }

        public async Task<InfoResult<List<ArtistInfo>>> GetArtistInfos()
        {
            List<ArtistInfo> artistInfo;
            artistInfo = new List<ArtistInfo>()
            {
                new ArtistInfo()
                {
                    Id = 0,
                    Title = "测试艺术家",
                    GroupHeader=GetGroupHeader("测试艺术家")
                }


            };
            var result = true;
            return new InfoResult<List<ArtistInfo>>(result, artistInfo);
        }
        public async Task<InfoResult<List<AlbumInfo>>> GetAlbumInfos()
        {
            List<AlbumInfo> albumInfos;
            albumInfos = new List<AlbumInfo>()
            {
               new AlbumInfo()
               {
                   Id = 0,
                   Title = "测试专辑",
                   GroupHeader = GetGroupHeader("测试专辑"),
               }


            };
            var result = true;
            return new InfoResult<List<AlbumInfo>>(result, albumInfos);
        }

        /// <summary>
        /// 将MusicInfo插入到队列
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> CreateQueueEntry(MusicInfo musicInfo)
        {
            var lastItemRank = queueRepository.OrderBy(c => c.Rank).Select(c => c.Rank).LastOrDefault();
            var entry = new Queue(musicInfo.Title, lastItemRank, musicInfo.Id);
            queueRepository.Add(entry);
            return true;
        }



        /// <summary>
        /// 将MusicInfo集合插入到队列中的末尾
        /// </summary>
        /// <param name="musicInfos"></param>
        /// <returns></returns>

        public async Task<bool> InsertToEndQueueEntrys(List<MusicInfo> musicInfos)
        {
            //var rankValue = 0;
            var sortedMusicInfos = musicInfos.Except(await GetQueueEntry(), new MusicInfoComparer()).ToList();
            var result = await CreateQueueEntrys(sortedMusicInfos);
            return result;
        }

        /// <summary>
        /// 将MusicInfo集合插入到队列
        /// </summary>
        /// <param name="musicInfos">需要进行操作的MusicInfo集合</param>
        /// <returns></returns>

        public async Task<bool> CreateQueueEntrys(List<MusicInfo> musicInfos)
        {
            var lastItemRank = queueRepository.OrderBy(c => c.Rank).Select(c => c.Rank).LastOrDefault();
            var entrys = new List<Queue>();
            foreach (var music in musicInfos)
            {
                var entry = new Queue(music.Title, lastItemRank, music.Id);
                lastItemRank++;
                entrys.Add(entry);
            }
            queueRepository.AddRange(entrys);
            return true;
        }

        /// <summary>
        /// 将MusicInfo集合插入到队列
        /// </summary>
        /// <param name="musics">需要进行操作的MusicInfo集合</param>
        /// <returns></returns>

        public async Task<bool> CreateQueueEntrys(MusicCollectionInfo musics)
        {
            return await CreateQueueEntrys(musics.Musics.ToList());
        }

        /// <summary>
        /// 从队列中读取MusicInfo
        /// </summary>
        /// <returns></returns>

        public async Task<List<MusicInfo>> GetQueueEntry()
        {
            var queueEntrys = queueRepository.ToList();
            if (_musicInfos == null || _musicInfos.Count == 0)
            {
                var isSucc = await GetMusicInfos();
                if (!isSucc.IsSucess)
                {
                    //CommonHelper.ShowNoAuthorized();
                }
                _musicInfos = isSucc.Result;

            }
            var result =
                from queue in queueEntrys
                join musicInfo in _musicInfos
                on queue.MusicInfoId equals musicInfo.Id
                orderby queue.Rank
                select musicInfo;
            return result.ToList();

        }

        /// <summary>
        /// 将MusicInfo插入到队列中的下一曲
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> InsertToNextQueueEntry(MusicInfo musicInfo, MusicInfo currentMusic)
        {
            var result = false;
            var isSuccessCreate = false;
            //如果没有则先创建
            if (!await GetIsQueueContains(musicInfo.Title))
            {
                isSuccessCreate = await CreateQueueEntry(musicInfo);

            }
            else
            {
                isSuccessCreate = true;
            }
            //确定包含后与下一曲交换位置
            if (isSuccessCreate)
            {
                var current = currentMusic;
                Queue newMusic = null;
                var lastItem = queueRepository.FirstOrDefault(c => c.MusicTitle==current.Title);
                if (lastItem!=null)
                {
                    newMusic =  queueRepository.FirstOrDefault(c => c.Rank==lastItem.Rank+1);
                }

                var oldMusic = queueRepository.FirstOrDefault(c => c.MusicTitle==musicInfo.Title);

                if (oldMusic ==null || newMusic==null)
                {
                    return true;
                }
                var oldMusicIndex = queueRepository.IndexOf(oldMusic);
                var newMusicIndex = queueRepository.IndexOf(newMusic);
                var oldRank = oldMusic.Rank;
                queueRepository[oldMusicIndex].Rank=newMusic.Rank;
                queueRepository[newMusicIndex].Rank=oldRank;


                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 将MusicInfo插入到队列中的末尾
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> InsertToEndQueueEntry(MusicInfo musicInfo)
        {
            var result = false;
            var isSuccessCreate = false;
            //如果没有则先创建    
            var queueEntry = queueRepository.FirstOrDefault(c => c.MusicTitle == musicInfo.Title);

            if (queueEntry == null)
            {
                isSuccessCreate = await CreateQueueEntry(musicInfo);
            }
            else
            {
                await DeleteMusicInfoFormQueueEntry(queueEntry.MusicTitle);
                isSuccessCreate = await CreateQueueEntry(musicInfo);
            }

            return isSuccessCreate;
        }


        /// <summary>
        /// 返回一个值表明一个Title是否包含在队列中
        /// </summary>
        /// <param name="musicTitle">music标题</param>
        /// <returns></returns>

        public async Task<bool> GetIsQueueContains(string musicTitle)
        {
            var queueEntrys = queueRepository.FirstOrDefault(c => c.MusicTitle == musicTitle);
            return queueEntrys is not null;
        }

        /// <summary>
        /// 从队列中删除指定MusicInfo
        /// </summary>
        /// <param name="musicTitle"></param>
        /// <returns></returns>

        public async Task<bool> DeleteMusicInfoFormQueueEntry(string musicTitle)
        {
            var entry = queueRepository.FirstOrDefault(c => c.MusicTitle == musicTitle);
            if (entry == null) return false;
            queueRepository.Remove(entry);
            return true;
        }

        /// <summary>
        /// 从队列中删除指定MusicInfo
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> DeleteMusicInfoFormQueueEntry(MusicInfo musicInfo)
        {
            var musicTitle = musicInfo.Title;
            var entry = queueRepository.FirstOrDefault(c => c.MusicTitle == musicTitle);
            if (entry == null) return false;
            queueRepository.Remove(entry);
            return true;
        }

        /// <summary>
        /// 交换队列中两个MusicInfo的位置
        /// </summary>
        /// <param name="oldMusicInfo"></param>
        /// <param name="newMusicInfo"></param>

        public void ReorderQueue(MusicInfo oldMusicInfo, MusicInfo newMusicInfo)
        {
            var oldMusic = queueRepository.FirstOrDefault(c => c.MusicTitle==oldMusicInfo.Title);
            var newMusic = queueRepository.FirstOrDefault(c => c.MusicTitle==newMusicInfo.Title);
            if (oldMusic ==null || newMusic==null)
            {
                return;
            }
            var oldMusicIndex = queueRepository.IndexOf(oldMusic);
            var newMusicIndex = queueRepository.IndexOf(newMusic);
            var oldRank = oldMusic.Rank;
            queueRepository[oldMusicIndex].Rank=newMusic.Rank;
            queueRepository[newMusicIndex].Rank=oldRank;
        }

        /// <summary>
        /// 从队列中清除所有MusicInfo
        /// </summary>

        public async Task ClearQueue()
        {
            queueRepository.RemoveAll(c => true);

        }

        /// <summary>
        /// 将MusicInfo插入到歌单
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylistEntry(MusicInfo musicInfo, long playlistId)
        {
            var lastItemRank = playlistItemRepository.OrderBy(c => c.Rank).Select(c => c.Rank).LastOrDefault();
            var entry = new PlaylistItem(playlistId, musicInfo.Id, musicInfo.Title, lastItemRank);
            playlistItemRepository.Add(entry);

            if (playlistId == MyFavouriteIndex)
            {
                musicInfo.SetFavourite(true, false);
            }

            return true;
        }

        /// <summary>
        /// 将MusicInfo集合插入到歌单
        /// </summary>
        /// <param name="musics"></param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylistEntrys(List<MusicInfo> musics, long playlistId)
        {
            var lastItemRank = playlistItemRepository.OrderBy(c => c.Rank).Select(c => c.Rank).LastOrDefault();
            var entrys = new List<PlaylistItem>();
            foreach (var music in musics)
            {
                var entry = new PlaylistItem(playlistId, music.Id, music.Title, lastItemRank);
                lastItemRank++;
                entrys.Add(entry);

            }

            playlistItemRepository.AddRange(entrys);

            if (playlistId == MyFavouriteIndex)
            {
                foreach (var musicInfo in musics)
                {
                    musicInfo.SetFavourite(true, false);
                }
            }

            return true;
        }

        /// <summary>
        /// 将MusicInfo集合插入到歌单
        /// </summary>
        /// <param name="musicCollectionInfo"></param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylistEntrys(MusicCollectionInfo musicCollectionInfo, long playlistId)
        {
            var result = await CreatePlaylistEntrys(musicCollectionInfo.Musics.ToList(), playlistId);
            return result;
        }


        /// <summary>
        /// 从歌单中删除MusicInfo根据指定的Title
        /// </summary>
        /// <param name="musicTitle"></param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> DeletePlaylistEntry(string musicTitle, long playlistId)
        {
            var entry = playlistItemRepository.FirstOrDefault(c => c.PlaylistId == playlistId && c.MusicTitle == musicTitle);
            if (entry != null)
            {
                playlistItemRepository.Remove(entry);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 从歌单中删除MusicInfo
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> DeletePlaylistEntry(MusicInfo musicInfo, long playlistId)
        {
            var result = await DeletePlaylistEntry(musicInfo.Title, playlistId);
            if (result)
            {
                musicInfo.SetFavourite(false, false);
            }
            return result;
        }


        /// <summary>
        /// 将MusicInfo插入到“我最喜爱”歌单
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylistEntryToMyFavourite(MusicInfo musicInfo)
        {
            var result = await CreatePlaylistEntry(musicInfo, MyFavouriteIndex);
            return result;

        }

        /// <summary>
        /// 将MusicInfo集合插入到“我最喜爱”
        /// </summary>
        /// <param name="musics"></param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylistEntrysToMyFavourite(List<MusicInfo> musics)
        {
            var result = await CreatePlaylistEntrys(musics, MyFavouriteIndex);
            return result;
        }

        /// <summary>
        /// 将MusicInfo集合插入到“我最喜爱”
        /// </summary>
        /// <param name="musicCollectionInfo"></param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylistEntrysToMyFavourite(MusicCollectionInfo musicCollectionInfo)
        {
            var result = await CreatePlaylistEntrys(musicCollectionInfo, MyFavouriteIndex);
            return result;
        }



        /// <summary>
        /// 从“我最喜爱”中删除MusicInfo
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> DeletePlaylistEntryFromMyFavourite(MusicInfo musicInfo)
        {
            return await DeletePlaylistEntry(musicInfo, MyFavouriteIndex);
        }

        /// <summary>
        /// 从歌单中读取MusicInfo
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<List<MusicInfo>> GetPlaylistEntry(long playlistId)
        {
            var currentPlaylistEntries = playlistItemRepository.Where(c => c.PlaylistId == playlistId);
            List<MusicInfo> musicInfos;

            var isSucc = await GetMusicInfos();
            if (!isSucc.IsSucess)
            {
                //CommonHelper.ShowNoAuthorized();

            }
            musicInfos = isSucc.Result;

            var result =
                from currentPlaylistEntry in currentPlaylistEntries
                join musicInfo in _musicInfos
                on currentPlaylistEntry.MusicInfoId equals musicInfo.Id
                orderby currentPlaylistEntry.Rank
                select musicInfo;
            return result.ToList();

        }

        /// <summary>
        /// 从“我最喜爱”中读取MusicInfo
        /// </summary>
        /// <returns></returns>

        public async Task<List<MusicInfo>> GetPlaylistEntryFormMyFavourite()
        {
            return await GetPlaylistEntry(MyFavouriteIndex);
        }

        /// <summary>
        /// 返回一个值表明一个Title是否包含在某个歌单中
        /// </summary>
        /// <param name="musicTitle">music标题</param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> GetIsPlaylistContains(string musicTitle, long playlistId)
        {
            var result = playlistItemRepository.FirstOrDefault(c => c.MusicTitle == musicTitle && c.PlaylistId == playlistId);
            return result is not null;

        }

        /// <summary>
        ///  返回一个值表明一个MusicInfo是否包含在某个歌单中
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> GetIsPlaylistContains(MusicInfo musicInfo, long playlistId)
        {
            return await GetIsPlaylistContains(musicInfo.Title, playlistId);
        }

        /// <summary>
        /// 返回一个值表明一个Title是否包含在"我最喜爱"中
        /// </summary>
        /// <param name="musicTitle">music标题</param>
        /// <returns></returns>

        public async Task<bool> GetIsMyFavouriteContains(string musicTitle)
        {
            return await GetIsPlaylistContains(musicTitle, MyFavouriteIndex);

        }

        /// <summary>
        ///  返回一个值表明一个MusicInfo是否包含在"我最喜爱"中
        /// </summary>
        /// <param name="musicInfo">musicInfo对象</param>
        /// <returns></returns>

        public async Task<bool> GetIsMyFavouriteContains(MusicInfo musicInfo)
        {
            return await GetIsPlaylistContains(musicInfo, MyFavouriteIndex);

        }

        /// <summary>
        /// 交换某歌单中两个MusicInfo的位置
        /// </summary>
        /// <param name="oldMusicInfo"></param>
        /// <param name="newMusicInfo"></param>
        /// <param name="playlistId"></param>

        public void ReorderPlaylist(MusicInfo oldMusicInfo, MusicInfo newMusicInfo, long playlistId)
        {
            var oldMusic = playlistItemRepository.Where(c => c.PlaylistId==playlistId).FirstOrDefault(c => c.MusicTitle==oldMusicInfo.Title);
            var newMusic = playlistItemRepository.Where(c => c.PlaylistId==playlistId).FirstOrDefault(c => c.MusicTitle==newMusicInfo.Title);
            if (oldMusic ==null || newMusic==null)
            {
                return;
            }
            var oldMusicIndex = playlistItemRepository.IndexOf(oldMusic);
            var newMusicIndex = playlistItemRepository.IndexOf(newMusic);
            var oldRank = oldMusic.Rank;
            playlistItemRepository[oldMusicIndex].Rank=newMusic.Rank;
            playlistItemRepository[newMusicIndex].Rank=oldRank;
        }
        /// <summary>
        /// 交换"我最喜爱"中两个MusicInfo的位置
        /// </summary>
        /// <param name="oldMusicInfo"></param>
        /// <param name="newMusicInfo"></param>

        public void ReorderMyFavourite(MusicInfo oldMusicInfo, MusicInfo newMusicInfo)
        {
            ReorderPlaylist(oldMusicInfo, newMusicInfo, MyFavouriteIndex);
        }
        /// <summary>
        /// 获取Playlist
        /// </summary>
        /// <returns></returns>

        public async Task<List<Playlist>> GetPlaylist()
        {
            return playlistRepository.ToList();

        }


        /// <summary>
        /// 创建Playlist
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>

        public async Task<bool> CreatePlaylist(Playlist playlist)
        {
            playlistRepository.Add(playlist);
            return true;
        }

        /// <summary>
        /// 更新Playlist
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>

        public async Task<bool> UpdatePlaylist(Playlist playlist)
        {
            playlistRepository.Add(playlist);
            return true;
        }

        /// <summary>
        /// 根据Id删除Playlist
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>

        public async Task<bool> DeletePlaylist(long playlistId)
        {
            playlistItemRepository.Remove(playlistItemRepository.FirstOrDefault(c => c.PlaylistId == playlistId));
            playlistRepository.Remove(playlistRepository.FirstOrDefault(c => c.Id==playlistId));
            return true;
        }

        /// <summary>
        /// 根据Playlist删除Playlist
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>

        public async Task<bool> DeletePlaylist(Playlist playlist)
        {
            return await DeletePlaylist(playlist.Id);

        }

        /// <summary>
        /// 获取一个字符串的标题头
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private string GetGroupHeader(string title)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(title))
            {


                if (Regex.IsMatch(title.Substring(0, 1), @"^[\u4e00-\u9fa5]+$"))
                {
                    try
                    {
                        var chinese = new ChineseChar(title.First());
                        result = chinese.Pinyins[0].Substring(0, 1);
                    }
                    catch (Exception ex)
                    {
                        return string.Empty;
                    }

                }
                else
                {
                    result = title.Substring(0, 1);
                }
            }
            return result;

        }

    }
}