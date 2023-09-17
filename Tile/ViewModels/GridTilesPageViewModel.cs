using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Tile.ViewModels
{

    public class GridTilesPageViewModel : ObservableObject, ITileSegmentServiceContainer
    {

        public GridTilesPageViewModel()
        {
            CreateSegment = new Command((obj) =>
            {
                CreateSegmentAction(obj, "NEW TILE", "Some description here", Colors.LightYellow);
            });

            InsertSegment = new Command((obj) =>
            {
                var index = Math.Max(this.TileSegments.Count-1, 0);
                InsertSegmentAction(index, obj, "NEW TILE", "Some description here", Colors.LightYellow);
            });

            RemoveSegment = new Command(RemoveSegmentAction);

            TileSegments = new ObservableCollection<ITileSegmentService>();
            CreateSegmentAction("TileSegment", "App1", "Some description here", Colors.LightPink);
            CreateSegmentAction("TileSegment", "App2", "Some description here", Colors.LightGreen);
            CreateSegmentAction("TileSegment", "App3", "Some description here", Colors.LightSalmon);
            CreateSegmentAction("TileSegment", "App4", "Some description here", Colors.LightSkyBlue);
            CreateSegmentAction("TileSegment", "App5", "Some description here", Colors.LightSeaGreen);
            CreateSegmentAction("TileSegment", "App6", "Some description here", Colors.LightSalmon);
            CreateSegmentAction("TileSegment", "App7", "Some description here", Colors.LightCyan);
            CreateSegmentAction("TileSegment", "App8", "Some description here", Colors.LightBlue);
            CreateSegmentAction("TileSegment", "App9", "Some description here", Colors.LightCyan);
            CreateSegmentAction("TileSegment", "App10", "Some description here", Colors.Azure);
            CreateSegmentAction("TileSegment", "App11", "Some description here", Colors.LimeGreen);
            CreateSegmentAction("TileSegment", "App12", "Some description here", Colors.LightYellow);
        }


        private void RemoveSegmentAction(object obj)
        {
            TileSegments.Remove(obj as ITileSegmentService);
        }

        private void CreateSegmentAction(object obj, string title, string desc, Color color)
        {

            var newModel = CreateTileSegmentService(obj, title, desc, color);
            TileSegments.Add(newModel);

        }

        private void InsertSegmentAction(int index, object obj, string title, string desc, Color color)
        {

            var newModel = CreateTileSegmentService(obj, title, desc, color);
            TileSegments.Insert(index, newModel);

        }

        private ITileSegmentService CreateTileSegmentService(object obj, string title, string desc, Color color)
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

            ITileSegmentService newModel = null;
            if (type=="TileSegment")
            {
                newModel = new GridTileSegmentService(tileSegment);
            }
            if (newModel != null)
            {
                newModel.Container = this;
            }
            return newModel;
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
        public Command InsertSegment { get; set; }
        public Command RemoveSegment { get; set; }
    }
}
