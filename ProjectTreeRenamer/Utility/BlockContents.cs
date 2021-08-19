using Siemens.Engineering;
using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace ProjectTreeRenamer.Utility
{
    public class BlockContents
    {
        private XmlDocument _document;
        private XmlNamespaceManager _ns;
        private XPathNavigator navigator; 
        private readonly string _path;
        private ExclusiveAccess _exclusiveAccess;

        /// <summary>The root node</summary>
        /// TODO Edit XML Comment Template for rootNode
        private XmlNode _rootNode;

        /// <summary>Gets or sets the root node.</summary>
        /// <value>The root node.</value>
        /// TODO Edit XML Comment Template for RootNode
        public XmlNode RootNode
        {
            get => _rootNode;
            set => _rootNode = value;
        }

        public BlockContents(string path, ExclusiveAccess exclusiveAccess)
        {
            _path = path;
            GetBlockContents();
            _exclusiveAccess = exclusiveAccess;
        }

        private void GetBlockContents()
        {
            _document = new XmlDocument();
            navigator = _document.CreateNavigator();
            _ns = new XmlNamespaceManager(_document.NameTable);
            _ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v4");
            _ns.AddNamespace("SLN", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v4");
            _ns.AddNamespace("SSTN", "http://www.siemens.com/automation/Openness/SW/NetworkSource/StructuredText/v3");

            //Load Xml File with fileName into memory
            _document.Load(_path);
            //get root node of xml file
            RootNode = _document.DocumentElement;
        }

        public void RenameBlockContents(string find, string replace)
        {
            //Rename Sections
            RenameBlockInterfaceItems(find, replace);

            //Rename Objects
            RenameObjects(find, replace);

            _document.Save(_path);
        }

        private void RenameObjects(string find, string replace)
        {
            RenameLadder(find, replace);
            RenameStructuredText(find, replace);
        }

        private void RenameStructuredText(string find, string replace)
        {
            XPathExpression query = navigator.Compile("//SW.Blocks.CompileUnit//SSTN:StructuredText//SSTN:Component[1]");
            query.SetContext(_ns);
            XPathNodeIterator nodes = navigator.Select(query);

            while (nodes.MoveNext())
            {
                XPathNavigator nav = nodes.Current;
                nav.MoveToAttribute("Name", String.Empty);
                nav.SetValue(nav.Value.Replace(find, replace));
            }
        }

        private void RenameLadder(string find, string replace)
        {
            XPathExpression query = navigator.Compile("//SW.Blocks.CompileUnit//SLN:FlgNet//SLN:Component[1]");
            query.SetContext(_ns);
            XPathNodeIterator nodes = navigator.Select(query);

            while (nodes.MoveNext())
            {
                XPathNavigator nav = nodes.Current;
                nav.MoveToAttribute("Name", String.Empty);
                nav.SetValue(nav.Value.Replace(find, replace));
            }
        }

        private void RenameBlockInterfaceItems(string find, string replace)
        {
            XmlNodeList sections = RootNode.SelectNodes("//Interface/SI:Sections/SI:Section", _ns);
            //For Each Memeber in section - update Name with find and replace
            foreach (XmlNode section in sections)
            {
                //continue if Block is graph block and have section with name="Base"(invisible for User in TIA Portal)
                if (section.Attributes != null && section.Attributes["Name"].Value.Equals("Base"))
                {
                    continue;
                }

                //list of Member within each section
                XmlNodeList members = section.ChildNodes;
                foreach (XmlNode member in members)
                {
                    if (member.Attributes != null && member.Attributes["Datatype"].Value.Equals("Struct"))
                    {
                        //Get Struct Members
                        RenameStructMembers(member, find, replace);
                    }
                    RenameXMLNodeName(member, find, replace);
                }
            }
        }

        private void RenameStructMembers(XmlNode member, string find, string replace)
        {
            XmlNodeList childNodes = member.ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                if (node.Name.Equals("Member"))
                {
                    if (node.Attributes != null && node.Attributes["Datatype"].Value.Equals("Struct"))
                    {
                        RenameStructMembers(node, find, replace);
                    }
                    RenameXMLNodeName(node, find, replace);
                }
            }
        }

        private void RenameXMLNodeName(XmlNode node, string find, string replace)
        {
            XmlAttribute nameAttribute = node.Attributes["Name"];
            UpdateXMLAttribute(nameAttribute, find, replace);
        }

        private void UpdateXMLAttribute(XmlAttribute attribute, string find, string replace)
        {
            attribute.Value = attribute.Value.Replace(find, replace);
        }
    }
}