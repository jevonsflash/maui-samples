using System.Collections.ObjectModel;

namespace Tile
{
    public interface ITileSegmentServiceContainer : IReadOnlyTileSegmentServiceContainer
    {
        Command CreateSegment { get; set; }
        Command RemoveSegment { get; set; }
    }
}