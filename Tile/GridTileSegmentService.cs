using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ThrottleDebounce;
using Tile.ViewModels;

namespace Tile
{

    public class GridTileSegmentService : ObservableObject, ITileSegmentService
    {
        public static object throttledLocker = new object();

        public static RateLimitedAction throttledAction = Debouncer.Debounce(null, TimeSpan.FromMilliseconds(500), leading: false, trailing: true);
        public GridTileSegmentService(
            TileSegment tileSegment)
        {
            Remove = new Command(RemoveAction);
            TileSegment = tileSegment;

            Dragged = new Command(OnDragged);
            DraggedOver = new Command(OnDraggedOver);
            DragLeave = new Command(OnDragLeave);
            Dropped = new Command(i => OnDropped(i));
            this.PropertyChanged+=GridTileSegmentService_PropertyChanged;
        }

        private void GridTileSegmentService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(this.IsBeingDraggedOver))
            {

                if (this.IsBeingDraggedOver && DropPlaceHolderItem!=null)
                {
                    // 使用防抖
                    lock (throttledLocker)
                    {
                        var newIndex = Container.TileSegments.IndexOf(this);
                        var oldIndex = Container.TileSegments.IndexOf(DropPlaceHolderItem as ITileSegmentService);

                        var originalAction = () =>
                        {
                            Container.TileSegments.Move(oldIndex, newIndex);
                        };
                        throttledAction.Update(originalAction);
                        throttledAction.Invoke();
                    }

                    // 无防抖
                    //var newIndex = Container.TileSegments.IndexOf(this);
                    //var oldIndex = Container.TileSegments.IndexOf(DropPlaceHolderItem as ITileSegmentService);
                    //Container.TileSegments.Move(oldIndex, newIndex);
                }
            }

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

                var itemToMove = Container.TileSegments.First(i => i.IsBeingDragged);
                if (itemToMove.DraggedItem!=null)
                {
                    DropPlaceHolderItem=itemToMove.DraggedItem;

                }
                IsBeingDraggedOver=true;

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
            DropPlaceHolderItem = null;
        }

        private void OnDropped(object item)
        {
            var itemToMove = Container.TileSegments.First(i => i.IsBeingDragged);

            if (itemToMove == null)
                return;


            itemToMove.IsBeingDragged = false;
            IsBeingDraggedOver = false;
            DraggedItem=null;
            DropPlaceHolderItem = null;

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
                if (value!=_isBeingDraggedOver)
                {
                    _isBeingDraggedOver = value;
                    OnPropertyChanged();
                }
            }
        }

        public Command Remove { get; set; }

        public Command Dragged { get; set; }

        public Command DraggedOver { get; set; }

        public Command DragLeave { get; set; }

        public Command Dropped { get; set; }
    }
}
