using System.Collections.ObjectModel;

namespace Tile
{
    public interface IReadOnlyTileSegmentServiceContainer
    {
        ObservableCollection<ITileSegmentService> TileSegments { get; set; }

    }
}