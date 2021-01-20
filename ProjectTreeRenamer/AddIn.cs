using ProjectTreeRenamer.Utility;
using Siemens.Engineering;
using Siemens.Engineering.AddIn.Menu;
using Siemens.Engineering.HW;
using Siemens.Engineering.SW.Blocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ProjectTreeRenamer
{
    public class AddIn : ContextMenuAddIn
    {
        private readonly TiaPortal _tiaPortal;
        private readonly Settings _settings;
        private string _Find;
        private string _Replace;
        private readonly string _traceFilePath;

        public AddIn(TiaPortal tiaPortal) : base("ProjectTree Renamer")
        {
            _tiaPortal = tiaPortal;
            _settings = Settings.Load();
            _Find = "";
            _Replace = "";

            var assemblyName = Assembly.GetCallingAssembly().GetName();
            var logDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TIA Add-Ins", assemblyName.Name, assemblyName.Version.ToString(), "Logs");
            var logDirectory = Directory.CreateDirectory(logDirectoryPath);
            _traceFilePath = Path.Combine(logDirectory.FullName, string.Concat(DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".txt"));
        }

        protected override void BuildContextMenuItems(ContextMenuAddInRoot addInRootSubmenu)
        {
            addInRootSubmenu.Items.AddActionItem<PlcBlockUserGroup>("RenameAll", RenameAllSelectedPlcBlockGroups);
            addInRootSubmenu.Items.AddActionItem<PlcBlock>("RenameAll", RenameAllSelectedPlcBlocks);
            addInRootSubmenu.Items.AddActionItem<DeviceUserGroup>("RenameAll", RenameAllSelectedDeviceGroups);
            addInRootSubmenu.Items.AddActionItem<Device>("RenameAll", RenameAllSelectedDevices);
        }

        private void RenameAllSelectedPlcBlockGroups(MenuSelectionProvider<PlcBlockUserGroup> menuSelectionProvider)
        {
            using (var fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;

                bool replaceClicked = false;
                using (Form owner = Util.GetForegroundWindow())
                {
                    DialogResult result = ShowMyDialogBox(owner);
                    if (result == DialogResult.OK)
                        replaceClicked = true;
                }

                if (replaceClicked & !_Find.Equals(_Replace))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    using (var exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming  folder and contents from: " + _Find + " to: " + _Replace))
                    {
                        bool TransactionSuccess = false;
                        var project = _tiaPortal.Projects.First();
                        var menuSelection = menuSelectionProvider.GetSelection();

                        var exportDirInfo = Util.CreateUniqueDirectory();

                        List<BlockGroup> blockGroups = new List<BlockGroup>();

                        using (var transaction = exclusiveAccess.Transaction(project, "Renaming folders from: " + _Find + " to: " + _Replace))
                        {
                            try
                            {
                                foreach (PlcBlockUserGroup PlcBlockUserGroup in menuSelection)
                                    blockGroups.Add(new BlockGroup(PlcBlockUserGroup, exportDirInfo));

                                foreach (BlockGroup blockGroup in blockGroups)
                                    blockGroup.Export(exclusiveAccess);

                                foreach (BlockGroup blockGroup in blockGroups)
                                    blockGroup.DeleteBlockGroup(exclusiveAccess);

                                foreach (BlockGroup blockGroup in blockGroups)
                                    blockGroup.CreateRenamedGroups(_Find, _Replace, exclusiveAccess);

                                foreach (BlockGroup blockGroup in blockGroups)
                                    blockGroup.ImportBlocks(exclusiveAccess);
                            }
                            catch (Exception ex)
                            {
                                string failText = "Exception during rename:" + Environment.NewLine + ex + Environment.NewLine + ex.TargetSite;
                                Trace.TraceError(failText);
                                using (Form owner = Util.GetForegroundWindow())
                                {
                                    MessageBox.Show(owner, failText, "Excpetion Occured Duuring Rename Operation");
                                }
                                return;
                            }
                            if (transaction.CanCommit)
                            {
                                TransactionSuccess = true;
                                transaction.CommitOnDispose();
                            }
                        }
                        if (TransactionSuccess)
                        {
                            foreach (BlockGroup blockGroup in blockGroups)
                            {
                                blockGroup.Compile(exclusiveAccess);
                            }
                            using (var transaction = exclusiveAccess.Transaction(project, "Renaming blocks from: " + _Find + " to: " + _Replace))
                            {
                                try
                                {
                                    foreach (BlockGroup blockGroup in blockGroups)
                                    {
                                        blockGroup.RefreshGroup();
                                        blockGroup.Rename(_Find, _Replace, exclusiveAccess);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string failText = "Exception during rename:" + Environment.NewLine + ex + Environment.NewLine + ex.TargetSite;
                                    Trace.TraceError(failText);
                                    using (Form owner = Util.GetForegroundWindow())
                                    {
                                        MessageBox.Show(owner, failText, "Excpetion Occured Duuring Rename Operation");
                                    }
                                    return;
                                }
                                if (transaction.CanCommit)
                                {
                                    transaction.CommitOnDispose();
                                }
                            }
                        }
                    }
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                          ts.Hours, ts.Minutes, ts.Seconds,
                          ts.Milliseconds / 10);
                    Trace.TraceInformation("RunTime " + elapsedTime);
                    using (Form owner = Util.GetForegroundWindow())
                    {
                        MessageBox.Show(owner, "Completed Renaming in " + elapsedTime);
                    }
                }
                Trace.Close();
            }
            DeleteEmptyTraceFile();
        }

        private void RenameBlockGroup(BlockGroup blockGroup)
        {
            //if (blockGroup.IsChangeable)
            //{
            //    Trace.TraceInformation("Exporting: " + blockGroup.Name + System.Environment.NewLine);
            //    blockGroup.RenameAll(_Find, _Replace);
            //}
            //else
            //{
            //    //string diagnostics = "The block group " + blockGroup.Name + " has some blocks that were not changeable..." + blockGroup.GetIschangeableInfo();
            //    //using (Form owner = Util.GetForegroundWindow())
            //    //{
            //    //    MessageBox.Show(owner, diagnostics);
            //    //}
            //    //Trace.TraceInformation(diagnostics);
            //}
        }

        private void RenameAllSelectedPlcBlocks(MenuSelectionProvider menuSelectionProvider)
        {
            IEnumerable<object> menuSelction = menuSelectionProvider.GetSelection();
            using (var fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;

                using (var exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming blocks..."))
                {
                    var project = _tiaPortal.Projects.First();
                    using (var transaction = exclusiveAccess.Transaction(project, "Renaming blocks"))
                    {
                        bool replaceClicked = false;
                        using (Form owner = Util.GetForegroundWindow())
                        {
                            DialogResult result = ShowMyDialogBox(owner);
                            if (result == DialogResult.OK)
                                replaceClicked = true;
                        }
                        if (replaceClicked & !_Find.Equals(_Replace))
                        {
                            foreach (PlcBlock block in menuSelction)
                            {
                                block.Name = block.Name.Replace(_Find, _Replace);
                            }
                        }
                        using (Form owner = Util.GetForegroundWindow())
                        {
                            MessageBox.Show(owner, "Completed Renaming");
                        }
                        if (transaction.CanCommit)
                        {
                            transaction.CommitOnDispose();
                        }
                    }
                }
                Trace.Close();
            }
            DeleteEmptyTraceFile();
        }

        private void RenameAllSelectedDeviceGroups(MenuSelectionProvider menuSelectionProvider)
        {
            IEnumerable<object> menuSelction = menuSelectionProvider.GetSelection();
            using (var fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;
                using (var exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming blocks..."))
                {
                    var project = _tiaPortal.Projects.First();
                    using (var transaction = exclusiveAccess.Transaction(project, "Renaming blocks"))
                    {
                        bool replaceClicked = false;
                        using (Form owner = Util.GetForegroundWindow())
                        {
                            if (ShowMyDialogBox(owner) == DialogResult.OK)
                                replaceClicked = true;
                        }
                        if (replaceClicked & !_Find.Equals(_Replace))
                        {
                            foreach (DeviceUserGroup deviceGroup in menuSelction)
                            {
                                UDeviceGroup uDeviceGroup = new UDeviceGroup(deviceGroup);
                                uDeviceGroup.Rename(_Find, _Replace);
                            }
                        }
                        using (Form owner = Util.GetForegroundWindow())
                        {
                            MessageBox.Show(owner, "Completed Renaming");
                        }
                        if (transaction.CanCommit)
                        {
                            transaction.CommitOnDispose();
                        }
                    }
                }
                Trace.Close();
            }
            DeleteEmptyTraceFile();
        }

        private void RenameAllSelectedDevices(MenuSelectionProvider menuSelectionProvider)
        {
            IEnumerable<object> menuSelction = menuSelectionProvider.GetSelection();
            using (var fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;
                using (var exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming blocks..."))
                {
                    var project = _tiaPortal.Projects.First();
                    using (var transaction = exclusiveAccess.Transaction(project, "Renaming blocks"))
                    {
                        List<Utility.UDeviceGroup> deviceGroups = new List<Utility.UDeviceGroup>();
                        bool replaceClicked = false;
                        using (Form owner = Util.GetForegroundWindow())
                        {
                            if (ShowMyDialogBox(owner) == DialogResult.OK)
                                replaceClicked = true;
                        }
                        if (replaceClicked & !_Find.Equals(_Replace))
                        {
                            foreach (Device device in menuSelction)
                            {
                                device.Name = device.Name.Replace(_Find, _Replace);
                                foreach (DeviceItem deviceItem in device.DeviceItems)
                                {
                                    deviceItem.Name = deviceItem.Name.Replace(_Find, _Replace);
                                }
                            }
                        }
                        using (Form owner = Util.GetForegroundWindow())
                        {
                            MessageBox.Show(owner, "Completed Renaming");
                        }
                        if (transaction.CanCommit)
                        {
                            transaction.CommitOnDispose();
                        }
                    }
                }
                Trace.Close();
            }
            DeleteEmptyTraceFile();
        }

        private static IEnumerable<DeviceUserGroup> EnumerateDeviceUserGroup(DeviceUserGroup deviceUserGroup)
        {
            yield return deviceUserGroup;

            foreach (DeviceUserGroup subDeviceUserGroup in deviceUserGroup.Groups)
            {
                foreach (DeviceUserGroup Group in EnumerateDeviceUserGroup(subDeviceUserGroup))
                {
                    yield return Group;
                }
            }
        }

        public DialogResult ShowMyDialogBox(Form owner)
        {
            DialogResult result = DialogResult.None;
            RenameForm testDialog = new RenameForm();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog(owner) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                _Find = testDialog.textBox_Find.Text;
                _Replace = testDialog.textBox_Replace.Text;
                result = DialogResult.OK;
            }
            testDialog.Dispose();
            return result;
        }

        private void DeleteEmptyTraceFile()
        {
            try
            {
                if (new FileInfo(_traceFilePath).Length == 0)
                {
                    File.Delete(_traceFilePath);
                }
            }
            catch
            {
                // Silently ignore file operations
            }
        }
    }
}