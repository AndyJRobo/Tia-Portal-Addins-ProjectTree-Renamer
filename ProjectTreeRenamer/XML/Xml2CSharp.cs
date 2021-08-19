/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace Xml2CSharp
{

	[XmlRoot(ElementName = "BooleanAttribute", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
	public class BooleanAttribute
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "SystemDefined")]
		public string SystemDefined { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "AttributeList", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
	public class AttributeList
	{
		[XmlElement(ElementName = "BooleanAttribute", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
		public List<BooleanAttribute> BooleanAttribute { get; set; }
	}

	[XmlRoot(ElementName = "Member", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
	public class Member
	{
		[XmlElement(ElementName = "AttributeList", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
		public AttributeList AttributeList { get; set; }
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "Datatype")]
		public string Datatype { get; set; }
		[XmlAttribute(AttributeName = "Remanence")]
		public string Remanence { get; set; }
		[XmlAttribute(AttributeName = "Accessibility")]
		public string Accessibility { get; set; }
		[XmlElement(ElementName = "Sections", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
		public Sections Sections { get; set; }
	}

	[XmlRoot(ElementName = "Section", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
	public class Section
	{
		[XmlElement(ElementName = "Member", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
		public List<Member> Member { get; set; }
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "Sections", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
	public class Sections
	{
		[XmlElement(ElementName = "Section", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
		public List<Section> Section { get; set; }
	}

	[XmlRoot(ElementName = "Interface")]
	public class Interface
	{
		[XmlElement(ElementName = "Sections", Namespace = "http://www.siemens.com/automation/Openness/SW/Interface/v4")]
		public Sections Sections { get; set; }
	}

	[XmlRoot(ElementName = "AttributeList")]
	public class AttributeList2
	{
		[XmlElement(ElementName = "AutoNumber")]
		public string AutoNumber { get; set; }
		[XmlElement(ElementName = "HeaderAuthor")]
		public string HeaderAuthor { get; set; }
		[XmlElement(ElementName = "HeaderFamily")]
		public string HeaderFamily { get; set; }
		[XmlElement(ElementName = "HeaderName")]
		public string HeaderName { get; set; }
		[XmlElement(ElementName = "HeaderVersion")]
		public string HeaderVersion { get; set; }
		[XmlElement(ElementName = "Interface")]
		public Interface Interface { get; set; }
		[XmlElement(ElementName = "IsIECCheckEnabled")]
		public string IsIECCheckEnabled { get; set; }
		[XmlElement(ElementName = "IsRetainMemResEnabled")]
		public string IsRetainMemResEnabled { get; set; }
		[XmlElement(ElementName = "MemoryLayout")]
		public string MemoryLayout { get; set; }
		[XmlElement(ElementName = "MemoryReserve")]
		public string MemoryReserve { get; set; }
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Number")]
		public string Number { get; set; }
		[XmlElement(ElementName = "ProgrammingLanguage")]
		public string ProgrammingLanguage { get; set; }
		[XmlElement(ElementName = "SetENOAutomatically")]
		public string SetENOAutomatically { get; set; }
		[XmlElement(ElementName = "UDABlockProperties")]
		public string UDABlockProperties { get; set; }
		[XmlElement(ElementName = "UDAEnableTagReadback")]
		public string UDAEnableTagReadback { get; set; }
		[XmlElement(ElementName = "Culture")]
		public string Culture { get; set; }
		[XmlElement(ElementName = "Text")]
		public string Text { get; set; }
		[XmlElement(ElementName = "NetworkSource")]
		public NetworkSource NetworkSource { get; set; }
	}

	[XmlRoot(ElementName = "MultilingualTextItem")]
	public class MultilingualTextItem
	{
		[XmlElement(ElementName = "AttributeList")]
		public AttributeList2 AttributeList2 { get; set; }
		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }
		[XmlAttribute(AttributeName = "CompositionName")]
		public string CompositionName { get; set; }
	}

	[XmlRoot(ElementName = "ObjectList")]
	public class ObjectList
	{
		[XmlElement(ElementName = "MultilingualTextItem")]
		public MultilingualTextItem MultilingualTextItem { get; set; }
		[XmlElement(ElementName = "MultilingualText")]
		public List<MultilingualText> MultilingualText { get; set; }
	}

	[XmlRoot(ElementName = "MultilingualText")]
	public class MultilingualText
	{
		[XmlElement(ElementName = "ObjectList")]
		public ObjectList ObjectList { get; set; }
		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }
		[XmlAttribute(AttributeName = "CompositionName")]
		public string CompositionName { get; set; }
	}

	[XmlRoot(ElementName = "Component", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Component
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "Symbol", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Symbol
	{
		[XmlElement(ElementName = "Component", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public List<Component> Component { get; set; }
	}

	[XmlRoot(ElementName = "Access", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Access
	{
		[XmlElement(ElementName = "Symbol", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public Symbol Symbol { get; set; }
		[XmlAttribute(AttributeName = "Scope")]
		public string Scope { get; set; }
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
	}

	[XmlRoot(ElementName = "Part", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Part
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
		[XmlElement(ElementName = "TemplateValue", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public TemplateValue TemplateValue { get; set; }
		[XmlAttribute(AttributeName = "DisabledENO")]
		public string DisabledENO { get; set; }
	}

	[XmlRoot(ElementName = "Instance", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Instance
	{
		[XmlElement(ElementName = "Component", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public Component Component { get; set; }
		[XmlAttribute(AttributeName = "Scope")]
		public string Scope { get; set; }
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
	}

	[XmlRoot(ElementName = "Parameter", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Parameter
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "Section")]
		public string Section { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName = "CallInfo", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class CallInfo
	{
		[XmlElement(ElementName = "Instance", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public Instance Instance { get; set; }
		[XmlElement(ElementName = "Parameter", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public List<Parameter> Parameter { get; set; }
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "BlockType")]
		public string BlockType { get; set; }
	}

	[XmlRoot(ElementName = "Call", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Call
	{
		[XmlElement(ElementName = "CallInfo", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public CallInfo CallInfo { get; set; }
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
	}

	[XmlRoot(ElementName = "Parts", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Parts
	{
		[XmlElement(ElementName = "Access", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public List<Access> Access { get; set; }
		[XmlElement(ElementName = "Part", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public List<Part> Part { get; set; }
		[XmlElement(ElementName = "Call", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public Call Call { get; set; }
	}

	[XmlRoot(ElementName = "NameCon", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class NameCon
	{
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "Wire", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Wire
	{
		[XmlElement(ElementName = "Powerrail", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public string Powerrail { get; set; }
		[XmlElement(ElementName = "NameCon", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public List<NameCon> NameCon { get; set; }
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
		[XmlElement(ElementName = "IdentCon", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public IdentCon IdentCon { get; set; }
	}

	[XmlRoot(ElementName = "IdentCon", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class IdentCon
	{
		[XmlAttribute(AttributeName = "UId")]
		public string UId { get; set; }
	}

	[XmlRoot(ElementName = "Wires", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class Wires
	{
		[XmlElement(ElementName = "Wire", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public List<Wire> Wire { get; set; }
	}

	[XmlRoot(ElementName = "FlgNet", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class FlgNet
	{
		[XmlElement(ElementName = "Parts", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public Parts Parts { get; set; }
		[XmlElement(ElementName = "Wires", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public Wires Wires { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "NetworkSource")]
	public class NetworkSource
	{
		[XmlElement(ElementName = "FlgNet", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
		public FlgNet FlgNet { get; set; }
	}

	[XmlRoot(ElementName = "SW.Blocks.CompileUnit")]
	public class CompileUnit {
		[XmlElement(ElementName = "AttributeList")]
		public AttributeList2 AttributeList2 { get; set; }
		[XmlElement(ElementName = "ObjectList")]
		public ObjectList ObjectList { get; set; }
		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }
		[XmlAttribute(AttributeName = "CompositionName")]
		public string CompositionName { get; set; }
	}

	[XmlRoot(ElementName = "TemplateValue", Namespace = "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4")]
	public class TemplateValue
	{
		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
		[XmlText]
		public string Text { get; set; }
	}



	[XmlRoot(ElementName = "Document")]
	public class Document
	{
		[XmlElement(ElementName = "Engineering")]
		public Engineering Engineering { get; set; }
		[XmlElement(ElementName = "DocumentInfo")]
		public DocumentInfo DocumentInfo { get; set; }
		[XmlElement(ElementName = "SW.Blocks.FB")]
		public FB FB { get; set; }
	}

}
