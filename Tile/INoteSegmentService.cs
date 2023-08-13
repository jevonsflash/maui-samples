namespace Tile
{
    public interface INoteSegmentService : IDraggableItem
    {
        NoteSegment NoteSegment { get; set; }
        NoteSegmentState NoteSegmentState { get; set; }
        IReadOnlyNoteSegmentServiceContainer Container { get; set; }


    }
}