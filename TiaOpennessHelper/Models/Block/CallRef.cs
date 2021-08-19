namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for CallRef
    public class CallRef
    {
        /// <summary>Name of the called Block</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Type of the called Block (FC/FB)</summary>
        /// <value>The type of the block.</value>
        public string BlockType { get; set; }

        /// <summary>Type of Call (FunctionCall/GlobalCall)</summary>
        /// <value>The type of the call.</value>
        public string CallType { get; set; }

        /// <summary>UId of the BlockCall</summary>
        /// <value>The u identifier.</value>
        public string UId { get; set; }
    }
}
