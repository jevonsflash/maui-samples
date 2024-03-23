using Android.Views;
using Lession2.TouchRecognizer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Region = Lession2.TouchRecognizer.Region;

namespace Lession2.TouchRecognizer
{
    public partial class TouchRecognizer : IDisposable
    {
        Android.Views.View androidView;
        Func<double, double> fromPixels;
        int[] twoIntArray = new int[2];
        private Point _oldscreenPointerCoords;

        public TouchRecognizer(Android.Views.View view)
        {
            this.androidView = view;
            if (view != null)
            {
                fromPixels = view.Context.FromPixels;
                view.Touch += OnTouch;
            }
        }

        public partial void Dispose()
        {
            androidView.Touch -= OnTouch;
        }

        void OnTouch(object sender, Android.Views.View.TouchEventArgs args)
        {
            var senderView = sender as Android.Views.View;
            var motionEvent = args.Event;
            var pointerIndex = motionEvent.ActionIndex;
            var id = motionEvent.GetPointerId(pointerIndex);
            senderView.GetLocationOnScreen(twoIntArray);
            var screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                  twoIntArray[1] + motionEvent.GetY(pointerIndex));


            switch (args.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    InvokeTouchActionEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);
                    break;

                case MotionEventActions.Move:
                    for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                    {
                        id = motionEvent.GetPointerId(pointerIndex);


                        senderView.GetLocationOnScreen(twoIntArray);

                        screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                        twoIntArray[1] + motionEvent.GetY(pointerIndex));


                        if (IsOutPit(senderView, screenPointerCoords))
                        {
                            if (_oldscreenPointerCoords != default)
                            {
                                InvokeTouchActionEvent(this, id, TouchActionType.Exited, screenPointerCoords, true);
                                _oldscreenPointerCoords=default;
                            }

                        }
                        else
                        {
                            if (_oldscreenPointerCoords == default
                                                      ||screenPointerCoords!= _oldscreenPointerCoords)
                            {
                                _oldscreenPointerCoords=screenPointerCoords;
                                InvokeTouchActionEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
                            }
                        }


                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    InvokeTouchActionEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
                    break;

                case MotionEventActions.Cancel:

                    InvokeTouchActionEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
                    break;
            }
        }

        private bool IsOutPit(Android.Views.View senderView, Point screenPointerCoords)
        {
            return (screenPointerCoords.X<twoIntArray[0]||screenPointerCoords.Y<twoIntArray[1])
                                        ||(screenPointerCoords.X>twoIntArray[0]+senderView.Width||screenPointerCoords.Y>twoIntArray[1]+senderView.Height);
        }

        void InvokeTouchActionEvent(TouchRecognizer touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
        {
            touchEffect.androidView.GetLocationOnScreen(twoIntArray);
            double x = pointerLocation.X - twoIntArray[0];
            double y = pointerLocation.Y - twoIntArray[1];
            var point = new Point(fromPixels(x), fromPixels(y));
            OnTouchActionInvoked?.Invoke(this, new TouchActionEventArgs(id, actionType, point, isInContact));
        }

    }
}
