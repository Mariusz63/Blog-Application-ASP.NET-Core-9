namespace SyncSyntax.Helpers
{
    public class RemoveHtmlTagHelper
    {
        public static string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            // Use a regular expression to remove HTML tags
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>| &.*?;", string.Empty);
        }
    }
}
