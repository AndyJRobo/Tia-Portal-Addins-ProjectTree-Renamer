using Siemens.Engineering;
using Siemens.Engineering.SW.Blocks;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace ProjectTreeRenamer.Utility
{
    internal static class Util
    {
        public static string RemoveInvalidFileNameChars(string name)
        {
            Path.GetInvalidFileNameChars().ToList().ForEach(c => name = name.Replace(c.ToString(), "_"));
            return name;
        }

        public static bool ExportBlock(PlcBlock block, string filePath)
        {
            try
            {
                Trace.TraceInformation("Path :" + filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                block.Export(new FileInfo(filePath), ExportOptions.None);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception during export:" + Environment.NewLine + ex);
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
