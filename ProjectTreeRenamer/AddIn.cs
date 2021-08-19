using ProjectTreeRenamer.NewFolder1;
using ProjectTreeRenamer.Utility;
using ProjectTreeRenamer.Visitors;
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
using System.Threading;
using System.Windows.Forms;

namespace ProjectTreeRenamer
{
    public class AddIn : ContextMenuAddIn
    {
        private readonly TiaPortal _tiaPortal;
        private readonly Settings _settings;
        private string _find;
        private string _replace;
        private readonly string _traceFilePath;
        private readonly string _defaultExportFolderPath = @"C:\Temp";
        private string _projectName = string.Empty;

        public PlcBlockProxy BlockProxy { get; private set; }

        public AddIn(TiaPortal tiaPortal) : base("ProjectTree Renamer")
        {
            _tiaPortal = tiaPortal;
            _settings = Settings.Load();
            _find = "";
            _replace = "";

            AssemblyName assemblyName = Assembly.GetCallingAssembly().GetName();
            string logDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TIA Add-Ins", assemblyName.Name, assemblyName.Version.ToString(), "Logs");
            DirectoryInfo logDirectory = Directory.CreateDirectory(logDirectoryPath);
            _traceFilePath = Path.Combine(logDirectory.FullName, string.Concat(DateTime.Now.ToString("yyyyMMdd_HHmmss"), ".txt"));            
        }

        protected override void BuildContextMenuItems(ContextMenuAddInRoot addInRootSubmenu)
        {
            addInRootSubmenu.Items.AddActionItem<PlcBlockUserGroup>("Rename All", RenameAllSelectedPlcBlockGroupsAndSubFolders);
            addInRootSubmenu.Items.AddActionItem<PlcBlockUserGroup>("Rename All - Excluding Subfolders", RenameAllSelectedPlcBlockGroups);
            addInRootSubmenu.Items.AddActionItem<PlcBlock>("Rename All", RenameAllSelectedPlcBlocks);
            addInRootSubmenu.Items.AddActionItem<DeviceUserGroup>("RenameAll", RenameAllSelectedDeviceGroups);
            addInRootSubmenu.Items.AddActionItem<Device>("RenameAll", RenameAllSelectedDevices);
        }

