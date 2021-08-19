using System;
using TiaOpennessHelper.Models.Members;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.ControllerTagTable
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Member" />
    /// TODO Edit XML Comment Template for PlcTagTableConstant
    public class PlcTagTableConstant : Member
    {

        /// <summary>The constant value</summary>
        /// TODO Edit XML Comment Template for constantValue
        private string _constantValue;
        /// <summary>Gets or sets the constant value.</summary>
        /// <value>The constant value.</value>
        /// TODO Edit XML Comment Template for ConstantValue
        public string ConstantValue
        {
            get => _constantValue;
            set => _constantValue = value;
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

            ret += "<SW.ControllerConstant ID=\"" + id++ + "\" CompositionName=\"Constants\">" + Environment.NewLine;
            ret += "<AttributeList>" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(MemberName) + "</Name>" + Environment.NewLine;
            ret += "<Value>" + AdjustNames.AdjustXmlStrings(ConstantValue) + "</Value>" + Environment.NewLine;
            ret += "</AttributeList>" + Environment.NewLine;
            ret += "<LinkList>" + Environment.NewLine;
            ret += "<DataType TargetID=\"@OpenLink\">" + Environment.NewLine;
            ret += "<Name>" + AdjustNames.AdjustXmlStrings(MemberDatatype) + "</Name>" + Environment.NewLine;
            ret += "</DataType>" + Environment.NewLine;
            ret += "</LinkList>" + Environment.NewLine;
            ret += "<ObjectList>" + Environment.NewLine;
            ret += MemberComment.ToString(ref id);
            ret += "</ObjectList>" + Environment.NewLine;
            ret += "</SW.ControllerConstant>" + Environment.NewLine;

            return ret;
        }
    }
}
