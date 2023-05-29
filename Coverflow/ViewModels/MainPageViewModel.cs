using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coverflow.ViewModels
{
    public class AlbumInfo
    {
        public AlbumInfo() { }

        public string AlbumName { get; set; }
        public string AlbumArtSource { get; set; }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            var list = new List<AlbumInfo>
            {
                new AlbumInfo() { AlbumName="Lavender Haze", AlbumArtSource= "p1.jpg"},
                new AlbumInfo() { AlbumName="Love Story", AlbumArtSource= "p2.jpg"},
                new AlbumInfo() { AlbumName="Sparks Fly", AlbumArtSource= "p3.jpg" },
                new AlbumInfo() { AlbumName="RED", AlbumArtSource= "p4.jpg" },
                new AlbumInfo() { AlbumName="Taylor Swift", AlbumArtSource= "p5.jpg" },
                new AlbumInfo() { AlbumName="Stay Stay Stay", AlbumArtSource= "p6.jpg" },
                new AlbumInfo() { AlbumName="FEARLESS", AlbumArtSource= "p7.jpg" },
                new AlbumInfo() { AlbumName="Novo Amor", AlbumArtSource= "p8.jpg" },
                new AlbumInfo() { AlbumName="Road to Rouen", AlbumArtSource= "p9.jpg" },
                new AlbumInfo() { AlbumName="My way", AlbumArtSource= "p10.jpg" },
                new AlbumInfo() { AlbumName="Born to Die", AlbumArtSource= "p11.jpg" },
                new AlbumInfo() { AlbumName="Hurts", AlbumArtSource= "p12.jpg" },
                new AlbumInfo() { AlbumName="2:00AM in LA", AlbumArtSource= "p13.jpg" },
                new AlbumInfo() { AlbumName="Mermaid", AlbumArtSource= "p14.jpg" },
                new AlbumInfo() { AlbumName="Modest Mouse", AlbumArtSource= "p15.jpg" },
                new AlbumInfo() { AlbumName="Jay - 范特西", AlbumArtSource= "p16.jpg" },
                new AlbumInfo() { AlbumName="Jay - 八度空间", AlbumArtSource= "p17.jpg" },
                new AlbumInfo() { AlbumName="Jay - 叶惠美", AlbumArtSource= "p18.jpg" },
                new AlbumInfo() { AlbumName="Jay - 七里香", AlbumArtSource= "p19.jpg" },
                new AlbumInfo() { AlbumName="Jay - 11月的肖邦", AlbumArtSource= "p20.jpg" },
            };

            AlbumInfos = new ObservableCollection<AlbumInfo>(list);
        }

        private ObservableCollection<AlbumInfo> _albumInfos;

        public ObservableCollection<AlbumInfo> AlbumInfos
        {
            get { return _albumInfos; }
            set
            {
                _albumInfos = value;
                NotifyPropertyChanged(nameof(AlbumInfos));
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
