using System;
using TiaOpennessHelper.Models.Members;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.ControllerTagTable
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Member" />
    /// TODO Edit XML Comment Template for PlcTagTableTag
    public class PlcTagTableTag : Member
    {
        /// <summary>The address</summary>
        /// TODO Edit XML Comment Template for address
        private string _address;

        /// <summary>Gets or sets the address.</summary>
        /// <value>The address.</value>
        /// TODO Edit XML Comment Template for Address
        public string Address
        {
            get => _address;
            set => _address = value;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        /// TODO Edit XML Comment Template for ToString
        public string ToString(ref int id)
        {
            string ret = "";

            ret += "<SW.ControllerTag ID=\"" + id++ + "\" CompositionName=\"Tags\">" + Environment.NewLine;
            ret += "<AttributeList>" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(MemberName) + "</Name>" + Environment.NewLine;
            ret += "<LogicalAddress>" + AdjustNames.AdjustXmlStrings(Address) + "</LogicalAddress>" + Environment.NewLine;
            ret += "</AttributeList>" + Environment.NewLine;
            ret += "<LinkList>" + Environment.NewLine;
            ret += "<DataType TargetID=\"@OpenLink\">" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(MemberDatatype) + "</Name>" + Environment.NewLine;
            ret += "</DataType>" + Environment.NewLine;
            ret += "</LinkList>" + Environment.NewLine;
            ret += "<ObjectList>" + Environment.NewLine;
            ret += MemberComment.ToString(ref id);
            ret += "</ObjectList>" + Environment.NewLine;
            ret += "</SW.ControllerTag>" + Environment.NewLine;

            return ret;
        }
    }
}