        private void RenameAllSelectedPlcBlockGroups(MenuSelectionProvider<PlcBlockUserGroup> menuSelectionProvider)
        {
            using (FileStream fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;

                bool replaceClicked = false;

                DialogResult result = ShowMyDialogBox();
                if (result == DialogResult.OK)
                    replaceClicked = true;

                if (replaceClicked & !_find.Equals(_replace))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    using (ExclusiveAccess exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming  folder and contents from: " + _find + " to: " + _replace))
                    {
                        Project project = _tiaPortal.Projects.First();
                        _projectName = project.Name;
                        IEnumerable<object> menuSelection = menuSelectionProvider.GetSelection();

                        DirectoryInfo exportDirInfo = Directory.CreateDirectory(Path.Combine(_defaultExportFolderPath, _projectName)); ;

                        List<PlcBlockUserGroupProxy> blockGroups = new List<PlcBlockUserGroupProxy>();

                        using (Transaction transaction = exclusiveAccess.Transaction(project, "Renaming folders from: " + _find + " to: " + _replace))
                        {
                            try
                            {
                                ExportVisitor exportVisitor = new ExportVisitor(exclusiveAccess, exportDirInfo, ExportOptions.None);
                                ImportVisitor importVisitor = new ImportVisitor(exclusiveAccess, exportDirInfo);
                                RenameVisitor renameVisitor = new RenameVisitor(_find, _replace, exportVisitor, importVisitor, exclusiveAccess, true);

                                foreach (PlcBlockUserGroup PlcBlockUserGroup in menuSelection)
                                    blockGroups.Add(new PlcBlockUserGroupProxy(PlcBlockUserGroup));

                                foreach (PlcBlockUserGroupProxy blockGroup in blockGroups)
                                {
                                    if (!blockGroup.CheckFBlocks())
                                        blockGroup.Accept(renameVisitor);
                                }
                            }
                            catch (EngineeringSecurityException engineeringSecurityException)
                            {
                                ShowMessage(engineeringSecurityException.ToString(), "EngineeringSecurityException");
                                //Console.WriteLine(engineeringSecurityException);
                            }
                            catch (EngineeringObjectDisposedException engineeringObjectDisposedException)
                            {
                                ShowMessage(engineeringObjectDisposedException.Message, "EngineeringObjectDisposedException");
                            }
                            catch (EngineeringNotSupportedException engineeringNotSupportedException)
                            {
                                string message = engineeringNotSupportedException.MessageData.Text;
                                message += System.Environment.NewLine;
                                foreach (ExceptionMessageData detailMessageData in engineeringNotSupportedException.DetailMessageData)
                                {
                                    message += detailMessageData.Text + System.Environment.NewLine;
                                }
                                ShowMessage(message, "EngineeringNotSupportedException");
                            }
                            catch (EngineeringTargetInvocationException ex)
                            {
                                string message = ex.Message;
                                ShowMessage(message, "EngineeringTargetInvocationException");
                                return;
                            }
                            catch (EngineeringException)
                            {
                                //Do not catch general exceptions
                                ShowMessage("EngineeringException", "EngineeringException");
                                return;
                            }
                            catch (NonRecoverableException nonRecoverableException)
                            {
                                ShowMessage(nonRecoverableException.Message, "NonRecoverableException");
                            }
                            catch (Exception ex)
                            {
                                string failText = "Exception during rename:" + Environment.NewLine + ex + Environment.NewLine + ex.TargetSite;
                                Trace.TraceError(failText);
                                ShowMessage(failText, "Excpetion Occured Duuring Rename Operation");
                                return;
                            }
                            if (transaction.CanCommit)
                            {
                                transaction.CommitOnDispose();
                            }
                        }
                    }
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
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

        private void RenameAllSelectedPlcBlockGroupsAndSubFolders(MenuSelectionProvider<PlcBlockUserGroup> menuSelectionProvider)
        {
            using (FileStream fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;

                bool replaceClicked = false;

                DialogResult result = ShowMyDialogBox();
                if (result == DialogResult.OK)
                    replaceClicked = true;

                if (replaceClicked & !_find.Equals(_replace))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    using (ExclusiveAccess exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming  folder and contents from: " + _find + " to: " + _replace))
                    {
                        Project project = _tiaPortal.Projects.First();
                        _projectName = project.Name;
                        IEnumerable<object> menuSelection = menuSelectionProvider.GetSelection();

                        DirectoryInfo exportDirInfo = Directory.CreateDirectory(Path.Combine(_defaultExportFolderPath, _projectName)); ;

                        List<PlcBlockUserGroupProxy> blockGroups = new List<PlcBlockUserGroupProxy>();

                        using (Transaction transaction = exclusiveAccess.Transaction(project, "Renaming folders from: " + _find + " to: " + _replace))
                        {
                            try
                            {
                                ExportVisitor exportVisitor = new ExportVisitor(exclusiveAccess, exportDirInfo, ExportOptions.None);
                                ImportVisitor importVisitor = new ImportVisitor(exclusiveAccess, exportDirInfo);
                                RenameVisitor renameVisitor = new RenameVisitor(_find, _replace, exportVisitor, importVisitor, exclusiveAccess);

                                foreach (PlcBlockUserGroup PlcBlockUserGroup in menuSelection)
                                    blockGroups.Add(new PlcBlockUserGroupProxy(PlcBlockUserGroup));

                                foreach (PlcBlockUserGroupProxy blockGroup in blockGroups)
                                {
                                    if (!blockGroup.CheckFBlocks())
                                        blockGroup.Accept(renameVisitor);
                                }
                            }
                            catch (EngineeringSecurityException engineeringSecurityException)
                            {
                                ShowMessage(engineeringSecurityException.ToString(), "EngineeringSecurityException");
                                //Console.WriteLine(engineeringSecurityException);
                            }
                            catch (EngineeringObjectDisposedException engineeringObjectDisposedException)
                            {
                                ShowMessage(engineeringObjectDisposedException.Message, "EngineeringObjectDisposedException");
                            }
                            catch (EngineeringNotSupportedException engineeringNotSupportedException)
                            {
                                string message = engineeringNotSupportedException.MessageData.Text;
                                message += System.Environment.NewLine;
                                foreach (ExceptionMessageData detailMessageData in engineeringNotSupportedException.DetailMessageData)
                                {
                                    message += detailMessageData.Text + System.Environment.NewLine;
                                }
                                ShowMessage(message, "EngineeringNotSupportedException");
                            }
                            catch (EngineeringTargetInvocationException ex)
                            {
                                string message = ex.Message;
                                ShowMessage(message, "EngineeringTargetInvocationException");
                                return;
                            }
                            catch (EngineeringException)
                            {
                                //Do not catch general exceptions
                                ShowMessage("EngineeringException", "EngineeringException");
                                return;
                            }
                            catch (NonRecoverableException nonRecoverableException)
                            {
                                ShowMessage(nonRecoverableException.Message, "NonRecoverableException");
                            }
                            catch (Exception ex)
                            {
                                string failText = "Exception during rename:" + Environment.NewLine + ex + Environment.NewLine + ex.TargetSite;
                                Trace.TraceError(failText);
                                ShowMessage(failText, "Excpetion Occured Duuring Rename Operation");
                                return;
                            }
                            if (transaction.CanCommit)
                            {
                                transaction.CommitOnDispose();
                            }
                        }
                    }
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
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

        private void ShowMessage(string message, string title)
        {
            using (Form owner = Util.GetForegroundWindow())
            {
                MessageBox.Show(owner, message, title);
            }
        }

        private void RenameAllSelectedPlcBlocks(MenuSelectionProvider menuSelectionProvider)
        {
            IEnumerable<object> menuSelction = menuSelectionProvider.GetSelection();
            using (FileStream fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;

                using (ExclusiveAccess exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming blocks..."))
                {
                    Project project = _tiaPortal.Projects.First();
                    using (Transaction transaction = exclusiveAccess.Transaction(project, "Renaming blocks"))
                    {
                        bool replaceClicked = false;

                        DialogResult result = ShowMyDialogBox();
                        if (result == DialogResult.OK)
                            replaceClicked = true;

                        if (replaceClicked & !_find.Equals(_replace))
                        {
                            foreach (PlcBlock block in menuSelction)
                            {
                                block.Name = block.Name.Replace(_find, _replace);
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
            using (FileStream fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;
                using (ExclusiveAccess exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming blocks..."))
                {
                    Project project = _tiaPortal.Projects.First();
                    using (Transaction transaction = exclusiveAccess.Transaction(project, "Renaming blocks"))
                    {
                        bool replaceClicked = false;

                        if (ShowMyDialogBox() == DialogResult.OK)
                            replaceClicked = true;

                        if (replaceClicked & !_find.Equals(_replace))
                        {
                            foreach (DeviceUserGroup deviceGroup in menuSelction)
                            {
                                UDeviceGroup uDeviceGroup = new UDeviceGroup(deviceGroup);
                                uDeviceGroup.Rename(_find, _replace);
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
            using (FileStream fileStream = new FileStream(_traceFilePath, FileMode.Append))
            {
                Trace.Listeners.Add(new TextWriterTraceListener(fileStream) { TraceOutputOptions = TraceOptions.DateTime });
                Trace.AutoFlush = true;
                using (ExclusiveAccess exclusiveAccess = _tiaPortal.ExclusiveAccess("Renaming blocks..."))
                {
                    Project project = _tiaPortal.Projects.First();
                    using (Transaction transaction = exclusiveAccess.Transaction(project, "Renaming blocks"))
                    {
                        List<Utility.UDeviceGroup> deviceGroups = new List<Utility.UDeviceGroup>();
                        bool replaceClicked = false;

                        if (ShowMyDialogBox() == DialogResult.OK)
                            replaceClicked = true;

                        if (replaceClicked & !_find.Equals(_replace))
                        {
                            foreach (Device device in menuSelction)
                            {
                                device.Name = device.Name.Replace(_find, _replace);
                                foreach (DeviceItem deviceItem in device.DeviceItems)
                                {
                                    deviceItem.Name = deviceItem.Name.Replace(_find, _replace);
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

        //public void Start()
        //{
        //    var t = new Thread(() => ShowMyDialogBox());
        //    t.Name = "FormThread";
        //    t.SetApartmentState(ApartmentState.STA);
        //    t.Start();
        //}

        public DialogResult ShowMyDialogBox()
        {
            DialogResult result = DialogResult.None;
            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            RenameForm renameForm = new RenameForm();

            if (renameForm.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                _find = renameForm.textBox_Find.Text;
                _replace = renameForm.textBox_Replace.Text;
                result = DialogResult.OK;
            }
            renameForm.Dispose();
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