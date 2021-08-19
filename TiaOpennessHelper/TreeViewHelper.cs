using Siemens.Engineering;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.Hmi.Cycle;
using Siemens.Engineering.HW;
using Siemens.Engineering.Library;
using Siemens.Engineering.Library.MasterCopies;
using Siemens.Engineering.Library.Types;
using Siemens.Engineering.SW;
using System;
using System.Windows.Controls;

namespace TiaOpennessHelper
{
    /// <summary>
    /// Helper Class to retrive projektnavigation as TreeViewItem
    /// </summary>
    public class OpennessTreeViews
    {
        /// <summary>
        /// Adds all objects in the provided folder as TreeViewItems to the provided treeViewItem
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="treeViewItem">The tree view item.</param>
        private static void RecursiveGetTreeView(IEngineeringInstance folder, ref TreeViewItem treeViewItem)
        {
            treeViewItem.Header = OpennessHelper.GetObjectName(folder);
            treeViewItem.Tag = folder;

            foreach (object item in OpennessHelper.GetSubItem(folder))
            {
                TreeViewItem sub = new TreeViewItem();
                sub.Header = OpennessHelper.GetObjectName(item as IEngineeringInstance);
                sub.Tag = item;

                if (!(item is Cycle && (item as Cycle).IsSystemObject))
                    treeViewItem.Items.Add(sub);
            }

            foreach (object subfolder in OpennessHelper.GetSubFolder(folder))
            {
                TreeViewItem subView = new TreeViewItem();
                RecursiveGetTreeView(subfolder as IEngineeringInstance, ref subView);
                treeViewItem.Items.Add(subView);
            }
        }

        /// <summary>Returns a TreeViewItem of the objects in the IDeviceItem</summary>
        /// <param name="item">The item.</param>
        /// <returns>TreeViewItem</returns>
        private static TreeViewItem RecursiveGetDevicesTreeView(DeviceItem item)
        {
            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.Header = item.Name;
            treeViewItem.Tag = item;

            //if (item.Subtype.ToLowerInvariant().Contains("sinamics"))
            //    return treeViewItem;

            //if (item.Addresses != null)
            //{
            //    foreach (var adr in item.Addresses)
            //    {
            //        treeViewItem.Items.Add(new TreeViewItem
            //        {
            //            Header = adr.ToString(),
            //            Tag = adr
            //        });
            //    }
            //}

            //IInterface itf = ((IEngineeringServiceProvider)item).GetService<IInterface>();
            //if (itf != null)
            //{
            //    foreach (var node in itf.Nodes)
            //    {
            //        treeViewItem.Items.Add(new TreeViewItem
            //        {
            //            Header = node.NodeId,
            //            Tag = node
            //        });
            //    }
            //}

            foreach (DeviceItem subItem in item.DeviceItems)
            {
                TreeViewItem temp = RecursiveGetDevicesTreeView(subItem);
                if (temp != null)
                    treeViewItem.Items.Add(temp);
            }

            return treeViewItem;
        }

        /// <summary>Returns a TreeView of the hardware</summary>
        /// <param name="station">The station.</param>
        /// <returns>TreeViewItem</returns>
        public static TreeViewItem GetHardwareTreeView(Device station)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = station.Name;
            item.Tag = station;

            //if (station.TypeIdentifier.ToLower().Contains("sinamics"))
            //    return item;

            foreach (DeviceItem subItem in station.DeviceItems)
            {
                item.Items.Add(RecursiveGetDevicesTreeView(subItem));
            }
            return item;
        }

        /// <summary>Gets all graphics TreeView.</summary>
        /// <param name="project">The project.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;project</exception>
        /// <exception cref="ArgumentNullException">Parameter is null;project</exception>
        public static TreeViewItem GetGraphicsTreeView(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(project.Graphics, ref collection);

            return collection;
        }

        /// <summary>
        /// Returns a TreeView of all Programm Blocks in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>TreeView of PlcBlocks</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        public static TreeViewItem GetBlocksTreeView(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(plcSoftware.BlockGroup, ref collection);

