using System;
using TiaOpennessHelper.Enums;

namespace TiaOpennessHelper.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IComparable" />
    /// TODO Edit XML Comment Template for XmlInformation
    public abstract class XmlInformation : IComparable
    {
        /// <summary>The name</summary>
        /// TODO Edit XML Comment Template for name
        private string _name;
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        /// TODO Edit XML Comment Template for Name
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>The XML type</summary>
        /// TODO Edit XML Comment Template for xmlType
        private TiaXmlType _xmlType;
        /// <summary>Gets or sets the type of the XML.</summary>
        /// <value>The type of the XML.</value>
        /// TODO Edit XML Comment Template for XmlType
        public TiaXmlType XmlType
        {
            get => _xmlType;
            set => _xmlType = value;
        }




        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        /// <exception cref="System.ArgumentException">Object is not XmlInformation</exception>
        /// TODO Edit XML Comment Template for CompareTo
        public int CompareTo(object obj)
        {
            XmlInformation xmlInfo = obj as XmlInformation;

            if (xmlInfo == null)
            {
                throw new ArgumentException("Object is not XmlInformation");

            }

            return XmlType.CompareTo(xmlInfo.XmlType);
        }
    }
}
