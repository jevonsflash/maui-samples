#if ANDROID
using Android.Graphics;
using Android.Text.Style;
using Android.Text;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

# endif
#if IOS
using Foundation;
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
# endif

using System;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Maui.Graphics.Text;

namespace RichTextEditor.Controls
{
    public partial class WysiwygContentEditor : Editor, INotifyPropertyChanged
    {
        public WysiwygContentEditor()
        {
            HandlerChanged+=WysiwygContentEditor_HandlerChanged;
            HandlerChanging+=WysiwygContentEditor_HandlerChanging;
            PropertyChanged+=WysiwygContentEditor_PropertyChanged;
        }

        private void WysiwygContentEditor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateMeasure();

        }

        private void WysiwygContentEditor_HandlerChanged(object sender, EventArgs e)
        {
            var handler = Handler;
            if (handler != null)
            {
#if WINDOWS
#endif
#if ANDROID
                var platformView = handler.PlatformView as AppCompatEditText;

                int getSelectionStart() => platformView.SelectionStart;
                int getSelectionEnd() => platformView.SelectionEnd;

                void SetEditableText(IEditable EditableText, AppCompatEditText platformView)
                {
                    var orginSelectionStart = getSelectionStart();
                    var orginSelectionEnd = getSelectionEnd();
                    platformView.TextFormatted = EditableText;
                    Focus();
                    platformView.SetSelection(orginSelectionStart, orginSelectionEnd);
                }


                GetHtmlRequest = new EventHandler(
                   (sender, e) =>
                        {
                            var editor = (WysiwygContentEditor)sender;
                            HtmlString=HtmlParser_Android.SpannedToHtml(platformView.EditableText);
                        }
                    );
                SetHtmlRequest =new EventHandler<string>(

                     (sender, htmlString) =>
                        {
                            platformView.TextFormatted = HtmlParser_Android.HtmlToSpanned(htmlString);
                        }
                    );

                void UpdateStyleSpans(TypefaceStyle flagStyle, IEditable EditableText)
                {
                    var styleSpans = EditableText.GetSpans(getSelectionStart(), getSelectionEnd(), Java.Lang.Class.FromType(typeof(StyleSpan)));
                    bool hasFlag = false;
                    var spanType = SpanTypes.InclusiveInclusive;

                    foreach (StyleSpan span in styleSpans)
                    {
                        var spanStart = EditableText.GetSpanStart(span);
                        var spanEnd = EditableText.GetSpanEnd(span);
                        var newStart = spanStart;
                        var newEnd = spanEnd;
                        var startsBefore = false;
                        var endsAfter = false;

                        if (spanStart < getSelectionStart())
                        {
                            newStart = getSelectionStart();
                            startsBefore = true;
                        }
                        if (spanEnd > getSelectionEnd())
                        {
                            newEnd = getSelectionEnd();
                            endsAfter = true;
                        }

                        if (span.Style == flagStyle)
                        {
                            hasFlag = true;
                            EditableText.RemoveSpan(span);
                            EditableText.SetSpan(new StyleSpan(TypefaceStyle.Normal), newStart, newEnd, spanType);
                        }
                        else if (span.Style == TypefaceStyle.BoldItalic)
                        {
                            hasFlag = true;
                            EditableText.RemoveSpan(span);
                            var flagLeft = TypefaceStyle.Bold;
                            if (flagStyle == TypefaceStyle.Bold)
                            {
                                flagLeft = TypefaceStyle.Italic;
                            }
                            EditableText.SetSpan(new StyleSpan(flagLeft), newStart, newEnd, spanType);
                        }

                        if (startsBefore)
                        {
                            EditableText.SetSpan(new StyleSpan(span.Style), spanStart, newStart, SpanTypes.ExclusiveExclusive);
                        }
                        if (endsAfter)
                        {
                            EditableText.SetSpan(new StyleSpan(span.Style), newEnd, spanEnd, SpanTypes.ExclusiveExclusive);
                        }

                    }
                    if (!hasFlag)
                    {
                        EditableText.SetSpan(new StyleSpan(flagStyle), getSelectionStart(), getSelectionEnd(), spanType);
                    }

                    SetEditableText(EditableText, platformView);
                }
                void UpdateUnderlineSpans(IEditable EditableText)
                {

                    var underlineSpans = EditableText.GetSpans(getSelectionStart(), getSelectionEnd(), Java.Lang.Class.FromType(typeof(UnderlineSpan)));

                    bool hasFlag = false;
                    var spanType = SpanTypes.InclusiveInclusive;

                    foreach (var span in underlineSpans)
                    {
                        hasFlag = true;

                        var spanStart = EditableText.GetSpanStart(span);
                        var spanEnd = EditableText.GetSpanEnd(span);
                        var newStart = spanStart;
                        var newEnd = spanEnd;
                        var startsBefore = false;
                        var endsAfter = false;

                        if (spanStart < getSelectionStart())
                        {
                            newStart = getSelectionStart();
                            startsBefore = true;
                        }
                        if (spanEnd > getSelectionEnd())
                        {
                            newEnd = getSelectionEnd();
                            endsAfter = true;
                        }

                        EditableText.RemoveSpan(span);

                        if (startsBefore)
                        {
                            EditableText.SetSpan(new UnderlineSpan(), spanStart, newStart, SpanTypes.ExclusiveExclusive);
                        }
                        if (endsAfter)
                        {
                            EditableText.SetSpan(new UnderlineSpan(), newEnd, spanEnd, SpanTypes.ExclusiveExclusive);
                        }
                    }

                    if (!hasFlag)
                    {
                        EditableText.SetSpan(new UnderlineSpan(), getSelectionStart(), getSelectionEnd(), spanType);
                    }
                    SetEditableText(EditableText, platformView);
                }



                void UpdateForegroundColorSpans(IEditable EditableText, Microsoft.Maui.Graphics.Color color)
                {
                    var spanType = SpanTypes.InclusiveInclusive;
                    EditableText.SetSpan(new ForegroundColorSpan(color.ToAndroid()), getSelectionStart(), getSelectionEnd(), spanType);
                    SetEditableText(EditableText, platformView);
                }

                void UpdateBackgroundColorSpans(IEditable EditableText, Microsoft.Maui.Graphics.Color color)
                {
                    var spanType = SpanTypes.InclusiveInclusive;
                    EditableText.SetSpan(new BackgroundColorSpan(color.ToAndroid()), getSelectionStart(), getSelectionEnd(), spanType);
                    SetEditableText(EditableText, platformView);
                }

                void UpdateAbsoluteSizeSpanSpans(IEditable EditableText, int size)
                {

                    var spanType = SpanTypes.InclusiveInclusive;

                    EditableText.SetSpan(new AbsoluteSizeSpan(size, true), getSelectionStart(), getSelectionEnd(), spanType);
                    SetEditableText(EditableText, platformView);
                }

                StyleChangeRequested =new EventHandler<StyleArgs>(
                    (sender, e) =>

                    {
                        var EditableText = platformView.EditableText;

                        switch (e.Style)
                        {
                            case StyleType.underline:
                                UpdateUnderlineSpans(EditableText);
                                break;
                            case StyleType.italic:
                                UpdateStyleSpans(TypefaceStyle.Italic, EditableText);
                                break;
                            case StyleType.bold:
                                UpdateStyleSpans(TypefaceStyle.Bold, EditableText);
                                break;
                            case StyleType.backgoundColor:
                                UpdateBackgroundColorSpans(EditableText, Microsoft.Maui.Graphics.Color.FromArgb(e.Params));
                                break;
                            case StyleType.foregroundColor:
                                UpdateForegroundColorSpans(EditableText, Microsoft.Maui.Graphics.Color.FromArgb(e.Params));
                                break;
                            case StyleType.size:
                                UpdateAbsoluteSizeSpanSpans(EditableText, int.Parse(e.Params));
                                break;
                            default:
                                break;
                        }


                    });

#endif

#if IOS
                var platformView = handler.PlatformView as UITextView;
                NSRange getSelectionRange() => platformView.SelectedRange;

                NSMutableAttributedString AttributedText;
                UIFont CurrentFont = null;
                bool CurrentUnderline = default;
                bool ChangeUnderline = default;
                NSMutableDictionary CurrentTypingAttributes = new NSMutableDictionary();

                GetHtmlRequest = new EventHandler(
                                      (sender, e) =>
                                      {
                                          var editor = (WysiwygContentEditor)sender;
                                          HtmlString=HtmlParser_iOS.AttributedStringToHtml(platformView.AttributedText);

                                      });

                SetHtmlRequest =new EventHandler<string>((sender, htmlString) =>
                {
                    platformView.AttributedText = HtmlParser_iOS.HtmlToAttributedString(htmlString);
                });

                void UpdateStyleAttributes(UIFontDescriptorSymbolicTraits fontAttr)
                {
                    bool hasFlag = false;
                    var selectionRange = getSelectionRange();

                    AttributedText.EnumerateAttribute(UIStringAttributeKey.Font, selectionRange, NSAttributedStringEnumeration.LongestEffectiveRangeNotRequired, (NSObject value, NSRange range, ref bool stop) =>
                    {
                        if (value != null)
                        {
                            var font = (UIFont)value;
                            var descriptor = font.FontDescriptor;

                            if (descriptor.SymbolicTraits.HasFlag(fontAttr))
                            {
                                hasFlag = true;
                                AttributedText.RemoveAttribute(UIStringAttributeKey.Font, range);

                                var newFont = UIFont.SystemFontOfSize(font.PointSize);
                                if (fontAttr == UIFontDescriptorSymbolicTraits.Bold && descriptor.SymbolicTraits.HasFlag(UIFontDescriptorSymbolicTraits.Italic))
                                {
                                    newFont = UIFont.ItalicSystemFontOfSize(font.PointSize);
                                }
                                else if (fontAttr == UIFontDescriptorSymbolicTraits.Italic && descriptor.SymbolicTraits.HasFlag(UIFontDescriptorSymbolicTraits.Bold))
                                {
                                    newFont = UIFont.BoldSystemFontOfSize(font.PointSize);
                                }
                                AttributedText.AddAttribute(UIStringAttributeKey.Font, newFont, range);
                                CurrentFont = newFont;
                            }
                        }
                    });

                    if (!hasFlag)
                    {

                        AttributedText.EnumerateAttribute(UIStringAttributeKey.Font, selectionRange, NSAttributedStringEnumeration.None, (NSObject value, NSRange range, ref bool stop) =>
                        {
                            if (value != null)
                            {
                                var font = (UIFont)value;
                                var descriptor = font.FontDescriptor;
                                var traits = descriptor.SymbolicTraits;
                                var newTraits = (UIFontDescriptorSymbolicTraits)((uint)fontAttr + (uint)traits);
                                var newDescriptor = descriptor.CreateWithTraits(newTraits);
                                var newFont = UIFont.FromDescriptor(newDescriptor, font.PointSize);
                                AttributedText.RemoveAttribute(UIStringAttributeKey.Font, range);
                                AttributedText.AddAttribute(UIStringAttributeKey.Font, newFont, range);
                                CurrentFont = newFont;
                            }
                        });
                    }
                }

                void UpdateUnderlineAttributes()
                {
                    var selectionRange = getSelectionRange();
                    bool hasFlag = false;

                    AttributedText.EnumerateAttribute(UIStringAttributeKey.UnderlineStyle, selectionRange, NSAttributedStringEnumeration.LongestEffectiveRangeNotRequired, (NSObject value, NSRange range, ref bool stop) =>
                    {
                        if (value != null)
                        {
                            var style = (NSNumber)value;
                            if (style.Equals(1))
                            {
                                hasFlag = true;
                                AttributedText.RemoveAttribute(UIStringAttributeKey.UnderlineStyle, range);
                                CurrentUnderline = false;
                            }
                        }
                    });

                    if (!hasFlag)
                    {
                        AttributedText.AddAttribute(UIStringAttributeKey.UnderlineStyle, (NSNumber)1, selectionRange);
                        CurrentUnderline = true;
                    }
                    ChangeUnderline = true;
                }

                void UpdateForegroundColorAttributes(Color color)
                {
                    var selectionRange = getSelectionRange();
                    AttributedText.AddAttribute(UIStringAttributeKey.ForegroundColor, color.ToUIColor(), selectionRange);
                }

                void UpdateBackgroundColorAttributes(Color color)
                {
                    var selectionRange = getSelectionRange();
                    AttributedText.AddAttribute(UIStringAttributeKey.BackgroundColor, color.ToUIColor(), selectionRange);
                }

                void UpdateFontSizeSpanAttributes(int size)
                {
                    UIFont newFont;
                    if (CurrentFont!=null)
                    {
                        newFont =  CurrentFont.WithSize((float)size);
                    }
                    else
                    {
                        newFont = UIFont.SystemFontOfSize((float)size);

                    }
                    var selectionRange = getSelectionRange();
                    AttributedText.AddAttribute(UIStringAttributeKey.Font, newFont, selectionRange);
                    CurrentFont=newFont;
                }



                StyleChangeRequested =new EventHandler<StyleArgs>(
                      (sender, e) =>

                      {
                          AttributedText = new NSMutableAttributedString(platformView.AttributedText);
                          if (platformView.TypingAttributes != null)
                          {
                              CurrentTypingAttributes = new NSMutableDictionary(platformView.TypingAttributes);
                          }
                          Debug.WriteLine("Rendering Style Change: " + e.Style);


                          switch (e.Style)
                          {
                              case StyleType.underline:
                                  var underlineAttr = UIStringAttributeKey.UnderlineStyle;
                                  UpdateUnderlineAttributes();
                                  break;
                              case StyleType.italic:
                                  UpdateStyleAttributes(UIFontDescriptorSymbolicTraits.Italic);
                                  break;
                              case StyleType.bold:
                                  UpdateStyleAttributes(UIFontDescriptorSymbolicTraits.Bold);
                                  break;
                              case StyleType.backgoundColor:
                                  UpdateBackgroundColorAttributes(Color.FromArgb(e.Params));

                                  break;
                              case StyleType.foregroundColor:
                                  UpdateForegroundColorAttributes(Color.FromArgb(e.Params));
                                  break;
                              case StyleType.size:
                                  UpdateFontSizeSpanAttributes(int.Parse(e.Params));

                                  break;
                              default:
                                  break;
                          }

                          platformView.AttributedText = AttributedText;
                          Focus();

                          if (CurrentFont != null)
                          {
                              var name = CurrentFont.Name;
                              CurrentTypingAttributes[UIStringAttributeKey.Font] = CurrentFont;
                              if (ChangeUnderline)
                              {
                                  if (CurrentUnderline)
                                  {
                                      CurrentTypingAttributes[UIStringAttributeKey.UnderlineStyle] = (NSNumber)1;
                                  }
                                  else
                                  {
                                      CurrentTypingAttributes[UIStringAttributeKey.UnderlineStyle] = (NSNumber)0;
                                  }
                                  ChangeUnderline = false;
                              }
                              platformView.TypingAttributes = CurrentTypingAttributes;
                          }



                      });


#endif
            }

        }

