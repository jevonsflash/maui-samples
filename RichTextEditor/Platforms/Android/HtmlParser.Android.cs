using System;
using Android.Text;

namespace RichTextEditor.Controls
{
    public static class HtmlParser_Android
    {
        public static ISpanned HtmlToSpanned(string htmlString)
        {
            ISpanned spanned = Html.FromHtml(htmlString, FromHtmlOptions.ModeCompact);
            return spanned;
        }

        public static string SpannedToHtml(ISpanned spanned)
        {
            string htmlString = Html.ToHtml(spanned, ToHtmlOptions.ParagraphLinesIndividual);
            return htmlString;
        }
    }


}
