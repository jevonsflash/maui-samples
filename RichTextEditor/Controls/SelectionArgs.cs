#if ANDROID

# endif
#if IOS
using Foundation;
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
# endif


namespace RichTextEditor.Controls
{
    public partial class WysiwygContentEditor
    {
        public class SelectionArgs : EventArgs
        {
            public int Start;
            public int End;
            public SelectionArgs(int start, int end)
            {
                Start = start;
                End = end;
            }
        }
    }
}
