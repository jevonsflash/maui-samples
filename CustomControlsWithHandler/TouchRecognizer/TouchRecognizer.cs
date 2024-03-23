using Lession2.TouchRecognizer;
using Microsoft.Maui.Platform;

namespace Lession2.TouchRecognizer
{
    public partial class TouchRecognizer: IDisposable
    {
        public event EventHandler<TouchActionEventArgs> OnTouchActionInvoked;
        public partial void Dispose();
    }
}
