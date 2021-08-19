using System;
using System.Collections.Generic;
using TiaOpennessHelper.Utils;

namespace TiaOpennessHelper.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for MultiLanguageText
    public class MultiLanguageText
    {
        /* ##### MultiLanguageText Interface Comment #####
         * <Comment>
                <MultiLanguageText Lang="en-US">TEXT</MultiLanguageText>
                <MultiLanguageText Lang="de-DE">TEXT</MultiLanguageText>
                <MultiLanguageText Lang="fr-FR">TEXT</MultiLanguageText>
         * </Comment>
         */

        /* ##### MultilingualText #####
         <MultilingualText ID="5" CompositionName="Title">
            <AttributeList>
                <TextItems>
                    <Value lang="en-US">TEXT</Value>
                    <Value lang="de-DE">TEXT</Value>
                    <Value lang="sv-SE">TEXT</Value>
                    <Value lang="fr-FR">TEXT</Value>
                </TextItems>
            </AttributeList>
          </MultilingualText>         
         */

        /// <summary>Dictionary that contains all defined languages of a text item</summary>
        /// <value>The multi language text items.</value>
        public Dictionary<string, string> MultiLanguageTextItems { get; set; }

        /// <summary>
        /// CompositionName within the xml structure (InterfaceMember Comment = string.Empty)
        /// </summary>
        /// <value>The Composition name in XML.</value>
        public string CompositionNameInXml { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLanguageText"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public MultiLanguageText()
        {
            MultiLanguageTextItems = new Dictionary<string, string>();

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
            if (CompositionNameInXml != null)
            {
                if (CompositionNameInXml.Equals(""))
                {
                    ret += "<Comment>" + Environment.NewLine;
                    foreach (KeyValuePair<string, string> entry in MultiLanguageTextItems)
                    {
                        ret += "<MultiLanguageText Lang=\"" + AdjustNames.AdjustXmlStrings(entry.Key) + "\">" + AdjustNames.AdjustXmlStrings(entry.Value) + "</MultiLanguageText>" + Environment.NewLine;
                    }
                    ret += "</Comment>" + Environment.NewLine;
                }

                else if (CompositionNameInXml.Equals("Comment") || CompositionNameInXml.Equals("Title"))
                {

                    ret += "<MultilingualText ID= \"" + id++ + "\"" + " CompositionName=\"" + AdjustNames.AdjustXmlStrings(CompositionNameInXml) + "\">" + Environment.NewLine;
                    ret += "<AttributeList>" + Environment.NewLine;
                    ret += "<TextItems>" + Environment.NewLine;

                    foreach (KeyValuePair<string, string> entry in MultiLanguageTextItems)
                    {
                        ret += "<Value lang=\"" + AdjustNames.AdjustXmlStrings(entry.Key) + "\">" + AdjustNames.AdjustXmlStrings(entry.Value) + "</Value>" + Environment.NewLine;
                    }

                    ret += "</TextItems>" + Environment.NewLine;
                    ret += "</AttributeList>" + Environment.NewLine;
                    ret += "</MultilingualText>" + Environment.NewLine;
                }
            }

            return ret;
        }
    }
}
