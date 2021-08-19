using ProjectTreeRenamer.Visitors;
using Siemens.Engineering;
using Siemens.Engineering.SW.Blocks;
using System.Collections.Generic;

namespace ProjectTreeRenamer.NewFolder1
{
    internal class RenameVisitor : IVisitor
    {
        public RenameVisitor(string find, string replace, ExportVisitor exportVisitor, ImportVisitor importVisitor, ExclusiveAccess exclusiveAccess)
        {
            Find = find;
            Replace = replace;
            ExportVisitor = exportVisitor;
            ImportVisitor = importVisitor;
            ExclusiveAccess = exclusiveAccess;
            ExcludeSubfolders = false;
        }

        public RenameVisitor(string find, string replace, ExportVisitor exportVisitor, ImportVisitor importVisitor, ExclusiveAccess exclusiveAccess, bool excludeSubfolders)
        {
            Find = find;
            Replace = replace;
            ExportVisitor = exportVisitor;
            ImportVisitor = importVisitor;
            ExclusiveAccess = exclusiveAccess;
            ExcludeSubfolders = excludeSubfolders;
        }

        public string Find { get; }
        public string Replace { get; }
        public ExportVisitor ExportVisitor { get; }
        public ImportVisitor ImportVisitor { get; }
        public ExclusiveAccess ExclusiveAccess { get; }
        public bool ExcludeSubfolders { get; }

        public void Visit(PlcBlockUserGroupProxy blockGroup)
        {
            //Export
            blockGroup.Accept(ExportVisitor);

            //Delete
            blockGroup.PlcBlockGroup.Delete();

            //Create
            CreateRenamedGroups(blockGroup);

            blockGroup.Accept(ImportVisitor);

            blockGroup.Refresh();
            blockGroup.Compile(ExclusiveAccess);

            RenameBlocks(blockGroup);
        }

        private void RenameBlocks(PlcBlockUserGroupProxy blockGroup)
        {
            if (blockGroup.F_Block == false)
            {
                List<PlcBlockProxy> Blocks = blockGroup.Blocks;
                List<PlcBlockUserGroupProxy> Groups = blockGroup.Groups;

                foreach (PlcBlockProxy block in Blocks)
                    this.Visit(block);
                if (!ExcludeSubfolders)
                    foreach (PlcBlockUserGroupProxy group in Groups)
                        RenameBlocks(group);
            }
        }

        private void CreateRenamedGroups(PlcBlockUserGroupProxy Group)
        {
            string newGroupName = "";
            //Create Group with new Name.

            newGroupName = Group.Name.Replace(Find, Replace);
            PlcBlockUserGroup newGroup = Group.Parent.Groups.Create(newGroupName);

            //Update Reference to actual group
            Group.UpdatePlcBlockGroup(newGroup);

            List<PlcBlockUserGroupProxy> Groups = Group.Groups;

            //Do it Again.
            foreach (PlcBlockUserGroupProxy group in Groups)
            {
                //Update Parent Reference of children.
                group.Parent = newGroup;
                if (!ExcludeSubfolders)
                    CreateRenamedGroups(group);
                else
                    ReCreateGroups(group);
            }
        }

        private void ReCreateGroups(PlcBlockUserGroupProxy Group)
        {
            PlcBlockUserGroup newGroup = Group.Parent.Groups.Create(Group.Name);
            Group.UpdatePlcBlockGroup(newGroup);
            List<PlcBlockUserGroupProxy> Groups = Group.Groups;
            foreach (PlcBlockUserGroupProxy group in Groups)
            {
                //Update Parent Reference of children.
                group.Parent = newGroup;
                ReCreateGroups(group);
            }
        }

        public void Visit(PlcBlockProxy block)
        {
            block.updatePlcBlockName(Find, Replace);
        }
    }
}