            return collection;
        }

        /// <summary>
        /// Returns a TreeView of all PlcTagTables in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>TreeView of ControlerTagTables</returns>
        /// <exception cref="System.ArgumentNullException">PArameter is null;PlcSoftware</exception>
        public static TreeViewItem GetTagTablesTreeView(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(plcSoftware.TagTableGroup, ref collection);

            return collection;
        }

        /// <summary>
        /// Returns a TreeView of all PlcTypes in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>TreeView of PlcTypes</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        public static TreeViewItem GetDatatypesTreeView(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(plcSoftware.TypeGroup, ref collection);

            return collection;
        }

        /// <summary>
        /// Returns a TreeView of all ExternalSourceFiles in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>TreeView of ExternalSourceFiles</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        public static TreeViewItem GetExternalSourceFilesTreeView(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(plcSoftware.ExternalSourceGroup, ref collection);

            return collection;
        }

        /// <summary>
        /// Returns a TreeViewItem of all hardware objects in the ContollesTarget
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>TreeViewItem of hardware objects</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        public static TreeViewItem GetHardwareTreeView(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            Device station = plcSoftware.Parent as Device;
            if (station == null) throw new ArgumentNullException(nameof(station));

            TreeViewItem item = new TreeViewItem();
            item.Header = station.Name;
            item.Tag = station;
            foreach (DeviceItem subItem in station.DeviceItems)
            {
                item.Items.Add(RecursiveGetDevicesTreeView(subItem));
            }
            return item;
        }

        /// <summary>Returns TreeViewItem of all screens in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetScreensTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.ScreenFolder, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all TagTables in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetTagTablesTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.TagFolder, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all user Cycles in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetCyclesTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.Cycles, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all Connections in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetConnectionsTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.Connections, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all VB Scripts in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetScriptsTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.VBScriptFolder, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all ScreenTemplates in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetScreenTemplatesTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.ScreenTemplateFolder, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all ScreenPopups in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetScreenPopupTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.ScreenPopupFolder, ref collection);

            return collection;
        }

        /// <summary>Returns TreeViewItem of all ScreenSlideIns in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetScreenSlideInTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            TreeViewItem collection = new TreeViewItem();

            RecursiveGetTreeView(hmiTarget.ScreenSlideinFolder, ref collection);

            return collection;
        }

        /// <summary>
        /// Returns a TreeViewItem of all hardware objects in the HmiTarget
        /// </summary>
        /// <param name="hmiTarget">HmiTarget to be searched</param>
        /// <returns>TreeViewItem of hardware objects</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static TreeViewItem GetHardwareTreeView(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            Device station = hmiTarget.Parent as Device;
            if (station == null) throw new ArgumentNullException(nameof(station));

            TreeViewItem item = new TreeViewItem();
            item.Header = station.Name;
            item.Tag = station;
            foreach (DeviceItem subItem in station.DeviceItems)
            {
                item.Items.Add(RecursiveGetDevicesTreeView(subItem));
            }
            return item;
        }

        /// <summary>Returns all ILibraryTypes in folder as TreeViewItems</summary>
        /// <param name="folder">The folder.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;folder</exception>
        public static TreeViewItem GetTypesTreeView(LibraryTypeFolder folder)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder), "Parameter is null");

            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.Header = folder.Name;
            treeViewItem.Tag = folder;

            foreach (LibraryType type in folder.Types)
            {
                TreeViewItem sub = new TreeViewItem();
                sub.Header = type.Name;
                sub.Tag = type;

                treeViewItem.Items.Add(sub);
            }

            foreach (LibraryTypeUserFolder subfolder in folder.Folders)
            {
                TreeViewItem subView = GetTypesTreeView(subfolder);

                treeViewItem.Items.Add(subView);
            }

            return treeViewItem;
        }

        /// <summary>Returns all MasterCopies in folder as TreeViewItems</summary>
        /// <param name="folder">The folder.</param>
        /// <returns>TreeViewItem</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;folder</exception>
        public static TreeViewItem GetMasterCopiesTreeView(MasterCopyFolder folder)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder), "Parameter is null");

            TreeViewItem treeViewItem = new TreeViewItem();

            treeViewItem.Header = OpennessHelper.GetObjectName(folder);
            treeViewItem.Tag = folder;

            foreach (MasterCopy mCopy in folder.MasterCopies)
            {
                TreeViewItem sub = new TreeViewItem();
                sub.Header = mCopy.Name;
                sub.Tag = mCopy;

                treeViewItem.Items.Add(sub);
            }

            foreach (MasterCopyUserFolder subfolder in folder.Folders)
            {
                TreeViewItem subView = GetMasterCopiesTreeView(subfolder);

                treeViewItem.Items.Add(subView);
            }

            return treeViewItem;
        }

        /// <summary>Returns a TreeViewItem representing the library</summary>
        /// <param name="library">The library.</param>
        /// <returns>TreeViewItem</returns>
        public static TreeViewItem GetLibraryTreeView(ILibrary library)
        {
            TreeViewItem treeViewItem = new TreeViewItem();
            if (library is GlobalLibrary)
            {
                string[] splitPath = (library as GlobalLibrary).Path.FullName.Split('\\', '.');
                treeViewItem.Header = splitPath[splitPath.Length - 2];
            }
            else
            {
                treeViewItem.Header = "Project Library";
            }
            treeViewItem.Tag = library;

            treeViewItem.Items.Add(GetTypesTreeView(library.TypeFolder));
            treeViewItem.Items.Add(GetMasterCopiesTreeView(library.MasterCopyFolder));

            return treeViewItem;
        }
    }
}
