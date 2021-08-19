using System.Collections.Generic;

namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for Network
    public class Network
    {
        /// <summary>List of Access (used Variables in Network)</summary>
        /// <value>The network access.</value>
        public List<Access> NetworkAccess { get; set; }

        /// <summary>List of used Instructions with a Network</summary>
        /// <value>The network instructions.</value>
        public List<Instruction> NetworkInstructions { get; set; }

        /// <summary>List of used BlockCalls within a Network</summary>
        /// <value>The network calls.</value>
        public List<CallRef> NetworkCalls { get; set; }

        /// <summary>title of Network</summary>
        /// <value>The network title.</value>
        public MultiLanguageText NetworkTitle { get; set; }

        /// <summary>comment of Network</summary>
        /// <value>The network comment.</value>
        public MultiLanguageText NetworkComment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// TODO Edit XML Comment Template for #ctor
        public Network()
        {
            NetworkAccess = new List<Access>();
            NetworkInstructions = new List<Instruction>();
            NetworkCalls = new List<CallRef>();

            NetworkTitle = new MultiLanguageText();
            NetworkComment = new MultiLanguageText();

        }
    }
}
