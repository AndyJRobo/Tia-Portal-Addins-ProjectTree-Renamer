using System;
using System.Collections.Generic;
using TiaOpennessHelper.Models.Members;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models
{
    /// <summary>
    ///
    /// </summary>
    /// TODO Edit XML Comment Template for BlockInterfaceSection
    public class BlockInterfaceSection
    {
        /// <summary>List of Member within the BlockInterface</summary>
        /// <value>The interface member.</value>
        public List<Member> InterfaceMember { get; set; }

        /// <summary>Name of the Section within the Interface (Input, Output...)</summary>
        /// <value>The name of the interface section.</value>
        public string InterfaceSectionName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockInterfaceSection"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// TODO Edit XML Comment Template for #ctor
        public BlockInterfaceSection(string name)
        {
            InterfaceMember = new List<Member>();
            InterfaceSectionName = name;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        /// TODO Edit XML Comment Template for ToString
        public override string ToString()
        {
            string ret = "";

            ret += "<Section Name=\"" + AdjustNames.AdjustXmlStrings(InterfaceSectionName) + "\">" + Environment.NewLine;
            foreach (Member member in InterfaceMember)
            {
                ret += member.ToString();
            }
            ret += "</Section>" + Environment.NewLine;

            return ret;
        }
    }
}