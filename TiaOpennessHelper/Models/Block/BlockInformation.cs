using System;
using System.Collections.Generic;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="TiaOpennessHelper.Models.XmlInformation" />
    /// TODO Edit XML Comment Template for BlockInformation
    public class BlockInformation : XmlInformation
    {
        //general information
        /// <summary>Gets or sets the block number.</summary>
        /// <value>The block number.</value>
        /// TODO Edit XML Comment Template for BlockNumber
        public string BlockNumber { get; set; }

        /// <summary>Gets or sets the type of the block.</summary>
        /// <value>The type of the block.</value>
        /// TODO Edit XML Comment Template for BlockType
        public string BlockType { get; set; }       //DB, FC, FB, OB ...

        /// <summary>Gets or sets the block language.</summary>
        /// <value>The block language.</value>
        /// TODO Edit XML Comment Template for BlockLanguage
        public string BlockLanguage { get; set; }   //KOP, FUP ...

        /// <summary>Gets or sets the block memory layout.</summary>
        /// <value>The block memory layout.</value>
        /// TODO Edit XML Comment Template for BlockMemoryLayout
        public string BlockMemoryLayout { get; set; }    //autonumber = true || false

        /// <summary>Gets or sets the block automatic number.</summary>
        /// <value>The block automatic number.</value>
        /// TODO Edit XML Comment Template for BlockAutoNumber
        public string BlockAutoNumber { get; set; }

        /// <summary>Gets or sets the block enable tag readback.</summary>
        /// <value>The block enable tag readback.</value>
        /// TODO Edit XML Comment Template for BlockEnableTagReadback
        public string BlockEnableTagReadback { get; set; }

        /// <summary>Gets or sets the block enable tag readback block properties.</summary>
        /// <value>The block enable tag readback block properties.</value>
        /// TODO Edit XML Comment Template for BlockEnableTagReadbackBlockProperties
        public string BlockEnableTagReadbackBlockProperties { get; set; }

        /// <summary>Gets or sets the block is iec check enabled.</summary>
        /// <value>The block is iec check enabled.</value>
        /// TODO Edit XML Comment Template for BlockIsIECCheckEnabled
        public string BlockIsIecCheckEnabled { get; set; }

        //properties information
        /// <summary>Gets or sets the block title.</summary>
        /// <value>The block title.</value>
        /// TODO Edit XML Comment Template for BlockTitle
        public MultiLanguageText BlockTitle { get; set; }

        /// <summary>Gets or sets the block author.</summary>
        /// <value>The block author.</value>
        /// TODO Edit XML Comment Template for BlockAuthor
        public string BlockAuthor { get; set; }

        /// <summary>Gets or sets the block comment.</summary>
        /// <value>The block comment.</value>
        /// TODO Edit XML Comment Template for BlockComment
        public MultiLanguageText BlockComment { get; set; }

        /// <summary>Gets or sets the block family.</summary>
        /// <value>The block family.</value>
        /// TODO Edit XML Comment Template for BlockFamily
        public string BlockFamily { get; set; }

        /// <summary>Gets or sets the block version.</summary>
        /// <value>The block version.</value>
        /// TODO Edit XML Comment Template for BlockVersion
        public string BlockVersion { get; set; }

        /// <summary>Gets or sets the block user identifier.</summary>
        /// <value>The block user identifier.</value>
        /// TODO Edit XML Comment Template for BlockUserID
        public string BlockUserId { get; set; }

        //Interface Information
        /// <summary>Gets or sets the block interface.</summary>
        /// <value>The block interface.</value>
        /// TODO Edit XML Comment Template for BlockInterface
        public BlockInterface BlockInterface { get; set; }

        /// <summary>List of NetworksInformation</summary>
        /// <value>The block networks.</value>
        public List<Network> BlockNetworks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockInformation"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public BlockInformation()
        {
            BlockNetworks = new List<Network>();
            BlockInterface = new BlockInterface();
            BlockTitle = new MultiLanguageText();
            BlockComment = new MultiLanguageText();
        }

        public override string ToString()
        {
            string ret = "";
            int id = 0;

            ret += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
            ret += "<Document>" + Environment.NewLine;
            ret += "<SW.CodeBlock ID=\"" + id++ + "\">" + Environment.NewLine;
            ret += "<AttributeList>" + Environment.NewLine;
            ret += "<AutoNumber>" + AdjustNames.AdjustXmlStrings(BlockAutoNumber) + "</AutoNumber>" + Environment.NewLine;
            ret += "<HeaderAuthor>" + AdjustNames.AdjustXmlStrings(BlockAuthor) + "</HeaderAuthor>" + Environment.NewLine;
            ret += "<HeaderFamily>" + AdjustNames.AdjustXmlStrings(BlockFamily) + "</HeaderFamily>" + Environment.NewLine;
            ret += "<HeaderName>" + AdjustNames.AdjustXmlStrings(BlockUserId) + "</HeaderName>" + Environment.NewLine;
            ret += "<HeaderVersion>" + AdjustNames.AdjustXmlStrings(BlockVersion) + "</HeaderVersion>" + Environment.NewLine;
            ret += BlockInterface.ToString();
            ret += "<MemoryLayout>" + AdjustNames.AdjustXmlStrings(BlockMemoryLayout) + "</MemoryLayout>" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(Name) + "</Name>" + Environment.NewLine;
            ret += "<Number>" + BlockNumber + "</Number>" + Environment.NewLine;
            ret += "<ProgrammingLanguage>DB</ProgrammingLanguage>" + Environment.NewLine;
            ret += "<Type></Type>" + Environment.NewLine;
            ret += "</AttributeList>" + Environment.NewLine;
            ret += "<ObjectList>" + Environment.NewLine;
            ret += BlockComment.ToString(ref id);
            ret += BlockTitle.ToString(ref id);
            ret += "</ObjectList>" + Environment.NewLine;
            ret += "</SW.CodeBlock>" + Environment.NewLine;
            ret += "</Document>" + Environment.NewLine;
            return ret;
        }
    }
}