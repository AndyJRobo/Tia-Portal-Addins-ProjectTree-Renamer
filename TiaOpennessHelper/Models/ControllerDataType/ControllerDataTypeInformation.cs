using System;
using System.Collections.Generic;
using TiaOpennessHelper.Models.Members;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.ControllerDataType
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TiaOpennessHelper.Models.XmlInformation" />
    /// TODO Edit XML Comment Template for PlcTypeInformation
    public class PlcTypeInformation : XmlInformation
    {
        /// <summary>Gets or sets the udt title.</summary>
        /// <value>The udt title.</value>
        /// TODO Edit XML Comment Template for UDTTitle
        public MultiLanguageText UdtTitle { get; set; }
        /// <summary>Gets or sets the udt comment.</summary>
        /// <value>The udt comment.</value>
        /// TODO Edit XML Comment Template for UDTComment
        public MultiLanguageText UdtComment { get; set; }

        //Interface Information
        /// <summary>Gets or sets the datatype member.</summary>
        /// <value>The datatype member.</value>
        /// TODO Edit XML Comment Template for DatatypeMember
        public List<Member> DatatypeMember { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="PlcTypeInformation"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public PlcTypeInformation()
        {
            DatatypeMember = new List<Member>();

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
            ret += "<SW.PlcType ID=\"" + id++ + "\">" + Environment.NewLine;
            ret += "<AttributeList>" + Environment.NewLine;
            ret += "<Interface>" + Environment.NewLine;
            ret += "<Sections xmlns=\"http://www.siemens.com/automation/Openness/SW/Interface/v1\">" + Environment.NewLine;
            ret += "<Section Name=\"None\">" + Environment.NewLine;
            foreach (Member member in DatatypeMember)
            {
                ret += member + Environment.NewLine;
            }
            ret += "</Section>" + Environment.NewLine;
            ret += "</Sections>" + Environment.NewLine;
            ret += "</Interface>" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(Name) + "</Name>" + Environment.NewLine;
            ret += "</AttributeList>" + Environment.NewLine;

            ret += "<ObjectList>" + Environment.NewLine;
            ret += UdtComment.ToString(ref id) + Environment.NewLine;
            ret += UdtTitle.ToString(ref id) + Environment.NewLine;
            ret += "</ObjectList>" + Environment.NewLine;

            ret += "</SW.PlcType>" + Environment.NewLine;
            ret += "</Document>";

            return ret;
        }
    }
}
