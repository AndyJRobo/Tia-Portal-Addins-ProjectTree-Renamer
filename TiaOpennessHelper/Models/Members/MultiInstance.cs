using System;
using System.Collections.Generic;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.Members
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Member" />
    /// TODO Edit XML Comment Template for MultiInstance
    public class MultiInstance : Member
    {
        /// <summary>Gets or sets the interface sections.</summary>
        /// <value>The interface sections.</value>
        /// TODO Edit XML Comment Template for interfaceSections
        public List<BlockInterfaceSection> InterfaceSections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiInstance"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="comment">The comment.</param>
        /// TODO Edit XML Comment Template for #ctor
        // ReSharper disable once UnusedParameter.Local
        public MultiInstance(string name, string dataType, string defaultValue = "", string comment = "")
        {
            InterfaceSections = new List<BlockInterfaceSection>();
            MemberName = name;
            MemberDatatype = dataType;
            MemberDefaultValue = defaultValue;
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

            ret += "<Member Name=\"" + AdjustNames.AdjustXmlStrings(MemberName) + "\" Datatype=\"" + AdjustNames.AdjustXmlStrings(MemberDatatype) + "\">" + Environment.NewLine;
            if (MemberComment.CompositionNameInXml != null)
                ret += MemberComment.ToString(ref id);

            ret += "<Sections>" + Environment.NewLine;
            foreach (BlockInterfaceSection section in InterfaceSections)
            {
                ret += "<Section name=\"" + InterfaceSections[0].InterfaceSectionName + "\">" + Environment.NewLine;
                foreach (Member member in section.InterfaceMember)
                    ret += member.ToString();
                ret += "</Section>" + Environment.NewLine;
            }
            ret += "</Sections>" + Environment.NewLine;

            if (!string.IsNullOrEmpty(MemberDefaultValue))
                ret += "<StartValue>" + AdjustNames.AdjustXmlStrings(MemberDefaultValue) + "</StartValue>" + Environment.NewLine;
            ret += "</Member>" + Environment.NewLine;

            return ret;
        }
    }
}
