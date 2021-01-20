using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.SW.Blocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProjectTreeRenamer.Utility
{
    public class BlockGroup
    {
        private PlcBlockUserGroup _blockGroup;
        private DirectoryInfo _parentDirectory;
        private DirectoryInfo _exportDirectory;
        private PlcBlockUserGroup _newGroup;

        public List<Block> Blocks { get; private set; }

        public List<BlockGroup> Groups { get; private set; }

        public string Name { get; private set; }

        //public bool IsChangeable => _state == ExitState.IsChangeable;
        //public bool NeedsUpdate { get; set; }
        public PlcBlockGroup Parent { get; set; }

        public BlockGroup(PlcBlockUserGroup plcBlockGroup, DirectoryInfo parentDirectory)
        {
            _blockGroup = plcBlockGroup;
            _parentDirectory = parentDirectory;
            Inititalise();
        }

        private void Inititalise()
        {
            Blocks = new List<Block>();
            Groups = new List<BlockGroup>();

            Parent = (PlcBlockGroup)_blockGroup.Parent;
            Name = _blockGroup.Name;

            CreateDirectory();

            foreach (PlcBlock block in _blockGroup.Blocks)
                Blocks.Add(new Block(block));

            foreach (PlcBlockUserGroup blockGroup in _blockGroup.Groups)
                Groups.Add(new BlockGroup(blockGroup, _exportDirectory));
        }

        private void CreateDirectory()
        {
            string path = Path.Combine(_parentDirectory.FullName, Util.RemoveInvalidFileNameChars(Name));
            _exportDirectory = Directory.CreateDirectory(path);
        }

        internal void Compile(ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Compiling: " + Name;
            _newGroup.GetService<ICompilable>().Compile();
        }

        public void Export(ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Exporting: " + Name;
            foreach (Block block in Blocks)
                block.Export(_exportDirectory.FullName, exclusiveAccess);
            foreach (BlockGroup group in Groups)
                group.Export(exclusiveAccess);
        }

        public void DeleteBlockGroup(ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Deleting: " + Name;
            _blockGroup.Delete();
        }

        public void CreateRenamedGroups(string find, string replace, ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Renaming: " + Name;
            string newGroupName = Name.Replace(find, replace);
            _newGroup = Parent.Groups.Create(newGroupName);
            foreach (BlockGroup group in Groups)
            {
                group.Parent = _newGroup;
                group.CreateRenamedGroups(find, replace, exclusiveAccess);
            }
        }

        public void ImportBlocks(ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Importing: " + Name;
            foreach (BlockGroup group in Groups)
                group.ImportBlocks(exclusiveAccess);
            foreach (Block block in Blocks)
                block.Import(_newGroup, exclusiveAccess);
        }

        public void Rename(string find, string replace, ExclusiveAccess exclusiveAccess)
        {
            foreach (Block block in Blocks)
                block.Rename(find, replace, exclusiveAccess);
            foreach (BlockGroup group in Groups)
                group.Rename(find, replace, exclusiveAccess);
        }

        public void RefreshGroup()
        {
            _blockGroup = _newGroup;
            Blocks = new List<Block>();
            Groups = new List<BlockGroup>();
            foreach (PlcBlock block in _blockGroup.Blocks)
                Blocks.Add(new Block(block));

            foreach (PlcBlockUserGroup blockGroup in _blockGroup.Groups)
                Groups.Add(new BlockGroup(blockGroup, _exportDirectory));
        }

        //public void RenameAll(string find, string replace)
        //{
        //    Export();
        //    //DeleteBlockGroup();
        //    //CreateRenamedGroups(find, replace);
        //    //ImportBlocks();
        //}

        //private void SetChangeableState()
        //{
        //    bool changeable = true;
        //    foreach (BlockGroup group in Groups)
        //    {
        //        changeable = changeable & group.IsChangeable;
        //    }
        //    foreach (Block block in Blocks)
        //    {
        //        changeable = changeable & block.IsChangeable;
        //        if (!block.IsChangeable)
        //        {
        //            Trace.TraceInformation(block.Name + " Block Changeable: " + block.IsChangeable);
        //        }
        //    }

        //    if (changeable)
        //        _state = ExitState.IsChangeable;
        //    else
        //        _state = ExitState.CouldNotCompile;
        //}

        //internal string GetIschangeableInfo()
        //{
        //    string returnString = "";
        //    if (!IsChangeable)
        //    {
        //        string blockInfoString = GetBlockInfoStrings();
        //        if (blockInfoString != "")
        //            returnString += _path + blockInfoString;
        //        foreach (BlockGroup group in Groups)
        //        {
        //            returnString += System.Environment.NewLine + group.GetIschangeableInfo();
        //            //if (groupInfoString != "")
        //            //    returnString += " --- " + groupInfoString;
        //        }
        //    }
        //    return returnString;
        //}

        //internal string GetBlockInfoStrings()
        //{
        //    string returnString = "";
        //    foreach (Block block in Blocks)
        //    {
        //        string blockInfoString = block.GetIschangeableInfo();
        //        if (blockInfoString != "")
        //            returnString += System.Environment.NewLine + "" + blockInfoString;
        //    }
        //    return returnString;
        //}
    }
}