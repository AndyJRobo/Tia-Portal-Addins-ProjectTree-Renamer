/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace Xml2CSharp
{
	[XmlRoot(ElementName = "SW.Blocks.FB")]
	public class FB
	{
		[XmlElement(ElementName = "AttributeList")]
		public AttributeList2 AttributeList2 { get; set; }
		[XmlElement(ElementName = "ObjectList")]
		public ObjectList ObjectList { get; set; }
		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }
	}
}