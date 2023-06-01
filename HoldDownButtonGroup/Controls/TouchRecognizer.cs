using Microsoft.Maui.Platform;
using Region = MauiSample.Common.Common.Region;

namespace HoldDownButtonGroup.Controls
{
    public partial class TouchRecognizer
    {
        public event EventHandler<TouchActionEventArgs> OnTouchActionInvoked;
        public partial void Dispose();
    }
}
