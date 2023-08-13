using System.Collections.ObjectModel;

namespace Tile
{
    public interface IReadOnlyNoteSegmentServiceContainer
    {
        ObservableCollection<INoteSegmentService> NoteSegments { get; set; }

    }
}