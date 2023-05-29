using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace StickyTab.Controls;

public partial class HoldDownButton : ContentView
{
    public HoldDownButton()
    {
        InitializeComponent();
        Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if WINDOWS
            var touchRecognizer = new TouchRecognizer(handler.PlatformView);
            touchRecognizer.OnTouchActionInvoked+=TouchRecognizer_OnTouchActionInvoked;  
#endif
#if ANDROID
            var touchRecognizer = new TouchRecognizer(handler.PlatformView);
            touchRecognizer.OnTouchActionInvoked+=TouchRecognizer_OnTouchActionInvoked;

#endif
#if IOS
            var touchRecognizer = new TouchRecognizer(handler.PlatformView);
            touchRecognizer.OnTouchActionInvoked+=TouchRecognizer_OnTouchActionInvoked;


            handler.PlatformView.UserInteractionEnabled = true;
            handler.PlatformView.AddGestureRecognizer(touchRecognizer);
#endif

        });
    }



    private void TouchRecognizer_OnTouchActionInvoked(object sender, TouchActionEventArgs e)
    {
        Debug.WriteLine(e.Type +" is Invoked, position:"+e.Location);
    }

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {

    }
}