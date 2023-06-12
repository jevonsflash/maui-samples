using Foundation;
using System;
using System.Collections.Specialized;
using UIKit;

namespace RichTextEditor.Controls
{

    public static class HtmlParser_iOS
    {
        static nfloat defaultSize = UIFont.SystemFontSize;
        static UIFont defaultFont;

        public static NSAttributedString HtmlToAttributedString(string htmlString)
        {
            var nsString = new NSString(htmlString);
            var data = nsString.Encode(NSStringEncoding.UTF8);
            var dictionary = new NSAttributedStringDocumentAttributes();
            dictionary.DocumentType = NSDocumentType.HTML;
            NSError error = new NSError();
            var attrString = new NSAttributedString(data, dictionary, ref error);
            var mutString = ResetFontSize(new NSMutableAttributedString(attrString));

            return mutString;
        }

        static NSAttributedString ResetFontSize(NSMutableAttributedString attrString)
        {
            defaultFont = UIFont.SystemFontOfSize(defaultSize);

            attrString.EnumerateAttribute(UIStringAttributeKey.Font, new NSRange(0, attrString.Length), NSAttributedStringEnumeration.None, (NSObject value, NSRange range, ref bool stop) =>
            {
                if (value != null)
                {
                    var oldFont = (UIFont)value;
                    var oldDescriptor = oldFont.FontDescriptor;

                    var newDescriptor = defaultFont.FontDescriptor;

                    bool hasBoldFlag = false;
                    bool hasItalicFlag = false;

                    if (oldDescriptor.SymbolicTraits.HasFlag(UIFontDescriptorSymbolicTraits.Bold))
                    {
                        hasBoldFlag = true;
                    }
                    if (oldDescriptor.SymbolicTraits.HasFlag(UIFontDescriptorSymbolicTraits.Italic))
                    {
                        hasItalicFlag = true;
                    }

                    if (hasBoldFlag && hasItalicFlag)
                    {
                        uint traitsInt = (uint)UIFontDescriptorSymbolicTraits.Bold + (uint)UIFontDescriptorSymbolicTraits.Italic;
                        newDescriptor = newDescriptor.CreateWithTraits((UIFontDescriptorSymbolicTraits)traitsInt);
                    }
                    else if (hasBoldFlag)
                    {
                        newDescriptor = newDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold);
                    }
                    else if (hasItalicFlag)
                    {
                        newDescriptor = newDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Italic);
                    }

                    var newFont = UIFont.FromDescriptor(newDescriptor, defaultSize);

                    attrString.RemoveAttribute(UIStringAttributeKey.Font, range);
                    attrString.AddAttribute(UIStringAttributeKey.Font, newFont, range);
                }

            });

            return attrString;
        }


        public static string AttributedStringToHtml(NSAttributedString attributedString)
        {
            var range = new NSRange(0, attributedString.Length);
            var dictionary = new NSAttributedStringDocumentAttributes();
            dictionary.DocumentType = NSDocumentType.HTML;
            NSError error = new NSError();
            var data = attributedString.GetDataFromRange(range, dictionary, ref error);
            var htmlString = new NSString(data, NSStringEncoding.UTF8);
            return htmlString;
        }
    }

}