        private void WysiwygContentEditor_HandlerChanging(object sender, HandlerChangingEventArgs e)
        {
            if (e.OldHandler != null)
            {
                GetHtmlRequest =null;
                SetHtmlRequest =null;
                StyleChangeRequested =null;
            }
        }

        public event EventHandler GetHtmlRequest;
        public event EventHandler<string> SetHtmlRequest;
        public event EventHandler<StyleArgs> StyleChangeRequested;
        public event EventHandler<SelectionArgs> SelectionChangeHandler;

        public string HtmlString { get; set; }

        public void SetSelection(int start, int end)
        {
            var args = new SelectionArgs(start, end);
            SelectionChangeHandler(this, args);
        }

        public void SetHtmlText(string htmlString)
        {
            HtmlString = htmlString;
            SetHtmlRequest(this, htmlString);
        }



        public string GetHtmlText()
        {

            GetHtmlRequest(this, new EventArgs());
            return HtmlString;
        }

        public virtual void BoldChanged()
        {
            StyleChangeRequested(this, new StyleArgs(StyleType.bold));
        }

        public virtual void ItalicChanged()
        {
            StyleChangeRequested(this, new StyleArgs(StyleType.italic));
        }

        public virtual void UnderlineChanged()
        {
            StyleChangeRequested(this, new StyleArgs(StyleType.underline));
        }

        public virtual void ColorChanged(string color)
        {
            StyleChangeRequested(this, new StyleArgs(StyleType.foregroundColor, color));
        }
        public virtual void TextSizeChanged(string size)
        {
            StyleChangeRequested(this, new StyleArgs(StyleType.size, size));
        }
    }
}
