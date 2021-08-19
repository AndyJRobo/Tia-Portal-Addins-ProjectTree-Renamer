using ProjectTreeRenamer.Visitors;
using Siemens.Engineering;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using System;
using System.IO;

namespace ProjectTreeRenamer
{
    public class PlcBlockProxy : IElement
    {
        //private BlockContents _blockContents;
        //private BlockInformation _blockInformation;
        public PlcBlockProxy(PlcBlock plcBlock)
        {
            PlcBlock = plcBlock;
        }

        public PlcBlockProxy(PlcBlock plcBlock, PlcBlockUserGroupProxy parent)
        {
            PlcBlock = plcBlock;
            Parent = parent;
        }

        private PlcBlock PlcBlock { get; }
        public PlcBlockUserGroupProxy Parent { get; set; }
        public FileInfo FileInfo { get; set;  }

        public string getPlcBlockName()
        {
            if (PlcBlock is null)
                return "";
            else
                return PlcBlock.Name;
        }

        public Type getPlcBlockType()
        {
            if (PlcBlock is null)
                return null;
            else
                return PlcBlock.GetType();
        }

        public bool ExportPlcBlock(ExportOptions ExportOptions)
        {
            if (PlcBlock is null)
                return false;
            else
            {
                PlcBlock.Export(FileInfo, ExportOptions);
                return true;
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal bool updatePlcBlockName(string find, string replace)
        {
            if (PlcBlock is null)
                return false;
            else
            {
                PlcBlock.Name = PlcBlock.Name.Replace(find, replace);
                return true;
            }
        }

        //public string Name { get; private set; }
        //public PlcBlock PlcBlock { get; }

        //internal void SetName(string name)
        //{
        //    Name = name;
        //}

        //public void GetBlockInformation()
        //{
        //    //XmlParser xmlParser = new XmlParser(_path);
        //    //_blockInformation = xmlParser.Parse();
        //    //using (Form owner = Util.GetForegroundWindow())
        //    //{
        //    //    MessageBox.Show(owner, "Get XML Information: " + System.Environment.NewLine + "Name: " + _blockInformation.ToString());
        //    //}
        //}

        //private BlockContents GetBlockContents(ExclusiveAccess exclusiveAccess)
        //{
        //    if (_path != null)
        //    {
        //        return new BlockContents(_path, exclusiveAccess);
        //    }
        //    else
        //    {
        //        throw new NullReferenceException("String _path in Block is null");
        //    }
        //    //_blockContents = new XmlDocument();

        //    //_ns = new XmlNamespaceManager(_blockContents.NameTable);
        //    //_ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v4");
        //    //_ns.AddNamespace("siemensNetworks", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v1");

        //    ////Load Xml File with fileName into memory
        //    //_blockContents.Load(_path);
        //    ////get root node of xml file
        //    //RootNode = _blockContents.DocumentElement;
        //}

        //public void RenameBlockContents(string find, string replace, ExclusiveAccess exclusiveAccess)
        //{
        //    //TODO - move XML operations to its own file
        //    //TODO - Add renaming of code as well as members - updating members doesnt update all the references of the members like using TIA portal UI.

        //    exclusiveAccess.Text = "Renaming Block Contents: " + Name;
        //    _blockContents = _blockContents == null ? GetBlockContents(exclusiveAccess) : _blockContents;
        //    _blockContents.RenameBlockContents(find, replace);

        //    //GetBlockContents();
        //    ////Get Node list of Sections
        //    //XmlNodeList sections = RootNode.SelectNodes("//Interface/SI:Sections/SI:Section", _ns);
        //    ////For Each Memeber in section - update Name with find and replace
        //    //foreach (XmlNode section in sections)
        //    //{
        //    //    //continue if Block is graph block and have section with name="Base"(invisible for User in TIA Portal)
        //    //    if (section.Attributes != null && section.Attributes["Name"].Value.Equals("Base"))
        //    //    {
        //    //        continue;
        //    //    }

        //    //    //list of Member within each section
        //    //    var members = section.ChildNodes;
        //    //    foreach (XmlNode member in members)
        //    //    {
        //    //        if (member.Attributes != null && member.Attributes["Datatype"].Value.Equals("Struct"))
        //    //        {
        //    //            //Get Struct Members
        //    //            RenameStructMemebers(member, find, replace);
        //    //        }
        //    //        RenameMemberName(member, find, replace);
        //    //    }
        //    //}
        //    //_blockContents.Save(_path);
        //}

        //private void RenameStructMemebers(XmlNode member, string find, string replace)
        //{
        //    XmlNodeList childNodes = member.ChildNodes;
        //    foreach (XmlNode node in childNodes)
        //    {
        //        if (node.Name.Equals("Member"))
        //        {
        //            if (node.Attributes != null && node.Attributes["Datatype"].Value.Equals("Struct"))
        //            {
        //                RenameStructMemebers(node, find, replace);
        //            }
        //            RenameMemberName(node, find, replace);
        //        }
        //    }
        //}

        //private void RenameMemberName(XmlNode member, string find, string replace)
        //{
        //    XmlAttribute nameAttribute = member.Attributes["Name"];
        //    if (nameAttribute != null)
        //    {
        //        // if yes - read its current value
        //        string currentValue = nameAttribute.Value;
        //        string replaceValue = currentValue.Replace(find, replace);
        //        nameAttribute.Value = replaceValue;
        //    }
        //}

        //public void RenameContents(string find, string replace, ExclusiveAccess exclusiveAccess)
        //{
        //    exclusiveAccess.Text = "Renaming Block Contents: " + Name;
        //    XmlParser xmlParser = new XmlParser(_path);
        //    _blockInformation = xmlParser.Parse();
        //    _blockInformation.Name = _blockInformation.Name.Replace(find, replace);
        //    for (int i = 0; i < _blockInformation.Interface.InterfaceSections.Count; i++)
        //    {
        //        for (int j = 0; j < _blockInformation.Interface.InterfaceSections[i].InterfaceMember.Count; j++)
        //        {
        //            _blockInformation.Interface.InterfaceSections[i].InterfaceMember[j].MemberName = _blockInformation.Interface.InterfaceSections[i].InterfaceMember[j].MemberName.Replace(find, replace);
        //        }
        //    }
        //    xmlParser.UpdateDocument(_blockInformation);
        //}
    }
}