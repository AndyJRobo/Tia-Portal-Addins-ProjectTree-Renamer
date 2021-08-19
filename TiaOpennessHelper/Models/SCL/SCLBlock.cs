using System.Collections.Generic;
using TiaOpennessHelper.Models.Block;
using TiaOpennessHelper.Models.Members;

namespace TiaOpennessHelper.Models.SCL
{
    /// <summary>
    /// Representation of a scl source file.
    /// </summary>
    /// <seealso cref="BlockInformation" />
    public class SclBlock : BlockInformation
    {
        #region Properties
        /// <summary>String containing the program code of the SCLBlock</summary>
        /// <value>The code of the block.</value>
        public string BlockCode { get; set; }
        #endregion

        #region c'tor
        /// <summary>
        /// Initializes a new instance of the <see cref="SclBlock"/> class.
        /// </summary>
        public SclBlock()
        {
            BlockLanguage = "SCL";
            BlockCode = "";
        }
        #endregion

        #region public methods
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            string ret;
            if (BlockType == "FB")
            {
                ret = "FUNCTION_BLOCK \"";
                ret += Name + "\"\r\n";
            }
            else if (BlockType == "FC")
            {
                ret = "FUNCTION \"";
                ret += Name + "\" : ";
                foreach (BlockInterfaceSection section in BlockInterface.InterfaceSections)
                {
                    if (section.InterfaceSectionName == "Return")
                    {
                        ret += section.InterfaceMember[0].MemberDatatype;
                    }
                }
                ret += "\r\n";
            }
            else
            {
                ret = "ORGANIZATION_BLOCK \"";
                ret += Name + "\"\r\n";
            }
            if (BlockTitle.MultiLanguageTextItems.Count != 0)
                ret += "TITLE = " + BlockTitle.MultiLanguageTextItems["en-US"] + "\r\n";
            if (BlockMemoryLayout != "")
                ret += "{ S7_Optimized_Access := '" + BlockMemoryLayout + "' }\r\n";
            if (BlockAuthor != "")
                ret += "AUTHOR : " + BlockAuthor + "\r\n";
            if (BlockFamily != "")
                ret += "FAMILY : " + BlockFamily + "\r\n";
            if (BlockUserId != "")
                ret += "NAME : '" + BlockUserId + "'\r\n";
            if (BlockVersion != "")
                ret += "VERSION : " + BlockVersion + "\r\n";
            if (BlockComment.MultiLanguageTextItems.Count != 0)
                ret += "// " + BlockComment.MultiLanguageTextItems["en-US"] + "\r\n";

            string indent = "   ";

            foreach (BlockInterfaceSection section in BlockInterface.InterfaceSections)
            {
                switch (section.InterfaceSectionName)
                {
                    case "Input":
                        ret += indent + "VAR_INPUT \r\n";
                        ret += PrintList(section.InterfaceMember, indent + "   ");
                        ret += indent + "END_VAR\r\n\r\n";
                        break;
                    case "Output":
                        ret += indent + "VAR_OUTPUT \r\n";
                        ret += PrintList(section.InterfaceMember, indent + "   ");
                        ret += indent + "END_VAR\r\n\r\n";
                        break;
                    case "InOut":
                        ret += indent + "VAR_IN_OUT \r\n";
                        ret += PrintList(section.InterfaceMember, indent + "   ");
                        ret += indent + "END_VAR\r\n\r\n";
                        break;
                    case "Static":
                        ret += indent + "VAR \r\n";
                        ret += PrintList(section.InterfaceMember, indent + "   ");
                        ret += indent + "END_VAR\r\n\r\n";
                        break;
                    case "Temp":
                        ret += indent + "VAR_TEMP \r\n";
                        ret += PrintList(section.InterfaceMember, indent + "   ");
                        ret += indent + "END_VAR\r\n\r\n";
                        break;
                    case "Constant":
                        ret += indent + "VAR CONSTANT \r\n";
                        ret += PrintList(section.InterfaceMember, indent + "   ");
                        ret += indent + "END_VAR\r\n\r\n";
                        break;
                }
            }

            ret += "\r\nBEGIN\r\n";
            ret += BlockCode;
            if (BlockType == "FB")
            {
                ret += "END_FUNCTION_BLOCK\r\n\r\n";
            }
            else if (BlockType == "FC")
            {
                ret += "END_FUNCTION\r\n\r\n";
            }
            else
            {
                ret += "END_ORGANIZATION_BLOCK\r\n\r\n";
            }

            return ret;
        }
        #endregion

        #region private methods
        /// <summary>Returns a string representation of all the Members in list</summary>
        /// <param name="list">The list.</param>
        /// <param name="indent">The indent.</param>
        /// <returns>String</returns>
        private string PrintList(List<Member> list, string indent)
        {
            string ret = "";

            foreach (Member member in list)
            {
                ret += indent + member.MemberName + " : " + member.MemberDatatype;
                if (member is Struct)
                {
                    if (member.MemberComment.MultiLanguageTextItems.Count != 0)
                        ret += "   // " + member.MemberComment.MultiLanguageTextItems["en-US"];
                    ret += "\r\n";
                    ret += PrintList((member as Struct).NestedMembers, indent + "   ");
                    ret += indent + "END_STRUCT;\r\n";
                }
                else
                {
                    if (member.MemberDefaultValue != "")
                        ret += " := " + member.MemberDefaultValue;
                    ret += ";";
                    if (member.MemberComment.MultiLanguageTextItems.Count != 0)
                        ret += "   // " + member.MemberComment.MultiLanguageTextItems["en-US"];
                    ret += "\r\n";
                }
            }

            return ret;
        }
        #endregion
    }
}
