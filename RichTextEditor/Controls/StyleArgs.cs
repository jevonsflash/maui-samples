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
        public class StyleArgs : EventArgs
        {
            public StyleType Style;

            public string Params;
            public StyleArgs(StyleType style, string @params = null)
            {
                Style = style;
                Params=@params;
            }
        }
    }
}
