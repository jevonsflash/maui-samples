namespace Tile
{
    public interface ITileSegmentService : IDraggableItem
    {
        TileSegment TileSegment { get; set; }
        IReadOnlyTileSegmentServiceContainer Container { get; set; }
    }
}