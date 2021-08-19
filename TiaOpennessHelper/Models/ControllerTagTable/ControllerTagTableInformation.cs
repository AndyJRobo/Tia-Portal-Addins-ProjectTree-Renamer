using System;
using System.Collections.Generic;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.ControllerTagTable
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TiaOpennessHelper.Models.XmlInformation" />
    /// TODO Edit XML Comment Template for PlcTagTableInformation
    public class PlcTagTableInformation : XmlInformation
    {
        /// <summary>The tags</summary>
        /// TODO Edit XML Comment Template for tags
        private List<PlcTagTableTag> _tags;
        /// <summary>Gets or sets the tags.</summary>
        /// <value>The tags.</value>
        /// TODO Edit XML Comment Template for Tags
        public List<PlcTagTableTag> Tags
        {
            get => _tags;
            set => _tags = value;
        }

        /// <summary>The constants</summary>
        /// TODO Edit XML Comment Template for constants
        private List<PlcTagTableConstant> _constants;
        /// <summary>Gets or sets the constants.</summary>
        /// <value>The constants.</value>
        /// TODO Edit XML Comment Template for Constants
        public List<PlcTagTableConstant> Constants
        {
            get => _constants;
            set => _constants = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlcTagTableInformation"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public PlcTagTableInformation()
        {
            _tags = new List<PlcTagTableTag>();
            _constants = new List<PlcTagTableConstant>();
        }

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
            ret += "<SW.PlcTagTable ID=\"" + id++ + "\">" + Environment.NewLine;
            ret += "<AttributeList>" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(Name) + "</Name>" + Environment.NewLine;
            ret += "</AttributeList>" + Environment.NewLine;
            ret += "<ObjectList>" + Environment.NewLine;
            foreach (PlcTagTableConstant constant in _constants)
            {
                ret += constant.ToString(ref id);
            }
            foreach (PlcTagTableTag tag in _tags)
            {
                ret += tag.ToString(ref id);
            }
            ret += "</ObjectList>" + Environment.NewLine;
            ret += "</SW.PlcTagTable>" + Environment.NewLine;
            ret += "</Document>" + Environment.NewLine;

            return ret;
        }
    }
}
