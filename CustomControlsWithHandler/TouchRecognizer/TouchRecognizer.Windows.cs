using Lession2.TouchRecognizer;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Lession2.TouchRecognizer
{
    public partial class TouchRecognizer : IDisposable
    {
        FrameworkElement windowsView;

        public TouchRecognizer(FrameworkElement view)
        {
            this.windowsView = view;
            if (this.windowsView != null)
            {
                this.windowsView.PointerEntered += View_PointerEntered;
                this.windowsView.PointerPressed += View_PointerPressed;
                this.windowsView.Tapped +=View_Tapped;
                this.windowsView.PointerMoved += View_PointerMoved;
                this.windowsView.PointerReleased += View_PointerReleased;
                this.windowsView.PointerExited += View_PointerExited;
                this.windowsView.PointerCanceled += View_PointerCancelled;
            }
        }

        public partial void Dispose()
        {
            windowsView.PointerEntered -= View_PointerEntered;
            windowsView.PointerPressed -= View_PointerPressed;
            windowsView.Tapped -=View_Tapped;
            windowsView.PointerMoved -= View_PointerMoved;
            windowsView.PointerReleased -= View_PointerReleased;
            windowsView.PointerExited -= View_PointerEntered;
            windowsView.PointerCanceled -= View_PointerCancelled;
        }
        private void View_Tapped(object sender, TappedRoutedEventArgs args)
        {
            //var windowsPoint = args.GetPosition(sender as UIElement);
            //Point point = new Point(windowsPoint.X, windowsPoint.Y);
            //InvokeTouchActionEvent(TouchActionType.Pressed, point, 0, true);

        }
        private void View_PointerEntered(object sender, PointerRoutedEventArgs args)
        {
            Point point = GetPoint(sender, args);
            var id = args.Pointer.PointerId;
            var isInContact = args.Pointer.IsInContact;
            InvokeTouchActionEvent(TouchActionType.Entered, point, id, isInContact);
        }

        private void View_PointerPressed(object sender, PointerRoutedEventArgs args)
        {
            Point point = GetPoint(sender, args);
            var id = args.Pointer.PointerId;
            var isInContact = args.Pointer.IsInContact;
            InvokeTouchActionEvent(TouchActionType.Pressed, point, id, isInContact);
            (sender as FrameworkElement).CapturePointer(args.Pointer);
        }

        private void View_PointerMoved(object sender, PointerRoutedEventArgs args)
        {
            Point point = GetPoint(sender, args);
            var id = args.Pointer.PointerId;
            var isInContact = args.Pointer.IsInContact;
            InvokeTouchActionEvent(TouchActionType.Moved, point, id, isInContact);
        }

        private void View_PointerReleased(object sender, PointerRoutedEventArgs args)
        {
            Point point = GetPoint(sender, args);
            var id = args.Pointer.PointerId;
            var isInContact = args.Pointer.IsInContact;
            InvokeTouchActionEvent(TouchActionType.Released, point, id, isInContact);
        }

        private void View_PointerExited(object sender, PointerRoutedEventArgs args)
        {
            Point point = GetPoint(sender, args);
            var id = args.Pointer.PointerId;
            var isInContact = args.Pointer.IsInContact;
            InvokeTouchActionEvent(TouchActionType.Exited, point, id, isInContact);
        }

        private void View_PointerCancelled(object sender, PointerRoutedEventArgs args)
        {
            Point point = GetPoint(sender, args);
            var id = args.Pointer.PointerId;
            var isInContact = args.Pointer.IsInContact;
            InvokeTouchActionEvent(TouchActionType.Cancelled, point, id, isInContact);
        }

        private void InvokeTouchActionEvent(TouchActionType touchActionType, Point point, uint id, bool isInContact)
        {
            OnTouchActionInvoked?.Invoke(this, new TouchActionEventArgs(id, touchActionType, point, isInContact));

        }

        private static Point GetPoint(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(sender as UIElement);
            Windows.Foundation.Point windowsPoint = pointerPoint.Position;
            Point point = new Point(windowsPoint.X, windowsPoint.Y);
            return point;
        }
    }
}
