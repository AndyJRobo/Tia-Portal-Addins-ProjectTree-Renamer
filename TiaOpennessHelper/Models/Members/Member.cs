using System;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models.Members
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for Member
    public class Member
    {
        /// <summary>Gets or sets the name of the member.</summary>
        /// <value>The name of the member.</value>
        /// TODO Edit XML Comment Template for MemberName
        public string MemberName { get; set; }
        /// <summary>Gets or sets the member datatype.</summary>
        /// <value>The member datatype.</value>
        /// TODO Edit XML Comment Template for MemberDatatype
        public string MemberDatatype { get; set; }
        /// <summary>Gets or sets the member default value.</summary>
        /// <value>The member default value.</value>
        /// TODO Edit XML Comment Template for MemberDefaultValue
        public string MemberDefaultValue { get; set; }
        /// <summary>Gets or sets the member comment.</summary>
        /// <value>The member comment.</value>
        /// TODO Edit XML Comment Template for MemberComment
        public MultiLanguageText MemberComment { get; set; }

        /// <summary>Initializes a new instance of the <see cref="Member"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="datatype">The datatype.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="comment">The comment.</param>
        /// TODO Edit XML Comment Template for #ctor
        // ReSharper disable once UnusedParameter.Local
        public Member(string name, string datatype, string defaultValue = "", string comment = "")
        {
            MemberName = name;
            MemberDatatype = datatype;
            MemberDefaultValue = defaultValue;

            MemberComment = new MultiLanguageText();
        }

        /// <summary>Initializes a new instance of the <see cref="Member"/> class.</summary>
        /// TODO Edit XML Comment Template for #ctor
        public Member()
        {
            MemberComment = new MultiLanguageText();
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
            if (!string.IsNullOrEmpty(MemberDefaultValue))
                ret += "<StartValue>" + AdjustNames.AdjustXmlStrings(MemberDefaultValue) + "</StartValue>" + Environment.NewLine;
            ret += "</Member>" + Environment.NewLine;

            return ret;
        }
    }
}
