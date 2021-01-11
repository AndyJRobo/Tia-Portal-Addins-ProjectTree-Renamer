using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using Siemens.Engineering.Library.Types;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;

namespace ProjectTreeRenamer.Utility
{
    public class Block
    {
        private PlcBlock _block;
        private ExitState _state;
        private PlcBlockGroup _parent;

        public string Name { get; private set; }
        public PlcBlockGroup Parent { get; private set; }

        public bool IsChangeable
        {
            get { return _state == ExitState.IsChangeable; }
        }

        public bool ChangeSuccessful
        {
            get { return _state == ExitState.Successful; }
        }

        public Block(PlcBlock plcBlock)
        {
            _block = plcBlock;
            _parent = (PlcBlockGroup)_block.Parent;
            Name = plcBlock.Name;            
            SetChangeableState();
        }

        private void SetChangeableState()
        {
            if (_block.IsKnowHowProtected)
            {
                _state = ExitState.BlockIsKnowHowProtected;
                Trace.TraceInformation(Name + " BlockIsKnowHowProtected");
                return;
            }

            if (!_block.IsConsistent)
            {
                try
                {
                    if (_block.GetService<ICompilable>().Compile().State == CompilerResultState.Error)
                    {
                        _state = ExitState.CouldNotCompile;
                        Trace.TraceInformation(Name + " CouldNotCompile");
                        return;
                    }
                }
                catch
                {
                    _state = ExitState.CouldNotCompile;
                    Trace.TraceInformation(Name + " CouldNotCompile - Catch");
                    return;
                }
            }

            if (_block.GetService<LibraryTypeInstanceInfo>() != null)
            {
                _state = ExitState.IsLibraryType;
                Trace.TraceInformation(Name + " IsLibraryType");
                return;
            }

            _state = ExitState.IsChangeable;
        }

        public void Rename(string find, string replace)
        {
            _block.Name = _block.Name.Replace(find, replace);
            Name = _block.Name;
            //_block.GetService<ICompilable>().Compile();
        }

        public bool Export(string path)
        {
            bool success;
            var filePath = Path.Combine(path, Util.RemoveInvalidFileNameChars(Name) + ".xml");                
            if (Util.ExportBlock(_block, filePath) != true)
            {
                 _state = ExitState.CouldNotExport;
                return false;
            }
            return true;
        }

        public void Import(PlcBlockUserGroup Group, string path)
        {            
            var filePath = Path.Combine(path, Util.RemoveInvalidFileNameChars(Name) + ".xml");
            IList<PlcBlock> importedBlocks;
            try
            {
                importedBlocks = Group.Blocks.Import(new FileInfo(filePath), ImportOptions.Override, SWImportOptions.IgnoreMissingReferencedObjects | SWImportOptions.IgnoreStructuralChanges | SWImportOptions.IgnoreUnitAttributes);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception during import:" + Environment.NewLine + ex);
                _state = ExitState.CouldNotImport;
                return;
            }
            finally
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                    // Silently ignore file operations
                }
            }
        }

        internal void Compile()
        {
            _block.GetService<ICompilable>().Compile();
        }

        public string GetStateText()
        {
            switch (_state)
            {
                case ExitState.BlockIsKnowHowProtected:
                    return "The block is know-how protected.";
                case ExitState.ProgrammingLanguageNotSupported:
                    return "The programming language of the block is not supported.";
                case ExitState.CouldNotCompile:
                    return "The block could not be compiled.";
                case ExitState.CouldNotExport:
                    return "The block could not be exported.";
                case ExitState.CouldNotImport:
                    return "The block could not be imported.";
                case ExitState.IsChangeable:
                    return "The block type is changeable.";
                case ExitState.XmlEditingError:
                    return "Error during editing of SimaticML file";
                case ExitState.IsLibraryType:
                    return "Library types are not supported.";
                case ExitState.Successful:
                    return "The block type was changed successfully.";
                default:
                    return "";
            }
        }
        internal string GetIschangeableInfo()
        {
            string returnString = "";
            if (!IsChangeable)
                returnString = Name + " could not be changed because: " + System.Environment.NewLine + "    " + GetStateText();
            return returnString;
        }

        public string GetActionText()
        {
            switch (_state)
            {
                case ExitState.BlockIsKnowHowProtected:
                    return "Remove the know-how protection.";
                case ExitState.ProgrammingLanguageNotSupported:
                    return "Change the programming language of the block.";
                case ExitState.CouldNotCompile:
                    return "Compile the block without errors.";
                case ExitState.CouldNotExport:
                    return "Please report this issue for further investigation.";
                case ExitState.CouldNotImport:
                    return "Please report this issue for further investigation.";
                case ExitState.IsChangeable:
                    return "No action required.";
                case ExitState.XmlEditingError:
                    return "Please report this issue for further investigation.";
                case ExitState.IsLibraryType:
                    return "Terminate the library type connection.";
                case ExitState.Successful:
                    return "No action required.";
                default:
                    return "";
            }
        }

        public string toString()
        {
            return _block.Name;
        }
    }
}
