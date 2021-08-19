using Siemens.Engineering;
using Siemens.Engineering.Cax;
using Siemens.Engineering.Hmi.Communication;
using Siemens.Engineering.Hmi.Cycle;
using Siemens.Engineering.Hmi.Globalization;
using Siemens.Engineering.Hmi.RuntimeScripting;
using Siemens.Engineering.Hmi.Screen;
using Siemens.Engineering.Hmi.Tag;
using Siemens.Engineering.Hmi.TextGraphicList;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using Siemens.Engineering.SW.ExternalSources;
using Siemens.Engineering.SW.Tags;
using Siemens.Engineering.SW.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        #region Public Methods

        /// <summary>
        /// Exports the selected structure including subdirectories. Missing folders will be created.
        /// </summary>
        /// <param name="elementToExport">Project, station or folder to export</param>
        /// <param name="exportPath">Folder path in which to export</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        public static void ExportStructure(IEngineeringCompositionOrObject elementToExport, string exportPath)
        {
            ExportStructure(elementToExport, (ExportOptions.WithDefaults | ExportOptions.WithReadOnly), exportPath);
        }

        /// <summary>
        /// Exports the selected structure including subdirectories. Missing folders will be created.
        /// </summary>
        /// <param name="elementToExport">Project, station or folder to export</param>
        /// <param name="exportOptions">The export options.</param>
        /// <param name="exportPath">Folder path in which to export</param>
        /// <exception cref="System.ArgumentNullException">Parameter is null;elementToExport</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;exportPath</exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        public static void ExportStructure(IEngineeringCompositionOrObject elementToExport, ExportOptions exportOptions, string exportPath)
        {
            if (elementToExport == null)
                throw new ArgumentNullException(nameof(elementToExport), "Parameter is null");
            if (string.IsNullOrEmpty(exportPath))
                throw new ArgumentException("Parameter is null or empty", nameof(exportPath));


            if (elementToExport is PlcBlock || elementToExport is PlcTagTable || elementToExport is PlcType || elementToExport is PlcExternalSource
                || elementToExport is ScreenOverview || elementToExport is ScreenGlobalElements || elementToExport is TagTable || elementToExport is Screen
                || elementToExport is Cycle || elementToExport is Connection || elementToExport is MultiLingualGraphic || elementToExport is ScreenTemplate
                || elementToExport is VBScript || elementToExport is TextList || elementToExport is ScreenPopup || elementToExport is ScreenSlidein)
            {
                ExportItem(elementToExport as IEngineeringObject, exportOptions, exportPath);
                return;
            }

            string folderName = GetObjectName(elementToExport);

            string newPath = Path.Combine(exportPath, folderName);
            Directory.CreateDirectory(newPath);

            foreach (object item in GetSubItem(elementToExport))
            {
                if (!(item is Cycle && (item as Cycle).IsSystemObject))
                {
                    //add if for inconsistence
                    ExportItem(item as IEngineeringObject, exportOptions, newPath);
                }
            }

            foreach (object folder in GetSubFolder(elementToExport))
            {
                ExportStructure(folder as IEngineeringCompositionOrObject, exportOptions, newPath);
            }
        }

        /// <summary>
        /// Exports the given object with the give options to the defined path.
        /// </summary>
        /// <param name="exportItem">Object to export</param>
        /// <param name="exportOption">Export options</param>
        /// <param name="exportPath">Folder path in which to export</param>
        /// <returns>String</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;exportItem</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;exportPath</exception>
        /// <exception cref="Siemens.Engineering.EngineeringException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        public static string ExportItem(IEngineeringObject exportItem, ExportOptions exportOption, string exportPath)
        {
            if (exportItem == null)
                throw new ArgumentNullException(nameof(exportItem), "Parameter is null");
            if (string.IsNullOrEmpty(exportPath))
                throw new ArgumentException("Parameter is null or empty", nameof(exportPath));

            string filePath = Path.GetFullPath(exportPath);

            if (exportItem is PlcBlock)
            {
                PlcBlock block = exportItem as PlcBlock;
                if (block.ProgrammingLanguage == ProgrammingLanguage.ProDiag || block.ProgrammingLanguage == ProgrammingLanguage.ProDiag_OB)
                    return null;
                if (block.IsConsistent)
                {
                    filePath = Path.Combine(filePath, AdjustNames.AdjustFileName(GetObjectName(exportItem)) + ".xml");
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    (exportItem as PlcBlock).Export(new FileInfo(filePath), exportOption);

                    return filePath;
                }
                throw new EngineeringException(string.Format(CultureInfo.InvariantCulture, "Block: {0} is inconsistent! Export will be aborted!", GetObjectName(exportItem)));
            }
            if (exportItem is PlcTagTable || exportItem is PlcType || exportItem is ScreenOverview || exportItem is ScreenGlobalElements
                || exportItem is Screen || exportItem is TagTable || exportItem is Connection || exportItem is GraphicList
                || exportItem is TextList || exportItem is Cycle || exportItem is MultiLingualGraphic || exportItem is ScreenTemplate
                || exportItem is VBScript || exportItem is ScreenPopup || exportItem is ScreenSlidein)
            {
                Directory.CreateDirectory(filePath);
                filePath = Path.Combine(filePath, AdjustNames.AdjustFileName(GetObjectName(exportItem)) + ".xml");
                File.Delete(filePath);
                Dictionary<Type, object> parameter = new Dictionary<Type, object>
                {
                    { typeof(FileInfo), new FileInfo(filePath) },
                    { typeof(ExportOptions), exportOption }
                };
                exportItem.Invoke("Export", parameter);
                return filePath;
            }
            if (exportItem is PlcExternalSource)
            {
                //Directory.CreateDirectory(filePath);
                //filePath = Path.Combine(filePath, AdjustNames.AdjustFileName(GetObjectName(exportItem)));
                //File.Delete(filePath);
                //File.Create(filePath);
                //return filePath;
            }
            return null;
        }

        /// <summary>Generates a source file from the given PlcBlock</summary>
        /// <param name="exportItem">The export item.</param>
        /// <param name="exportPath">The export path.</param>
        /// <param name="options"></param>
        /// <returns>String</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;exportItem</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;exportPath</exception>
        /// <exception cref="Siemens.Engineering.EngineeringException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        public static string GenerateSourceFromBlock(PlcBlock exportItem, string exportPath, GenerateOptions options)
        {
            if (exportItem == null)
                throw new ArgumentNullException(nameof(exportItem), "Parameter is null");
            if (string.IsNullOrEmpty(exportPath))
                throw new ArgumentException("Parameter is null or empty", nameof(exportPath));

            string filePath = Path.GetFullPath(exportPath);

            if (!exportItem.IsKnowHowProtected)
            {
                Directory.CreateDirectory(filePath);
                switch (exportItem.ProgrammingLanguage)
                {
                    case ProgrammingLanguage.DB:
                        filePath = Path.Combine(filePath, AdjustNames.AdjustFileName(exportItem.Name) + ".db");
                        break;
                    case ProgrammingLanguage.SCL:
                        filePath = Path.Combine(filePath, AdjustNames.AdjustFileName(exportItem.Name) + ".scl");
                        break;
                    case ProgrammingLanguage.STL:
                        filePath = Path.Combine(filePath, AdjustNames.AdjustFileName(exportItem.Name) + ".awl");
                        break;
                    default:
                        return null;
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                IEngineeringInstance temp = exportItem;

                do
                {
                    temp = temp.Parent;
                }
                while (!(temp is PlcSoftware));

                (temp as PlcSoftware).ExternalSourceGroup.GenerateSource(new[] { exportItem }, new FileInfo(filePath), options);

                return filePath;
            }
            throw new EngineeringException(string.Format(CultureInfo.InvariantCulture, "Block: '{0}' is Know-how protected! \r\n 'Generate source from block' is not possible on know how protected blocks!", exportItem.Name));
        }

        /// <summary>Deletes everything in the given folder</summary>
        /// <param name="projectName">Name of the folder which holds the exported files</param>
        /// <param name="exportPath">Folder Path in which the exported project folder is</param>
        /// <exception cref="System.ArgumentException">
        /// Parameter is null or empty;exportPath
        /// or
        /// Parameter is null or empty;projectName
        /// </exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        public static void DeletePreviousExportedElements(string projectName, string exportPath)
        {
            if (string.IsNullOrEmpty(exportPath))
                throw new ArgumentException("Parameter is null or empty", nameof(exportPath));
            if (string.IsNullOrEmpty(projectName))
                throw new ArgumentException("Parameter is null or empty", nameof(projectName));


            string[] files = Directory.GetFiles(Path.Combine(exportPath, projectName));
            string[] dirs = Directory.GetDirectories(Path.Combine(exportPath, projectName));

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeletePreviousExportedElements(dir, exportPath);
            }

            Directory.Delete(Path.Combine(exportPath, projectName), false);
        }

        public static bool CaxExport(Project project, string exportPath)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project), "Parameter is null");
            if (string.IsNullOrEmpty(exportPath))
                throw new ArgumentException("Parameter is null or empty", nameof(exportPath));

            CaxProvider provider = project.GetService<CaxProvider>();
            if (project == null) return false;

            FileInfo filePath = new FileInfo(Path.Combine(exportPath, project.Name + ".aml"));
            FileInfo logPath = new FileInfo(Path.Combine(exportPath, "Export.log"));

            return provider.Export(project, filePath, logPath);
        }

        #endregion
    }
}
