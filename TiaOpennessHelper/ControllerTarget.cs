using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using Siemens.Engineering.SW.ExternalSources;
using Siemens.Engineering.SW.Tags;
using Siemens.Engineering.SW.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {

        #region GetSpecific

        /// <summary>
        /// Adds all PlcBlocks with the defined ProgrammingLanguage to collection
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="language">The language.</param>
        /// <param name="collection">The collection.</param>
        private static void RecursiveFindBlocksByLanguage(PlcBlockGroup folder, ProgrammingLanguage language, ref Collection<PlcBlock> collection)
        {
            foreach (PlcBlock block in folder.Blocks)
            {
                if (block.ProgrammingLanguage == language)
                    collection.Add(block);
            }
            foreach (PlcBlockUserGroup subFolder in folder.Groups)
            {
                RecursiveFindBlocksByLanguage(subFolder, language, ref collection);
            }
        }

        /// <summary>
        /// Returns a Collection of Blocks with the specified Programming language
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <param name="language">Programming language to search for</param>
        /// <returns>Collection of matching blocks</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        public static IEnumerable<PlcBlock> FindBlocksByLanguage(PlcSoftware plcSoftware, ProgrammingLanguage language)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            Collection<PlcBlock> ret = new Collection<PlcBlock>();

            RecursiveFindBlocksByLanguage(plcSoftware.BlockGroup, language, ref ret);

            return ret;
        }

        /// <summary>
        /// Searches recursively through the PlcSoftware to find the block with given name.
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <param name="blockName">Name to be searched for</param>
        /// <returns>Reference to found block</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;blockName</exception>
        public static PlcBlock FindBlockByName(PlcSoftware plcSoftware, string blockName)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");
            if (string.IsNullOrEmpty(blockName))
                throw new ArgumentException("Parameter is null or empty", nameof(blockName));

            return RecursiveFindElementByName(plcSoftware.BlockGroup, blockName) as PlcBlock;
        }

        /// <summary>
        /// Searches recursively through the PlcSoftware to find the PlcTagTable with given name.
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <param name="tagTableName">Name to be searched for</param>
        /// <returns>Reference to found PlcTagTable</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;tagTableName</exception>
        public static PlcTagTable FindPlcTagTableByName(PlcSoftware plcSoftware, string tagTableName)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "PlcSoftware");
            if (string.IsNullOrEmpty(tagTableName))
                throw new ArgumentException("Parameter is null or empty", nameof(tagTableName));

            return RecursiveFindElementByName(plcSoftware.TagTableGroup, tagTableName) as PlcTagTable;
        }

        /// <summary>
        /// Searches recursively through the PlcSoftware to find the PlcType with given name.
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <param name="datatypeName">Name to be searched for</param>
        /// <returns>Reference to found PlcTagTable</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;datatypeName</exception>
        public static PlcType FindPlcTypeByName(PlcSoftware plcSoftware, string datatypeName)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");
            if (string.IsNullOrEmpty(datatypeName))
                throw new ArgumentException("Parameter is null or empty", nameof(datatypeName));

            return RecursiveFindElementByName(plcSoftware.TypeGroup, datatypeName) as PlcType;
        }

        #endregion

        #region GetAllItems
        /// <summary>
        /// Returns a Collection of all Programm Blocks in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>Collection of PlcBlocks</returns>
        /// <exception cref="System.ArgumentNullException">PArameter is null;PlcSoftware</exception>
        public static IEnumerable<PlcBlock> GetAllBlocks(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(plcSoftware.BlockGroup, ref collection);

            return collection.Cast<PlcBlock>();
        }

        /// <summary>
        /// Returns IEnumerable Collection of all PlcTagTables in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>Collection of ControlerTagTables</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;controlllerTarget</exception>
        public static IEnumerable<PlcTagTable> GetAllTagTables(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(plcSoftware.TagTableGroup, ref collection);

            return collection.Cast<PlcTagTable>();
        }

        /// <summary>
        /// Returns a Collection of all PlcTypes in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>Collection of PlcTypes</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is nul;PlcSoftware</exception>
        public static IEnumerable<PlcType> GetAllDatatypes(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(plcSoftware.TypeGroup, ref collection);

            return collection.Cast<PlcType>();
        }

        /// <summary>
        /// Returns a Collection of all ExternalSourceFiles in the PlcSoftware
        /// </summary>
        /// <param name="plcSoftware">PlcSoftware to be searched</param>
        /// <returns>Collection of ExternalSourceFiles</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;PlcSoftware</exception>
        public static IEnumerable<PlcExternalSource> GetAllPlcExternalSourceFiles(PlcSoftware plcSoftware)
        {
            if (plcSoftware == null)
                throw new ArgumentNullException(nameof(plcSoftware), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(plcSoftware.ExternalSourceGroup, ref collection);

            return collection.Cast<PlcExternalSource>();
        }

        #endregion
    }
}
