using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectTreeRenamer.Utility
{
    internal static class Util
    {
        public static string RemoveInvalidFileNameChars(string name)
        {
            Path.GetInvalidFileNameChars().ToList().ForEach(c => name = name.Replace(c.ToString(), "_"));
            return name;
        }

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

        public static DirectoryInfo CreateUniqueDirectory()
        {
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), AppDomain.CurrentDomain.FriendlyName, $@"{Guid.NewGuid()}");
            return Directory.CreateDirectory(path);
        }

        internal static Form GetForegroundWindow()
        {
            // Workaround for Add-In Windows to be shown in foreground of TIA Portal
            Form form = new Form { Opacity = 0, ShowIcon = false };
            form.Show();
            form.TopMost = true;
            form.Activate();
            form.TopMost = false;
            return form;
        }
    }
}