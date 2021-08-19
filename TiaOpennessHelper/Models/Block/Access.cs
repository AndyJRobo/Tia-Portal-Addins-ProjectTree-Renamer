using TiaOpennessHelper.Models.Members;

namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Member" />
    /// TODO Edit XML Comment Template for Access
    public class Access : Member
    {
        /// <summary>
        /// Scope of used Element within Network (LocalVariable, GlobalVariable)
        /// </summary>
        /// <value>The access scope.</value>
        public string AccessScope { get; set; }

        /// <summary>
        /// Symbol Node of XML structure which contains the used Component names
        /// </summary>
        /// <value>The access symbol.</value>
        public Symbol AccessSymbol { get; set; }

        /// <summary>UId used in XML File</summary>
        /// <value>The u identifier.</value>
        public string UId { get; set; }

        #region C´tor

        #endregion
    }
}
