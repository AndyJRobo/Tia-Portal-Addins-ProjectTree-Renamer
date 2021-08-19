using System.Text.RegularExpressions;

namespace TiaOpennessHelper.Utils
{
    /// <summary>
    /// Contains functions tohelp with prerequisites for strings and file names
    /// </summary>
    public static class AdjustNames
    {
        /// <summary>
        /// remove all characters in the filename that are not allowed during export.
        /// </summary>
        /// <param name="fileName">string to check</param>
        /// <returns>new string without forbidden characters</returns>
        public static string AdjustFileName(string fileName)
        {
            return Regex.Replace(fileName, @"[\\/:*?""<>|]", string.Empty);
        }

        /// <summary>Replaces all special characters with their XML representation</summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>String</returns>
        public static string AdjustXmlStrings(string xmlString)
        {
            if (xmlString == null)
                return "";
            string ret = Regex.Replace(xmlString, "([&])(?!amp;|gt;|apos;|lt;)", "&amp;");
            ret = ret.Replace("\"", "&quot;");
            ret = ret.Replace("'", "&apos;");
            ret = ret.Replace("<", "&lt;");
            ret = ret.Replace(">", "&gt;");

            return ret;
        }
    }
}
