using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultitaskingCardList.ViewModels
{
    public class AppTombStone
    {
        public AppTombStone() { }

        public string AppName { get; set; }
        public string AppScreen { get; set; }
        public double TestOffset { get; set; }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            var list = new List<AppTombStone>
            {
                new AppTombStone() { AppName="Edge", AppScreen= "p1.png",TestOffset=0},
                new AppTombStone() { AppName="Map", AppScreen= "p2.png",TestOffset=-10 },
                new AppTombStone() { AppName="Photo", AppScreen= "p3.png",TestOffset=-70 },
                new AppTombStone() { AppName="App Store", AppScreen= "p4.png" ,TestOffset=-90},
                new AppTombStone() { AppName="Calculator", AppScreen= "p5.png",TestOffset=-70 },
                new AppTombStone() { AppName="Music", AppScreen= "p6.png" ,TestOffset=-30},
                new AppTombStone() { AppName="File", AppScreen= "p7.png" },
                new AppTombStone() { AppName="Note", AppScreen= "p8.png" },
                new AppTombStone() { AppName="Paint", AppScreen= "p9.png" },
                new AppTombStone() { AppName="Weather", AppScreen= "p10.png" },
                new AppTombStone() { AppName="Chrome", AppScreen= "p11.png" },
                new AppTombStone() { AppName="Book", AppScreen= "p12.png" },
                new AppTombStone() { AppName="Browser", AppScreen= "p13.png" }
            };

            AppTombStones = new ObservableCollection<AppTombStone>(list);
        }

        private ObservableCollection<AppTombStone> _appTombStones;

        public ObservableCollection<AppTombStone> AppTombStones
        {
            get { return _appTombStones; }
            set
            {
                _appTombStones = value;
                NotifyPropertyChanged(nameof(AppTombStones));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
