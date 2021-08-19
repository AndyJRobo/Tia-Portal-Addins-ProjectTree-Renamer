using System.Collections.Generic;

namespace TiaOpennessHelper.Models.Block
{
    /// <summary>
    /// 
    /// </summary>
    /// TODO Edit XML Comment Template for Symbol
    public class Symbol
    {
        /// <summary>
        /// List of Components within the Symbol XML Node (e.g. Data.Member1.Member1_1)
        /// </summary>
        /// <value>The components.</value>
        public List<string> Components { get; set; }

        /// <summary>Access Modfifier of used PLC Tag (e.g.":P")</summary>
        /// <value>The simple access modifier.</value>
        public string SimpleAccessModifier { get; set; }

        /// <summary>Initializes a new instance of the <see cref="Symbol"/> class.</summary>
        /// TODO Edit XML Comment Template for #ctor
        public Symbol()
        {
            Components = new List<string>();
        }


        /// <summary>gives a Symbol as a String (e.g. "Motor".Start)</summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string tmp = string.Empty;

            if (Components != null)
            {
                tmp = string.Format("\"{0}\"", Components[0]);

                for (int i = 1; i < Components.Count; i++)
                {
                    tmp += "." + Components[i];
                }

            }

            return tmp;
        }


    }
}
