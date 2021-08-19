using System;
using TiaOpennessHelper.Enums;
using TiaOpennessHelper.Models.Block;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.DataBlock
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="BlockInformation" />
    /// TODO Edit XML Comment Template for DataBlockInformation
    public class DataBlockInformation : BlockInformation
    {
        //DataBlockType (ArrayDB, Shared DB...)
        /// <summary>The database type</summary>
        /// TODO Edit XML Comment Template for dbType
        private DatablockType _dbType;

        /// <summary>Gets or sets the type of the database.</summary>
        /// <value>The type of the database.</value>
        /// TODO Edit XML Comment Template for DbType
        public DatablockType DbType
        {
            get => _dbType;
            set => _dbType = value;
        }

        //Name of UDT if DB is ArrayDB
        /// <summary>Gets or sets the type of the array data.</summary>
        /// <value>The type of the array data.</value>
        /// TODO Edit XML Comment Template for ArrayDataType
        public string InstanceOfType { get; set; }

        //Name of FB if DB is IDB
        /// <summary>Gets or sets the name of the instance of.</summary>
        /// <value>The name of the instance of.</value>
        /// TODO Edit XML Comment Template for InstanceOfName
        public string InstanceOfName { get; set; }

        /// <summary>Gets or sets the download without reinit.</summary>
        /// <value>The download without reinit.</value>
        /// TODO Edit XML Comment Template for DownloadWithoutReinit
        public string DownloadWithoutReinit { get; set; }

        /// <summary>Gets or sets the is only stored in load memory.</summary>
        /// <value>The is only stored in load memory.</value>
        /// TODO Edit XML Comment Template for IsOnlyStoredInLoadMemory
        public string IsOnlyStoredInLoadMemory { get; set; }

        /// <summary>Gets or sets the is retain memory resource enabled.</summary>
        /// <value>The is retain memory resource enabled.</value>
        /// TODO Edit XML Comment Template for IsRetainMemResEnabled
        public string IsRetainMemResEnabled { get; set; }

        /// <summary>Gets or sets the is write protected in as.</summary>
        /// <value>The is write protected in as.</value>
        /// TODO Edit XML Comment Template for IsWriteProtectedInAS
        public string IsWriteProtectedInAs { get; set; }

        /// <summary>Gets or sets the memory reserve.</summary>
        /// <value>The memory reserve.</value>
        /// TODO Edit XML Comment Template for MemoryReserve
        public string MemoryReserve { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        /// TODO Edit XML Comment Template for ToString
        public override string ToString()
        {
            string ret = "";
            int id = 0;

            ret += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
            ret += "<Document>" + Environment.NewLine;
            ret += "<SW.DataBlock ID=\"" + id++ + "\">" + Environment.NewLine;
            ret += "<AttributeList>" + Environment.NewLine;
            ret += "<AutoNumber>" + AdjustNames.AdjustXmlStrings(BlockAutoNumber) + "</AutoNumber>" + Environment.NewLine;
            ret += "<DownloadWithoutReinit>" + AdjustNames.AdjustXmlStrings(DownloadWithoutReinit) + "</DownloadWithoutReinit>" + Environment.NewLine;
            ret += "<DatablockType>" + AdjustNames.AdjustXmlStrings(DbType.ToString()) + "</DatablockType>" + Environment.NewLine;
            ret += "<HeaderAuthor>" + AdjustNames.AdjustXmlStrings(BlockAuthor) + "</HeaderAuthor>" + Environment.NewLine;
            ret += "<HeaderFamily>" + AdjustNames.AdjustXmlStrings(BlockFamily) + "</HeaderFamily>" + Environment.NewLine;
            ret += "<HeaderName>" + AdjustNames.AdjustXmlStrings(BlockUserId) + "</HeaderName>" + Environment.NewLine;
            ret += "<HeaderVersion>" + AdjustNames.AdjustXmlStrings(BlockVersion) + "</HeaderVersion>" + Environment.NewLine;
            ret += "<InstanceOfName>" + AdjustNames.AdjustXmlStrings(InstanceOfName) + "</InstanceOfName>" + Environment.NewLine;
            ret += BlockInterface.ToString();
            ret += "<IsOnlyStoredInLoadMemory>" + AdjustNames.AdjustXmlStrings(IsOnlyStoredInLoadMemory) + "</IsOnlyStoredInLoadMemory>" + Environment.NewLine;
            ret += "<IsRetainMemResEnabled>" + AdjustNames.AdjustXmlStrings(IsRetainMemResEnabled) + "</IsRetainMemResEnabled>" + Environment.NewLine;
            ret += "<IsWriteProtectedInAS>" + AdjustNames.AdjustXmlStrings(IsWriteProtectedInAs) + "</IsWriteProtectedInAS>" + Environment.NewLine;
            ret += "<MemoryLayout>" + AdjustNames.AdjustXmlStrings(BlockMemoryLayout) + "</MemoryLayout>" + Environment.NewLine;
            ret += "<MemoryReserve>" + AdjustNames.AdjustXmlStrings(MemoryReserve) + "</MemoryReserve>" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(Name) + "</Name>" + Environment.NewLine;
            ret += "<Number>" + BlockNumber + "</Number>" + Environment.NewLine;
            ret += "<ProgrammingLanguage>DB</ProgrammingLanguage>" + Environment.NewLine;
            ret += "<Type>DB</Type>" + Environment.NewLine;
            ret += "</AttributeList>" + Environment.NewLine;
            ret += "<ObjectList>" + Environment.NewLine;
            ret += BlockComment.ToString(ref id);
            ret += BlockTitle.ToString(ref id);
            ret += "</ObjectList>" + Environment.NewLine;
            ret += "</SW.DataBlock>" + Environment.NewLine;
            ret += "</Document>" + Environment.NewLine;
            return ret;
        }
    }
}