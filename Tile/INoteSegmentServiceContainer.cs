using System.Collections.ObjectModel;

namespace Tile
{
    public interface INoteSegmentServiceContainer : IReadOnlyNoteSegmentServiceContainer
    {
        Command CreateSegment { get; set; }
        bool IsConfiguratingNoteSegment { get; set; }
        Command RemoveSegment { get; set; }
    }
}