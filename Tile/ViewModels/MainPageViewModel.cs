using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Tile.ViewModels
{

    public class MainPageViewModel : ObservableObject, ITileSegmentServiceContainer
    {

        public MainPageViewModel()
        {
            CreateSegment = new Command((obj) =>
            {
                CreateSegmentAction(obj, "NEW TILE", "Some description here", Colors.DarkOrange);
            });
            RemoveSegment = new Command(RemoveSegmentAction);

            TileSegments = new ObservableCollection<ITileSegmentService>();
            CreateSegmentAction("SmallSegment", "App1", "Some description here", Colors.DarkGoldenrod);
            CreateSegmentAction("MediumSegment", "App2", "Some description here", Colors.DarkGreen);
            CreateSegmentAction("LargeSegment", "App3", "Some description here", Colors.DarkKhaki);
            CreateSegmentAction("SmallSegment", "App4", "Some description here", Colors.DarkGoldenrod);
            CreateSegmentAction("MediumSegment", "App5", "Some description here", Colors.DarkOliveGreen);
            CreateSegmentAction("LargeSegment", "App6", "Some description here", Colors.DarkSalmon);
        }


        private void RemoveSegmentAction(object obj)
        {
            TileSegments.Remove(obj as ITileSegmentService);
        }

        private void CreateSegmentAction(object obj, string title, string desc, Color color)
        {
            var type = obj as string;
            var tileSegment = new TileSegment()
            {
                Title = title,
                Type = type,
                Desc = desc,
                Icon = "dotnet_bot.svg",
                Color = color,
            };



            var newModel = new TileSegmentService(tileSegment);
            if (newModel != null)
            {
                newModel.Container = this;
                TileSegments.Add(newModel);
            }

        }





        private long tileId;

        public long TileId
        {
            get { return tileId; }
            set
            {
                if (tileId != value)
                {
                    tileId = value;
                    OnPropertyChanged();
                }

            }
        }




        private ObservableCollection<ITileSegmentService> _tileSegments;

        public ObservableCollection<ITileSegmentService> TileSegments
        {
            get { return _tileSegments; }
            set
            {
                _tileSegments = value;
                OnPropertyChanged();
            }
        }


        public Command CreateSegment { get; set; }
        public Command RemoveSegment { get; set; }
    }
}
