using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

using System.Diagnostics;


namespace StickyTab.Controls;

public partial class HoldDownButton : ContentView
{
    private IList<IView> appendedViews;


    public HoldDownButton()
    {
        InitializeComponent();
        appendedViews = new List<IView>();


        ViewHandler.ViewMapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
            if (appendedViews.Count > 0)
            {
                return;
            }

#if WINDOWS
            var touchRecognizer = new TouchRecognizer(handler.PlatformView as Microsoft.UI.Xaml.FrameworkElement);
            touchRecognizer.OnTouchActionInvoked += TouchRecognizer_OnTouchActionInvoked;
#endif
#if ANDROID
            var touchRecognizer = new TouchRecognizer(handler.PlatformView as Android.Views.View);
            touchRecognizer.OnTouchActionInvoked += TouchRecognizer_OnTouchActionInvoked;

#endif

#if IOS
            var touchRecognizer = new TouchRecognizer(handler.PlatformView as UIKit.UIView);
            touchRecognizer.OnTouchActionInvoked+=TouchRecognizer_OnTouchActionInvoked;

            (handler.PlatformView as UIKit.UIView).UserInteractionEnabled = true;
            (handler.PlatformView as UIKit.UIView).AddGestureRecognizer(touchRecognizer);
#endif
            this.appendedViews.Add(view);

        });
    }



    private void TouchRecognizer_OnTouchActionInvoked(object sender, TouchActionEventArgs e)
    {
        Debug.WriteLine(e.Type + " is Invoked, position:" + e.Location);
    }

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {

    }
}