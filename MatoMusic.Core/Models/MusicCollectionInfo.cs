using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using MatoMusic.Infrastructure;
using MatoMusic.Infrastructure.Common;
using Microsoft.Maui.Controls;

namespace MatoMusic.Core
{
    public abstract class MusicCollectionInfo : ObservableObject, IBasicInfo
    {

        public long Id
        {
            get;
            set;
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }


        public string GroupHeader { get; set; }


        private ObservableCollection<MusicInfo> _musics;

        public ObservableCollection<MusicInfo> Musics
        {
            get
            {
                if (_musics == null)
                {
                    _musics = new ObservableCollection<MusicInfo>();
                }
                return _musics;

            }
            set
            {
                _musics = value;
                _musics.CollectionChanged += _musics_CollectionChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Time));
                OnPropertyChanged(nameof(Count));

            }
        }

        private void _musics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(Time));
                OnPropertyChanged(nameof(Count));
            }
        }

        public string Artist
        {
            get;
            set;
        }
        public ImageSource AlbumArt { get; set; }
        public string AlbumArtPath { get; set; }

        public int Count => Musics.Count();

        public string Time
        {
            get
            {
                var totalSec = Math.Truncate((double)Musics.Sum(c => (long)c.Duration));
                var totalTime = TimeSpan.FromSeconds(totalSec);
                var time = totalTime.ToString("g");
                return time;
            }
        }

    }
}