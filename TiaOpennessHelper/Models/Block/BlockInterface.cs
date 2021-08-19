using System;
using System.Collections.Generic;

namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    ///
    /// </summary>
    /// TODO Edit XML Comment Template for BlockInterface
    public class BlockInterface
    {
        /// <summary>
        /// List of Interface section which contains also the appropriate Member
        /// </summary>
        /// <value>The interface sections.</value>
        public List<BlockInterfaceSection> InterfaceSections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockInterface"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public BlockInterface()
        {
            InterfaceSections = new List<BlockInterfaceSection>();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        /// TODO Edit XML Comment Template for ToString
        public override string ToString()
        {
            string ret = "";

            ret += "<Interface>" + Environment.NewLine;
            ret += "<Sections xmlns=\"http://www.siemens.com/automation/Openness/SW/Interface/v4\">" + Environment.NewLine;
            foreach (BlockInterfaceSection section in InterfaceSections)
            {
                ret += section.ToString();
            }
            ret += "</Sections>" + Environment.NewLine;
            ret += "</Interface>" + Environment.NewLine;

            return ret;
        }
    }
}