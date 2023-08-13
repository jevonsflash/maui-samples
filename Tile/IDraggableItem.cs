namespace Tile
{
    public interface IDraggableItem
    {
        bool IsBeingDraggedOver { get; set; }
        bool IsBeingDragged { get; set; }
        Command Dragged { get; set; }
        Command DraggedOver { get; set; }

        Command DragLeave { get; set; }

        Command Dropped { get; set; }

        object DraggedItem { get; set; }

    }
}