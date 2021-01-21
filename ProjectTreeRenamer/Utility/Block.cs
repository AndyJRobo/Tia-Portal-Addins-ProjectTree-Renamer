using Siemens.Engineering;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using System.IO;

namespace ProjectTreeRenamer.Utility
{
    public class Block
    {
        private PlcBlock _block;
        private string _path;

        public Block(PlcBlock plcBlock)
        {
            _block = plcBlock;
            Name = plcBlock.Name;
        }

        public string Name { get; private set; }

        public void Export(string path, ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Exporting: " + Name;
            _path = Path.Combine(path, Util.RemoveInvalidFileNameChars(Name) + ".xml");
            if (File.Exists(_path))
                File.Delete(_path);
            _block.Export(new FileInfo(_path), ExportOptions.None);
        }

        public void Import(PlcBlockUserGroup Group, ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Importing: " + Name;
            Group.Blocks.Import(new FileInfo(_path), ImportOptions.Override, SWImportOptions.IgnoreMissingReferencedObjects | SWImportOptions.IgnoreStructuralChanges | SWImportOptions.IgnoreUnitAttributes);

            if (File.Exists(_path))
                File.Delete(_path);
        }

        public void Rename(string find, string replace, ExclusiveAccess exclusiveAccess)
        {
            exclusiveAccess.Text = "Renaming: " + Name;
            _block.Name = _block.Name.Replace(find, replace);
            Name = _block.Name;
        }
    }
}