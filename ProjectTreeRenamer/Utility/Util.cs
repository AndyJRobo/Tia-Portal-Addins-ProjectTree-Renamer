using Siemens.Engineering;
using Siemens.Engineering.SW.Blocks;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public static DirectoryInfo CreateUniqueDirectory()
        {
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), AppDomain.CurrentDomain.FriendlyName, $@"{Guid.NewGuid()}");
            return Directory.CreateDirectory(path);
        }

        public static bool ExportBlock(PlcBlock block, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                block.Export(new FileInfo(filePath), ExportOptions.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception during export:" + Environment.NewLine + ex + Environment.NewLine + "Block Name: " + block.Name + Environment.NewLine + " Path: " + filePath);
                return false;
            }
            return true;
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