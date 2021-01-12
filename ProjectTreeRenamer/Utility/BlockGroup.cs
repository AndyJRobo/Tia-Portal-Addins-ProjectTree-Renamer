using Siemens.Engineering.Compiler;
using Siemens.Engineering.SW.Blocks;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ProjectTreeRenamer.Utility
{
    public class BlockGroup
    {
        private PlcBlockUserGroup _blockGroup;
        private PlcBlockUserGroup _newGroup;
        private ExitState _state;
        private string _path;
        private string _exportDir;
        private string _exportPath;

        public string Name { get; private set; }

        public bool IsChangeable
        {
            get { return _state == ExitState.IsChangeable; }
        }

        public List<Block> Blocks { get; private set; }
        public List<BlockGroup> Groups { get; private set; }
        public PlcBlockGroup Parent { get; set; }
        public bool NeedsUpdate { get; set; }

        public BlockGroup(PlcBlockUserGroup plcBlockGroup, string exportLocation, string path = "")
        {
            _blockGroup = plcBlockGroup;
            Parent = (PlcBlockGroup)_blockGroup.Parent;
            Name = _blockGroup.Name;
            _exportPath = Path.Combine(exportLocation, Util.RemoveInvalidFileNameChars(Name));
            var dir = Directory.CreateDirectory(_exportPath);
            _exportDir = dir.FullName;
            Inititalise(path);
            SetChangeableState();
        }

        private void Inititalise(string path)
        {
            if (path == "")
                _path = Name;
            else
                _path = path + " --- " + Name;
            Blocks = new List<Block>();
            foreach (PlcBlock block in _blockGroup.Blocks)
            {
                Blocks.Add(new Block(block));
            }
            Groups = new List<BlockGroup>();
            foreach (PlcBlockUserGroup blockGroup in _blockGroup.Groups)
            {
                Groups.Add(new BlockGroup(blockGroup, _exportPath, _path));
            }
        }

        private void SetChangeableState()
        {
            bool changeable = true;
            foreach (BlockGroup group in Groups)
            {
                changeable = changeable & group.IsChangeable;
            }
            foreach (Block block in Blocks)
            {
                changeable = changeable & block.IsChangeable;
                if (!block.IsChangeable)
                {
                    Trace.TraceInformation(block.Name + " Block Changeable: " + block.IsChangeable);
                }
            }

            if (changeable)
                _state = ExitState.IsChangeable;
            else
                _state = ExitState.CouldNotCompile;
        }

        public void RenameAll(string find, string replace)
        {
            if (Export())
            {
                //DeleteBlockGroup();
                //CreateRenamedGroups(find, replace);
                //ImportBlocks();
            }
            else
            {
                throw new System.Exception("Failed to Export");
            }
        }

        public bool Export()
        {
            bool success = true;
            foreach (Block block in Blocks)
            {
                success = success & block.Export(_exportDir);
                if (success == false)
                    return false;
            }
            foreach (BlockGroup group in Groups)
            {
                success = success & group.Export();
                if (success == false)
                    return false;
            }
            return success;
        }

        public void DeleteBlockGroup()
        {
            _blockGroup.Delete();
        }

        public void CreateRenamedGroups(string find, string replace)
        {
            string newGroupName = Name.Replace(find, replace);
            _newGroup = Parent.Groups.Create(newGroupName);
            foreach (BlockGroup group in Groups)
            {
                group.Parent = _newGroup;
                group.CreateRenamedGroups(find, replace);
            }
        }

        public void ImportBlocks()
        {
            foreach (BlockGroup group in Groups)
            {
                group.ImportBlocks();
            }
            foreach (Block block in Blocks)
            {
                block.Import(_newGroup, _exportDir);
            }
        }

        public void RefreshGroup()
        {
            _blockGroup = _newGroup;
            Blocks = new List<Block>();
            foreach (PlcBlock block in _blockGroup.Blocks)
            {
                Blocks.Add(new Block(block));
            }
            Groups = new List<BlockGroup>();
            foreach (PlcBlockUserGroup blockGroup in _blockGroup.Groups)
            {
                Groups.Add(new BlockGroup(blockGroup, _path));
            }
        }

        internal void Compile()
        {
            _newGroup.GetService<ICompilable>().Compile();
        }

        public void Rename(string find, string replace)
        {
            foreach (Block block in Blocks)
            {
                block.Rename(find, replace);
            }
            foreach (BlockGroup group in Groups)
            {
                group.Rename(find, replace);
            }
        }

        internal string GetIschangeableInfo()
        {
            string returnString = "";
            if (!IsChangeable)
            {
                string blockInfoString = GetBlockInfoStrings();
                if (blockInfoString != "")
                    returnString += _path + blockInfoString;
                foreach (BlockGroup group in Groups)
                {
                    returnString += System.Environment.NewLine + group.GetIschangeableInfo();
                    //if (groupInfoString != "")
                    //    returnString += " --- " + groupInfoString;
                }
            }
            return returnString;
        }

        internal string GetBlockInfoStrings()
        {
            string returnString = "";
            foreach (Block block in Blocks)
            {
                string blockInfoString = block.GetIschangeableInfo();
                if (blockInfoString != "")
                    returnString += System.Environment.NewLine + "" + blockInfoString;
            }
            return returnString;
        }
    }
}