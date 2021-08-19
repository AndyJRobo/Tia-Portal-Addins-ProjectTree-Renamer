using ProjectTreeRenamer.Visitors;
using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.SW.Blocks;
using System.Collections.Generic;

namespace ProjectTreeRenamer
{
    public class PlcBlockUserGroupProxy : IElement
    {
        public List<PlcBlockProxy> Blocks { get; private set; }
        public List<PlcBlockUserGroupProxy> Groups { get; private set; }
        public PlcBlockGroup Parent { get; set; }
        public PlcBlockUserGroup PlcBlockGroup { get; private set; }
        public bool F_Block { get; private set; }
        public string Name { get; private set; }

        public PlcBlockUserGroupProxy(PlcBlockUserGroup plcBlockGroup)
        {
            PlcBlockGroup = plcBlockGroup;
            Name = PlcBlockGroup.Name;
            Parent = (PlcBlockGroup)PlcBlockGroup.Parent;
            Blocks = new List<PlcBlockProxy>();
            Groups = new List<PlcBlockUserGroupProxy>();
            foreach (PlcBlock block in PlcBlockGroup.Blocks)
            {
                ProgrammingLanguage programmingLanguage = block.ProgrammingLanguage;
                if (programmingLanguage == ProgrammingLanguage.F_CALL | programmingLanguage == ProgrammingLanguage.F_DB | programmingLanguage == ProgrammingLanguage.F_FBD
                    | programmingLanguage == ProgrammingLanguage.F_FBD_LIB | programmingLanguage == ProgrammingLanguage.F_LAD | programmingLanguage == ProgrammingLanguage.F_LAD_LIB
                    | programmingLanguage == ProgrammingLanguage.F_STL)
                {
                    F_Block = true;
                    return;
                }
                else
                    Blocks.Add(new PlcBlockProxy(block, this));
            }

            foreach (PlcBlockUserGroup blockGroup in PlcBlockGroup.Groups)
                Groups.Add(new PlcBlockUserGroupProxy(blockGroup));
        }

        public void Refresh()
        {
            Blocks.Clear();
            Groups.Clear();

            foreach (PlcBlock block in PlcBlockGroup.Blocks)
                Blocks.Add(new PlcBlockProxy(block, this));

            foreach (PlcBlockUserGroup blockGroup in PlcBlockGroup.Groups)
            {
                Groups.Add(new PlcBlockUserGroupProxy(blockGroup));
            }
        }

        public bool CheckFBlocks()
        {
            if (F_Block)
                return true;
            foreach (PlcBlockUserGroupProxy group in Groups)
            {
                if (group.F_Block)
                    return true;
            }
            return false;
        }

        public void UpdatePlcBlockGroup(PlcBlockUserGroup newGroup)
        {
            PlcBlockGroup = newGroup;
            Name = PlcBlockGroup.Name;
        }

        internal void Compile(ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Compiling: " + PlcBlockGroup.Name;
            PlcBlockGroup.GetService<ICompilable>().Compile();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            string ToStringString = "";
            ToStringString += "Blocks: ";
            ToStringString += Blocks.Count;
            ToStringString += System.Environment.NewLine;
            ToStringString += "Groups: " + Groups.Count;
            ToStringString += System.Environment.NewLine;
            if (Parent != null)
                ToStringString += "Parent: " + Parent.Name;
            else
                ToStringString += "Parent: null";
            ToStringString += System.Environment.NewLine;
            if (Parent != null)
                ToStringString += "PlcBlockGroup: " + PlcBlockGroup.Name;
            else
                ToStringString += "PlcBlockGroup: null";

            return ToStringString;
        }

        //public void DisplayBlockInformation(ExclusiveAccess exclusiveAccess)
        //{
        //    foreach (BlockProxy block in Blocks)
        //        block.GetBlockInformation();
        //}

        //public void RenameBlockContents(string find, string replace, ExclusiveAccess exclusiveAccess)
        //{
        //    foreach (BlockProxy block in Blocks)
        //        block.RenameBlockContents(find, replace, exclusiveAccess);
        //}

        //public void DeleteBlockGroup(ExclusiveAccess exclusiveAccess)
        //{
        //    exclusiveAccess.Text = "Deleting: " + Name;
        //    PlcBlockGroup.Delete();
        //}

        //public void CreateRenamedGroups(string find, string replace, ExclusiveAccess exclusiveAccess)
        //{
        //    exclusiveAccess.Text = "Renaming: " + Name;
        //    string newGroupName = Name.Replace(find, replace);
        //    _newGroup = Parent.Groups.Create(newGroupName);
        //    foreach (BloPlcBlockUserGroupProxy group in Groups)
        //    {
        //        group.Parent = _newGroup;
        //        group.CreateRenamedGroups(find, replace, exclusiveAccess);
        //    }
        //}

        //public void ImportBlocks(ExclusiveAccess exclusiveAccess)
        //{
        //}

        //public void RefreshGroup()
        //{
        //    PlcBlockGroup = _newGroup;
        //    Blocks = new List<BlockProxy>();
        //    Groups = new List<BloPlcBlockUserGroupProxy>();
        //    foreach (PlcBlock block in PlcBlockGroup.Blocks)
        //        Blocks.Add(new BlockProxy(block));

        //    foreach (PlcBlockUserGroup blockGroup in PlcBlockGroup.Groups)
        //        Groups.Add(new BloPlcBlockUserGroupProxy(blockGroup, _exportDirectory));
        //}
    }
}