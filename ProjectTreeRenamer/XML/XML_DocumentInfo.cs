/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Xml2CSharp
{
	[XmlRoot(ElementName = "Engineering")]
	public class Engineering
	{
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
	}

	[XmlRoot(ElementName = "Product")]
	public class Product
	{
		[XmlElement(ElementName = "DisplayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "DisplayVersion")]
		public string DisplayVersion { get; set; }
	}

	[XmlRoot(ElementName = "OptionPackage")]
	public class OptionPackage
	{
		[XmlElement(ElementName = "DisplayName")]
		public string DisplayName { get; set; }
		[XmlElement(ElementName = "DisplayVersion")]
		public string DisplayVersion { get; set; }
	}

	[XmlRoot(ElementName = "InstalledProducts")]
	public class InstalledProducts
	{
		[XmlElement(ElementName = "Product")]
		public List<Product> Product { get; set; }
		[XmlElement(ElementName = "OptionPackage")]
		public List<OptionPackage> OptionPackage { get; set; }
	}

	[XmlRoot(ElementName = "DocumentInfo")]
	public class DocumentInfo
	{
		[XmlElement(ElementName = "Created")]
		public string Created { get; set; }
		[XmlElement(ElementName = "ExportSetting")]
		public string ExportSetting { get; set; }
		[XmlElement(ElementName = "InstalledProducts")]
		public InstalledProducts InstalledProducts { get; set; }
	}
}
