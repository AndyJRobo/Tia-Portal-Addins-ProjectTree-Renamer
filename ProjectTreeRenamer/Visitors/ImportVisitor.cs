using Siemens.Engineering;
using Siemens.Engineering.SW;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectTreeRenamer.Visitors
{
    public class ImportVisitor : IVisitor
    {
        public ImportVisitor(ExclusiveAccess exclusiveAccess, DirectoryInfo directoryInfo)
        {
            ExclusiveAccess = exclusiveAccess;
            DirectoryInfo = directoryInfo;
            ExcludeSubfolders = false;
        }

        public ImportVisitor(ExclusiveAccess exclusiveAccess, DirectoryInfo directoryInfo, bool excludeSubfolders)
        {
            ExclusiveAccess = exclusiveAccess;
            DirectoryInfo = directoryInfo;
            ExcludeSubfolders = excludeSubfolders;
        }

        public ExclusiveAccess ExclusiveAccess { get; }
        public DirectoryInfo DirectoryInfo { get; }
        public bool ExcludeSubfolders { get; }

        public void Visit(PlcBlockUserGroupProxy blockGroup)
        {
            ExclusiveAccess.Text = "Import - Visit BlockGroup";
            ImportDBs(blockGroup);
            ImportFCs(blockGroup);
            ImportFBs(blockGroup);
        }

        private void ImportFBs(PlcBlockUserGroupProxy blockGroup)
        {
            List<PlcBlockProxy> Blocks = blockGroup.Blocks;
            List<PlcBlockUserGroupProxy> Groups = blockGroup.Groups;

            foreach (PlcBlockUserGroupProxy group in Groups)
            {
                ImportFBs(group);
            }
            Blocks.Reverse();
            Blocks.Where(b => b.getPlcBlockType().Equals(typeof(Siemens.Engineering.SW.Blocks.FB))).ToList().ForEach(b => this.Visit(b));
        }

        private void ImportFCs(PlcBlockUserGroupProxy blockGroup)
        {
            List<PlcBlockProxy> Blocks = blockGroup.Blocks;
            List<PlcBlockUserGroupProxy> Groups = blockGroup.Groups;

            foreach (PlcBlockUserGroupProxy group in Groups)
            {
                ImportFCs(group);
            }
            Blocks.Where(b => b.getPlcBlockType().Equals(typeof(Siemens.Engineering.SW.Blocks.FC))).ToList().ForEach(b => this.Visit(b));
        }

        public void ImportDBs(PlcBlockUserGroupProxy blockGroup)
        {
            List<PlcBlockProxy> Blocks = blockGroup.Blocks;
            List<PlcBlockUserGroupProxy> Groups = blockGroup.Groups;
            foreach (PlcBlockUserGroupProxy group in Groups)
            {
                ImportDBs(group);
            }
            Blocks.Where(b => b.getPlcBlockType().Equals(typeof(Siemens.Engineering.SW.Blocks.InstanceDB))).ToList().ForEach(b => this.Visit(b));
            Blocks.Where(b => b.getPlcBlockType().Equals(typeof(Siemens.Engineering.SW.Blocks.GlobalDB))).ToList().ForEach(b => this.Visit(b));
        }

        public void Visit(PlcBlockProxy block)
        {
            ExclusiveAccess.Text = "Import - Visit Block, Parent: " + block.Parent.Name + " Path: " + block.FileInfo.FullName;
            block.Parent.PlcBlockGroup.Blocks.Import(block.FileInfo, ImportOptions.Override, SWImportOptions.IgnoreMissingReferencedObjects);//| SWImportOptions.IgnoreStructuralChanges | SWImportOptions.IgnoreUnitAttributes
        }
    }
}