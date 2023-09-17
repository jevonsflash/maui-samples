using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tile.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace Tile
{

    public class GridTileSegmentService : ObservableObject, ITileSegmentService
    {
        public static object throttleLocker = new object();
        private static IDispatcherTimer _throttleDispatcher;
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
                    var newIndex = Container.TileSegments.IndexOf(this);
                    var oldIndex = Container.TileSegments.IndexOf(DropPlaceHolderItem as ITileSegmentService);
                    // 使用throttle
                    Monitor.Enter(throttleLocker);
                    bool needExit = true;
                    try
                    {

                        if (_throttleDispatcher==null)
                        {
                            _throttleDispatcher = Application.Current.Dispatcher.CreateTimer();
                            _throttleDispatcher.IsRepeating=false;
                            _throttleDispatcher.Interval=TimeSpan.FromSeconds(3);
                            _throttleDispatcher.Tick+=(o, e) =>
                            {
                                _throttleDispatcher.Stop();
                                _throttleDispatcher=null;

                            };
                            _throttleDispatcher.Start();
                            Monitor.Exit(throttleLocker);
                            needExit = false;
                            Debug.WriteLine("===========");
                            Debug.WriteLine("o:"+oldIndex+",n:"+newIndex);
                            Container.TileSegments.Move(oldIndex, newIndex);
                        }
                        else
                        {
                            Debug.WriteLine("===========");
                            Debug.WriteLine("ignored");

                        }
                    }
                    finally
                    {
                        if (needExit)
                            System.Threading.Monitor.Exit(throttleLocker);
                    }
                    // 不使用throttle
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
