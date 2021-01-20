using Siemens.Engineering;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using System.IO;

namespace ProjectTreeRenamer.Utility
{
    public class Block
    {
        private PlcBlock _block;
        private PlcBlockGroup _parent;
        private string _path;
        private ExitState _state;

        public Block(PlcBlock plcBlock)
        {
            _block = plcBlock;
            //_parent = (PlcBlockGroup)_block.Parent;
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

        //private void SetChangeableState()
        //{
        //    if (_block.IsKnowHowProtected)
        //    {
        //        _state = ExitState.BlockIsKnowHowProtected;
        //        Trace.TraceInformation(Name + " BlockIsKnowHowProtected");
        //        return;
        //    }

        //    if (!_block.IsConsistent)
        //    {
        //        try
        //        {
        //            if (_block.GetService<ICompilable>().Compile().State == CompilerResultState.Error)
        //            {
        //                _state = ExitState.CouldNotCompile;
        //                Trace.TraceInformation(Name + " CouldNotCompile");
        //                return;
        //            }
        //        }
        //        catch
        //        {
        //            _state = ExitState.CouldNotCompile;
        //            Trace.TraceInformation(Name + " CouldNotCompile - Catch");
        //            return;
        //        }
        //    }

        //    if (_block.GetService<LibraryTypeInstanceInfo>() != null)
        //    {
        //        _state = ExitState.IsLibraryType;
        //        Trace.TraceInformation(Name + " IsLibraryType");
        //        return;
        //    }

        //    _state = ExitState.IsChangeable;
        //}
        //public bool IsChangeable
        //{
        //    get { return _state == ExitState.IsChangeable; }
        //}
        //internal string GetIschangeableInfo()
        //{
        //    string returnString = "";
        //    if (!IsChangeable)
        //        returnString = Name + " could not be changed because: " + System.Environment.NewLine + "    " + GetStateText();
        //    return returnString;
        //}

        //public string GetActionText()
        //{
        //    switch (_state)
        //    {
        //        case ExitState.BlockIsKnowHowProtected:
        //            return "Remove the know-how protection.";

        //        case ExitState.ProgrammingLanguageNotSupported:
        //            return "Change the programming language of the block.";

        //        case ExitState.CouldNotCompile:
        //            return "Compile the block without errors.";

        //        case ExitState.CouldNotExport:
        //            return "Please report this issue for further investigation.";

        //        case ExitState.CouldNotImport:
        //            return "Please report this issue for further investigation.";

        //        case ExitState.IsChangeable:
        //            return "No action required.";

        //        case ExitState.XmlEditingError:
        //            return "Please report this issue for further investigation.";

        //        case ExitState.IsLibraryType:
        //            return "Terminate the library type connection.";

        //        case ExitState.Successful:
        //            return "No action required.";

        //        default:
        //            return "";
        //    }
        //}
        //internal void Compile()
        //{
        //    _block.GetService<ICompilable>().Compile();
        //}
        //public bool ChangeSuccessful
        //{
        //    get { return _state == ExitState.Successful; }
        //}

        //public string GetStateText()
        //{
        //    switch (_state)
        //    {
        //        case ExitState.BlockIsKnowHowProtected:
        //            return "The block is know-how protected.";

        //        case ExitState.ProgrammingLanguageNotSupported:
        //            return "The programming language of the block is not supported.";

        //        case ExitState.CouldNotCompile:
        //            return "The block could not be compiled.";

        //        case ExitState.CouldNotExport:
        //            return "The block could not be exported.";

        //        case ExitState.CouldNotImport:
        //            return "The block could not be imported.";

        //        case ExitState.IsChangeable:
        //            return "The block type is changeable.";

        //        case ExitState.XmlEditingError:
        //            return "Error during editing of SimaticML file";

        //        case ExitState.IsLibraryType:
        //            return "Library types are not supported.";

        //        case ExitState.Successful:
        //            return "The block type was changed successfully.";

        //        default:
        //            return "";
        //    }
        //}
    }
}