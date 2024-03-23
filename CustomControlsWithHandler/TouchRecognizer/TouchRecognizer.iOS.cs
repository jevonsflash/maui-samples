using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using Lession2.TouchRecognizer;
using UIKit;

namespace Lession2.TouchRecognizer
{
    public partial class TouchRecognizer : UIGestureRecognizer, IDisposable
    {
        UIView iosView;

        public TouchRecognizer(UIView view)
        {
            this.iosView = view;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.Handle.ToInt64();
                InvokeTouchActionEvent(this, id, TouchActionType.Pressed, touch, true);
            }


        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.Handle.ToInt64();

                InvokeTouchActionEvent(this, id, TouchActionType.Moved, touch, true);

            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.Handle.ToInt64();

                InvokeTouchActionEvent(this, id, TouchActionType.Released, touch, false);

            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.Handle.ToInt64();

                InvokeTouchActionEvent(this, id, TouchActionType.Cancelled, touch, false);

            }
        }


        void InvokeTouchActionEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            var cgPoint = touch.LocationInView(recognizer.View);
            var xfPoint = new Point(cgPoint.X, cgPoint.Y);
            OnTouchActionInvoked?.Invoke(this, new TouchActionEventArgs(id, actionType, xfPoint, isInContact));
        }
    }
}