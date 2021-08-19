using ProjectTreeRenamer.Utility;
using Siemens.Engineering;
using Siemens.Engineering.SW.Blocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectTreeRenamer.Visitors
{
    public class ExportVisitor : IVisitor
    {
        public ExportVisitor(ExclusiveAccess exclusiveAccess, DirectoryInfo directoryInfo, ExportOptions exportOptions)
        {
            ExclusiveAccess = exclusiveAccess;
            DirectoryInfo = directoryInfo;
            ExportOptions = exportOptions;
            ExcludeSubfolders = false;
        }

        public ExportVisitor(ExclusiveAccess exclusiveAccess, DirectoryInfo directoryInfo, ExportOptions exportOptions, bool excludeSubfolders)
        {
            ExclusiveAccess = exclusiveAccess;
            DirectoryInfo = directoryInfo;
            ExportOptions = exportOptions;
            ExcludeSubfolders = excludeSubfolders;
        }

        public ExclusiveAccess ExclusiveAccess { get; }
        public DirectoryInfo DirectoryInfo { get; }
        public ExportOptions ExportOptions { get; }
        public bool ExcludeSubfolders { get; }

        public void Visit(PlcBlockUserGroupProxy blockGroup)
        {
            List<PlcBlockProxy> Blocks = blockGroup.Blocks;
            List<PlcBlockUserGroupProxy> Groups = blockGroup.Groups;
            ExclusiveAccess.Text = "Exporting: " + blockGroup.Name;

            Blocks.ForEach(b => this.Visit(b));
            Groups.ForEach(g => this.Visit(g));
        }

        public void Visit(PlcBlockProxy block)
        {
            block.FileInfo = new FileInfo(DirectoryInfo.FullName + "\\" + block.getPlcBlockName() + ".xml");
            if (File.Exists(block.FileInfo.FullName))
            {
                File.Delete(block.FileInfo.FullName);
            }
            block.ExportPlcBlock(ExportOptions);
        }
    }
}