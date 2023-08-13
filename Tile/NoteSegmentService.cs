using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tile
{

    public enum NoteSegmentState
    {
        Config,
        Edit,
        PreView
    }
    public class NoteSegmentService : ObservableObject, INoteSegmentService
    {

        public NoteSegmentService(
            NoteSegment noteSegment)
        {
            Remove = new Command(RemoveAction);
            NoteSegment = noteSegment;
            NoteSegmentState = NoteSegmentState.Config;

            Dragged = new Command(OnDragged);
            DraggedOver = new Command(OnDraggedOver);
            DragLeave = new Command(OnDragLeave);
            Dropped = new Command(i => OnDropped(i));

        }

        private void OnDragged(object item)
        {
            IsBeingDragged=true;
            DraggedItem=item;
        }

        private void OnDraggedOver(object item)
        {
            if (!IsBeingDragged && item!=null)
            {
                IsBeingDraggedOver=true;

                var itemToMove = Container.NoteSegments.First(i => i.IsBeingDragged);
                if (itemToMove.DraggedItem!=null)
                {
                    DropPlaceHolderItem=itemToMove.DraggedItem;

                }
            }

        }


        private object _draggedItem;

        public object DraggedItem
        {
            get { return _draggedItem; }
            set
            {
                _draggedItem = value;
                OnPropertyChanged();
            }
        }

        private object _dropPlaceHolderItem;

        public object DropPlaceHolderItem
        {
            get { return _dropPlaceHolderItem; }
            set
            {
                _dropPlaceHolderItem = value;
                OnPropertyChanged();
            }
        }

        private void OnDragLeave(object item)
        {

            IsBeingDraggedOver = false;

        }

        private void OnDropped(object item)
        {
            var itemToMove = Container.NoteSegments.First(i => i.IsBeingDragged);

            if (itemToMove == null ||  itemToMove == this)
                return;


            Container.NoteSegments.Remove(itemToMove);

            var insertAtIndex = Container.NoteSegments.IndexOf(this);

            Container.NoteSegments.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            IsBeingDraggedOver = false;
            DraggedItem=null;

        }

        private async void RemoveAction(object obj)
        {
            if (Container is INoteSegmentServiceContainer)
            {
                (Container as INoteSegmentServiceContainer).RemoveSegment.Execute(this);
            }
        }


        public IReadOnlyNoteSegmentServiceContainer Container { get; set; }


        private NoteSegment noteSegment;

        public NoteSegment NoteSegment
        {
            get { return noteSegment; }
            set
            {
                noteSegment = value;
                OnPropertyChanged();

            }
        }

        private NoteSegmentState _isConfigState;

        public NoteSegmentState NoteSegmentState
        {
            get { return _isConfigState; }
            set
            {
                _isConfigState = value;
                OnPropertyChanged();

            }
        }


        private bool _isBeingDragged;
        public bool IsBeingDragged
        {
            get { return _isBeingDragged; }
            set
            {
                _isBeingDragged = value;
                OnPropertyChanged();

            }
        }

        private bool _isBeingDraggedOver;
        public bool IsBeingDraggedOver
        {
            get { return _isBeingDraggedOver; }
            set
            {
                _isBeingDraggedOver = value;
                OnPropertyChanged();

            }
        }

        public Command Submit { get; set; }
        public Command Create { get; set; }
        public Command Remove { get; set; }


        public Command Dragged { get; set; }

        public Command DraggedOver { get; set; }

        public Command DragLeave { get; set; }

        public Command Dropped { get; set; }
    }
}
