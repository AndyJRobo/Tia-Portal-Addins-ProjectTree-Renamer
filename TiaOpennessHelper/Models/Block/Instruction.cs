namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for Instruction
    public class Instruction
    {
        /// <summary>Name of instruction within the xml structure</summary>
        /// <value>The name of the instruction.</value>
        public string InstructionName { get; set; }

        /// <summary>UId of the instruction within the xml structure</summary>
        /// <value>The u identifier.</value>
        public string UId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Instruction"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uId">The u identifier.</param>
        /// TODO Edit XML Comment Template for #ctor
        public Instruction(string name, string uId)
        {
            InstructionName = name;
            UId = uId;
        }
    }
}
