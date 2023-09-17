using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tile
{

    public class TileSegmentService : ObservableObject, ITileSegmentService
    {

        public TileSegmentService(
            TileSegment tileSegment)
        {
            Remove = new Command(RemoveAction);
            TileSegment = tileSegment;

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

                var itemToMove = Container.TileSegments.First(i => i.IsBeingDragged);
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
            var itemToMove = Container.TileSegments.First(i => i.IsBeingDragged);

            if (itemToMove == null ||  itemToMove == this)
                return;


            Container.TileSegments.Remove(itemToMove);

            var insertAtIndex = Container.TileSegments.IndexOf(this);

            Container.TileSegments.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            IsBeingDraggedOver = false;
            DraggedItem=null;

        }

        private async void RemoveAction(object obj)
        {
            if (Container is ITileSegmentServiceContainer)
            {
                (Container as ITileSegmentServiceContainer).RemoveSegment.Execute(this);
            }
        }


        public IReadOnlyTileSegmentServiceContainer Container { get; set; }


        private TileSegment tileSegment;

        public TileSegment TileSegment
        {
            get { return tileSegment; }
            set
            {
                tileSegment = value;
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

        public Command Remove { get; set; }


        public Command Dragged { get; set; }

        public Command DraggedOver { get; set; }

        public Command DragLeave { get; set; }

        public Command Dropped { get; set; }
    }
}
