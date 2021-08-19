using System;
using System.Collections.Generic;
using System.Xml;
using TiaOpennessHelper.Enums;
using TiaOpennessHelper.Models;
using TiaOpennessHelper.Models.Block;
using TiaOpennessHelper.Models.ControllerDataType;
using TiaOpennessHelper.Models.ControllerTagTable;
using TiaOpennessHelper.Models.DataBlock;
using TiaOpennessHelper.Models.Members;

namespace TiaOpennessHelper.XMLParser
{
    /// <summary>
    ///
    /// </summary>
    /// TODO Edit XML Comment Template for XmlParser
    public class XmlParser
    {
        #region Fields

        /// <summary>
        /// </summary>
        /// TODO Edit XML Comment Template for ns
        private readonly XmlNamespaceManager _ns;

        #endregion Fields

        #region Properties

        /// <summary>The file name</summary>
        /// TODO Edit XML Comment Template for fileName
        private string _fileName;

        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        /// TODO Edit XML Comment Template for FileName
        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }

        /// <summary>The document</summary>
        /// TODO Edit XML Comment Template for document
        private XmlDocument _document;

        /// <summary>Gets or sets the document.</summary>
        /// <value>The document.</value>
        /// TODO Edit XML Comment Template for Document
        public XmlDocument Document
        {
            get => _document;
            set => _document = value;
        }

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

        /// <summary>The node</summary>
        /// TODO Edit XML Comment Template for node
        private XmlNode _node;

        /// <summary>Gets or sets the node.</summary>
        /// <value>The node.</value>
        /// TODO Edit XML Comment Template for Node
        public XmlNode Node
        {
            get => _node;
            set => _node = value;
        }

        #endregion Properties

        #region C´tor

        /// <summary>Create Xml Parser</summary>
        /// <param name="fileName">Name of the file.</param>
        public XmlParser(string fileName)
        {
            //create new Xml Document
            Document = new XmlDocument();

            _ns = new XmlNamespaceManager(Document.NameTable);
            _ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v4");
            _ns.AddNamespace("siemensNetworks", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v1");

            //Load Xml File with fileName into memory
            Document.Load(fileName);
            //get root node of xml file
            RootNode = Document.DocumentElement;
        }

        #endregion C´tor

        #region public methods

        /// <summary>Parse XML File into Type 'XmlInformation'</summary>
        /// <returns></returns>
        public XmlInformation Parse()
        {
            XmlInformation xmlInfo = null;
            //Get the node that starts with 'SW.'
            XmlNode xmlTypeNode = RootNode.SelectSingleNode(@"//*[contains(name(),'SW.')]");

            switch (xmlTypeNode?.Name)
            {
                case "SW.Blocks.FB":
                case "SW.Blocks.FC":
                case "SW.Blocks.OB":
                    xmlInfo = ParseSoftwareBlock();
                    break;

                //case "SW.Blocks.DB":
                //    xmlInfo = ParseDataBlock();
                //    break;

                case "SW.Blocks.GlobalDB":
                    xmlInfo = ParseGlobalDataBlock();
                    break;

                case "SW.Blocks.ArrayDB":
                    xmlInfo = ParseArrayDataBlock();
                    break;

                case "SW.Blocks.InstanceDB":
                    xmlInfo = ParseInstanceDataBlock();
                    break;

                case "SW.PlcType":
                    xmlInfo = ParsePlcType();
                    break;

                case "SW.Tags.PlcTagTable":
                    xmlInfo = ParsePlcTagTable();
                    break;
            }

            return xmlInfo;
        }

        #endregion public methods

        #region private methods

        /// <summary>Parse XML of Controller Tag Table</summary>
        /// <returns>XmlInformation</returns>
        private XmlInformation ParsePlcTagTable()
        {
            PlcTagTableInformation tagTableInfo = new PlcTagTableInformation();

            tagTableInfo.XmlType = TiaXmlType.PlcTagTable;
            tagTableInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);

            XmlNodeList listOfConstants = RootNode.SelectNodes("//ObjectList/SW.Tags.PlcConstant");
            XmlNodeList listOfTags = RootNode.SelectNodes("//ObjectList/SW.Tags.PlcTag");

            //get all infos about constants within tagtable

            #region constants infos

            if (listOfConstants != null)
            {
                foreach (XmlNode constantInTable in listOfConstants)
                {
                    PlcTagTableConstant constant = new PlcTagTableConstant();

                    foreach (XmlNode child in constantInTable.ChildNodes)
                    {
                        if (child.Name.Equals("AttributeList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get Name of constant
                                if (node.Name.Equals("Name"))
                                {
                                    constant.MemberName = node.InnerText;
                                }
                                //get value of constant
                                if (node.Name.Equals("Value"))
                                {
                                    constant.ConstantValue = node.InnerText;
                                }
                            }
                        }

                        if (child.Name.Equals("LinkList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get datatype of constant
                                constant.MemberDatatype = node.InnerText;
                            }
                        }

                        if (child.Name.Equals("ObjectList"))
                        {
                            XmlNode comment = child.SelectSingleNode("MultilingualText");
                            constant.MemberComment.CompositionNameInXml = comment?.Attributes?["CompositionName"].Value;

                            GetTitleOrComment(child, constant.MemberComment);
                        }
                    }

                    tagTableInfo.Constants.Add(constant);
                }
            }

            #endregion constants infos

            //get all infos about tags within tagtable

            #region tag infos

            if (listOfTags != null)
            {
                foreach (XmlNode tagsInTable in listOfTags)
                {
                    PlcTagTableTag tag = new PlcTagTableTag();

                    foreach (XmlNode child in tagsInTable.ChildNodes)
                    {
                        if (child.Name.Equals("AttributeList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get Name of constant
                                if (node.Name.Equals("Name"))
                                {
                                    tag.MemberName = node.InnerText;
                                }
                                //get value of constant
                                if (node.Name.Equals("LogicalAddress"))
                                {
                                    tag.Address = node.InnerText;
                                }
                            }
                        }

                        if (child.Name.Equals("LinkList"))
                        {
                            foreach (XmlNode node in child.ChildNodes)
                            {
                                //get datatype of constant
                                tag.MemberDatatype = node.InnerText;
                            }
                        }

                        if (child.Name.Equals("ObjectList"))
                        {
                            XmlNode comment = child.SelectSingleNode("MultilingualText");
                            tag.MemberComment.CompositionNameInXml = comment?.Attributes?["CompositionName"].Value;

                            GetTitleOrComment(child, tag.MemberComment);
                        }
                    }

                    tagTableInfo.Tags.Add(tag);
                }
            }

            #endregion tag infos

            return tagTableInfo;
        }

        /// <summary>Parse XML of Software Block</summary>
        /// <returns>XmlInformation</returns>
        private XmlInformation ParseSoftwareBlock()
        {
            BlockInformation blockInfo = new BlockInformation();

            blockInfo.XmlType = TiaXmlType.Block;
            blockInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);
            blockInfo.BlockNumber = GetMetaInformation(XPathConstants.Xpathblocknumber);
            blockInfo.BlockLanguage = GetMetaInformation(XPathConstants.Xpathblocklanguage);
            blockInfo.BlockType = GetMetaInformation(XPathConstants.Xpathblocktype);
            blockInfo.BlockMemoryLayout = GetMetaInformation(XPathConstants.Xpathblockmemorylayout);
            //blockInfo.BlockAutoNumber = GetMetaInformation(XPathConstants.XpathAutonumber);
            blockInfo.BlockEnableTagReadback = GetMetaInformation(XPathConstants.XpathEnabletagreadback);
            blockInfo.BlockEnableTagReadbackBlockProperties = GetMetaInformation(XPathConstants.XpathEnabletagreadbackblockproperties);
            blockInfo.BlockIsIecCheckEnabled = GetMetaInformation(XPathConstants.XpathIsieccheckenabled);

            blockInfo.BlockAuthor = GetMetaInformation(XPathConstants.Xpathblockauthor);
            blockInfo.BlockTitle = GetBlockTitleOrComment(XPathConstants.Xpathblocktitle);
            blockInfo.BlockComment = GetBlockTitleOrComment(XPathConstants.Xpathblockcomment);
            blockInfo.BlockFamily = GetMetaInformation(XPathConstants.Xpathblockfamily);
            blockInfo.BlockVersion = GetMetaInformation(XPathConstants.Xpathblockversion);
            blockInfo.BlockUserId = GetMetaInformation(XPathConstants.Xpathblockuserid);

            blockInfo.BlockInterface = GetBlockInterfaceInformation();

            blockInfo.BlockNetworks = GetBlockNetworkInformation();

            return blockInfo;
        }

        private XmlInformation ParseInstanceDataBlock()
        {
            DataBlockInformation dataBlockInfo = new DataBlockInformation();
            dataBlockInfo.InstanceOfName = GetMetaInformation(XPathConstants.Xpathinstanceofname);
            dataBlockInfo.InstanceOfType = GetMetaInformation(XPathConstants.Xpathinstanceoftype);
            return ParseDataBlock(dataBlockInfo);
        }

        private XmlInformation ParseArrayDataBlock()
        {
            DataBlockInformation dataBlockInfo = new DataBlockInformation();
            dataBlockInfo.InstanceOfName = string.Empty;
            dataBlockInfo.InstanceOfType = string.Empty;
            return ParseDataBlock(dataBlockInfo);
        }

        private XmlInformation ParseGlobalDataBlock()
        {
            DataBlockInformation dataBlockInfo = new DataBlockInformation();

            dataBlockInfo.InstanceOfName = string.Empty;
            dataBlockInfo.InstanceOfType = string.Empty;

            return ParseDataBlock(dataBlockInfo);
        }

        /// <summary>Parse XML of Data Block</summary>
        /// <returns>XmlInformation</returns>
        private XmlInformation ParseDataBlock(DataBlockInformation dataBlockInfo)
        {
            dataBlockInfo.XmlType = TiaXmlType.DataBlock;
            dataBlockInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);

            dataBlockInfo.BlockNumber = GetMetaInformation(XPathConstants.Xpathblocknumber);
            dataBlockInfo.BlockLanguage = GetMetaInformation(XPathConstants.Xpathblocklanguage);
            dataBlockInfo.BlockType = GetMetaInformation(XPathConstants.Xpathblocktype);
            dataBlockInfo.BlockMemoryLayout = GetMetaInformation(XPathConstants.Xpathblockmemorylayout);
            dataBlockInfo.DownloadWithoutReinit = GetMetaInformation(XPathConstants.XpathDownloadwithoutreinit);
            dataBlockInfo.IsOnlyStoredInLoadMemory = GetMetaInformation(XPathConstants.XpathIsonlystoredinloadmemory);
            dataBlockInfo.IsRetainMemResEnabled = GetMetaInformation(XPathConstants.XpathIsretainmemresenabled);
            dataBlockInfo.IsWriteProtectedInAs = GetMetaInformation(XPathConstants.XpathIswriteprotectedinas);
            dataBlockInfo.MemoryReserve = GetMetaInformation(XPathConstants.XpathMemoryreserve);

            dataBlockInfo.BlockAuthor = GetMetaInformation(XPathConstants.Xpathblockauthor);
            dataBlockInfo.BlockTitle = GetBlockTitleOrComment(XPathConstants.Xpathblocktitle);
            dataBlockInfo.BlockComment = GetBlockTitleOrComment(XPathConstants.Xpathblockcomment);
            dataBlockInfo.BlockFamily = GetMetaInformation(XPathConstants.Xpathblockfamily);
            dataBlockInfo.BlockVersion = GetMetaInformation(XPathConstants.Xpathblockversion);
            dataBlockInfo.BlockUserId = GetMetaInformation(XPathConstants.Xpathblockuserid);

            dataBlockInfo.BlockInterface = GetBlockInterfaceInformation();

            return dataBlockInfo;
        }

        /// <summary>Parse XML of Controller Data type</summary>
        /// <returns></returns>
        private XmlInformation ParsePlcType()
        {
            PlcTypeInformation udtInfo = new PlcTypeInformation();

            udtInfo.Name = GetMetaInformation(XPathConstants.Xpathblockname);
            udtInfo.XmlType = TiaXmlType.PlcType;

            udtInfo.UdtTitle = GetBlockTitleOrComment(XPathConstants.Xpathblocktitle);
            udtInfo.UdtComment = GetBlockTitleOrComment(XPathConstants.Xpathblockcomment);

            udtInfo.DatatypeMember = GetUdtMembers();

            return udtInfo;
        }

        /// <summary>Gets the type of the datablock.</summary>
        /// <returns></returns>
        /// TODO Edit XML Comment Template for GetDatablockType
        //private DatablockType GetDatablockType()
        //{
        //    Node = RootNode.SelectSingleNode(XPathConstants.Xpathdatablocktype);

        //    if (Node != null)
        //    {
        //        var myType = (DatablockType)Enum.Parse(typeof(DatablockType), Node.InnerText);
        //        return myType;
        //    }
        //    return 0;
        //}

        /// <summary>Gets the meta information.</summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns>String</returns>
        /// TODO Edit XML Comment Template for GetMetaInformation
        private string GetMetaInformation(string xpath)
        {
            Node = RootNode.SelectSingleNode(xpath);
            string innerText;

            if (Node != null)
            {
                innerText = Node.InnerText;
            }
            else
            {
                innerText = "";
            }

            return innerText;
        }

        /// <summary>Gets the block interface information.</summary>
        /// <returns>BlockInterface</returns>
        /// TODO Edit XML Comment Template for GetBlockInterfaceInformation
        private BlockInterface GetBlockInterfaceInformation()
        {
            //local BlockInformation Object
            BlockInterface blockInterface = new BlockInterface();

            ParseInterface(RootNode.SelectNodes("//Interface/SI:Sections/SI:Section", _ns), blockInterface.InterfaceSections);

            return blockInterface;
        }

        /// <summary>Parses the interface.</summary>
        /// <param name="listOfSections">The list of sections.</param>
        /// <param name="interfaceSections">The interface sections.</param>
        /// TODO Edit XML Comment Template for ParseInterface
        private void ParseInterface(XmlNodeList listOfSections, List<BlockInterfaceSection> interfaceSections)
        {
            foreach (XmlNode section in listOfSections)
            {
                //continue if Block is graph block and have section with name="Base"(invisible for User in TIA Portal)
                if (section.Attributes != null && section.Attributes["Name"].Value.Equals("Base"))
                {
                    continue;
                }

                //new section within the BlockInterface
                BlockInterfaceSection blockInterfaceSection = new BlockInterfaceSection(section.Attributes?["Name"].Value);

                //add BlockInterface Section
                interfaceSections.Add(blockInterfaceSection);

                //list of Member within each section
                XmlNodeList listOfMember = section.ChildNodes;

                foreach (XmlNode member in listOfMember)
                {
                    Member blockIMember = null;

                    //Struct Member
                    if (member.Attributes != null && member.Attributes["Datatype"].Value.Equals("Struct"))
                    {
                        blockIMember = new Struct(member.Attributes["Name"].Value);
                        GetStructChildNodes(member.ChildNodes, (Struct)blockIMember);
                    }

                    //MultiInstance Member
                    else if (member.Attributes != null && (member.LocalName.Equals("Member") && member.HasChildNodes && member.Attributes["Datatype"].Value.Contains("\"") && !member.Attributes["Datatype"].Value.Contains("Array of")))
                    {
                        foreach (XmlNode child in member.ChildNodes)
                        {
                            if (child.Name.Equals("Sections"))
                            {
                                blockIMember = new MultiInstance(member.Attributes["Name"].Value, member.Attributes["Datatype"].Value);
                                GetMultiInstanceMember(member, (MultiInstance)blockIMember);
                            }
                        }
                    }

                    //normal Member
                    else
                    {
                        blockIMember = new Member(member.Attributes?["Name"].Value, member.Attributes?["Datatype"].Value);
                    }

                    if (blockIMember != null)
                    {
                        GetStartValue(member, blockIMember);
                        GetComment(member, blockIMember);
                        blockInterfaceSection.InterfaceMember.Add(blockIMember);
                    }
                }
            }
        }

        /// <summary>Gets the block network information.</summary>
        /// <returns>List&lt;Network&gt;</returns>
        /// TODO Edit XML Comment Template for GetBlockNetworkInformation
        private List<Network> GetBlockNetworkInformation()
        {
            List<Network> networks = new List<Network>();

            XmlNodeList listOfNetworks = RootNode.SelectNodes("//SW.Blocks.CompileUnit");

            if (listOfNetworks != null)
            {
                foreach (XmlNode network in listOfNetworks)
                {
                    Network blockNetwork = new Network();

                    #region Title/Comment

                    XmlNodeList listMultiLingualText = network.SelectNodes(".//MultilingualText");

                    foreach (XmlNode nodeMultiLingualText in listMultiLingualText)
                    {
                        if (nodeMultiLingualText.Attributes["CompositionName"].Value.Equals("Title"))
                        {
                            blockNetwork.NetworkTitle.CompositionNameInXml = nodeMultiLingualText.Attributes["CompositionName"].Value;
                            GetTitleOrComment(nodeMultiLingualText, blockNetwork.NetworkTitle);
                        }

                        if (nodeMultiLingualText.Attributes["CompositionName"].Value.Equals("Comment"))
                        {
                            blockNetwork.NetworkComment.CompositionNameInXml = nodeMultiLingualText.Attributes["CompositionName"].Value;
                            GetTitleOrComment(nodeMultiLingualText, blockNetwork.NetworkComment);
                        }
                    }

                    #endregion Title/Comment

                    #region Access

                    //all used Tags with Symbol information (excl. Constants)
                    XmlNodeList listOfAccess = network.SelectNodes(".//siemensNetworks:Access[siemensNetworks:Symbol]", _ns);

                    foreach (XmlNode access in listOfAccess)
                    {
                        Access memberAccess = new Access();
                        Symbol accessSymbol = new Symbol();

                        memberAccess.AccessScope = access.Attributes["Scope"].Value;
                        memberAccess.MemberDatatype = string.Empty; //access.Attributes["Type"].Value;
                        memberAccess.UId = access.Attributes["UId"].Value;

                        XmlNodeList listOfSymbolComponentsWithAccessModifier = access.SelectNodes(".//siemensNetworks:Symbol/siemensNetworks:Component", _ns);

                        foreach (XmlNode component in listOfSymbolComponentsWithAccessModifier)
                        {
                            try
                            {
                                accessSymbol.Components.Add(component.Attributes["Name"].Value);
                                accessSymbol.SimpleAccessModifier = component.Attributes["SimpleAccessModifier"].Value;
                            }
                            catch (NullReferenceException)
                            {
                                //catch NullReferenceException if Access Component does not have the Attribute "SimpleAccessModifier"
                            }
                        }
                        memberAccess.AccessSymbol = accessSymbol;
                        memberAccess.MemberName = accessSymbol.ToString();

                        blockNetwork.NetworkAccess.Add(memberAccess);
                    }

                    #endregion Access

                    #region Part

                    XmlNodeList listOfPart = network.SelectNodes(".//siemensNetworks:Part", _ns);

                    foreach (XmlNode part in listOfPart)
                    {
                        Instruction usedInstruction = new Instruction(part.Attributes["Name"].Value, part.Attributes["UId"].Value);

                        blockNetwork.NetworkInstructions.Add(usedInstruction);
                    }

                    #endregion Part

                    #region CallRef

                    XmlNodeList listOfCallRef = network.SelectNodes(".//siemensNetworks:Call", _ns);

                    foreach (XmlNode nodeCallref in listOfCallRef)
                    {
                        CallRef callref = new CallRef();

                        callref.CallType = string.Empty; // nodeCallref.Attributes["CallType"].Value;
                        callref.UId = nodeCallref.Attributes["UId"].Value;

                        XmlNode nodeCallInfo = nodeCallref.SelectSingleNode(".//siemensNetworks:CallInfo", _ns);

                        callref.Name = nodeCallInfo.Attributes["Name"].Value;
                        callref.BlockType = nodeCallInfo.Attributes["BlockType"].Value;

                        blockNetwork.NetworkCalls.Add(callref);
                    }

                    #endregion CallRef

                    networks.Add(blockNetwork);
                }
            }

            return networks;
        }

        /// <summary>Gets the udt members.</summary>
        /// <returns>List&lt;Member&gt;</returns>
        /// TODO Edit XML Comment Template for GetUdtMembers
        private List<Member> GetUdtMembers()
        {
            //add xml specific namespace
            XmlNamespaceManager ns = new XmlNamespaceManager(Document.NameTable);
            ns.AddNamespace("SI", "http://www.siemens.com/automation/Openness/SW/Interface/v1");

            List<Member> memberList = new List<Member>();

            //List of Sections within the BlockInterface (Input, Output ....)
            XmlNodeList listOfSections = RootNode.SelectNodes("//Interface/SI:Sections/SI:Section", ns);

            if (listOfSections != null)
            {
                foreach (XmlNode section in listOfSections)
                {
                    //list of Member within each section
                    XmlNodeList listOfMember = section.ChildNodes;

                    foreach (XmlNode member in listOfMember)
                    {
                        Member blockIMember = null;

                        if (member.Attributes != null && member.Attributes["Datatype"].Value.Equals("Struct"))
                        {
                            blockIMember = new Struct(member.Attributes["Name"].Value);

                            GetStructChildNodes(member.ChildNodes, (Struct)blockIMember);
                        }
                        else if (member.Attributes != null && (member.LocalName.Equals("Member") && member.HasChildNodes && member.Attributes["Datatype"].Value.Contains("\"") && !member.Attributes["Datatype"].Value.Contains("Array of")))
                        {
                            foreach (XmlNode child in member.ChildNodes)
                            {
                                if (child.Name.Equals("Sections"))
                                {
                                    blockIMember = new MultiInstance(member.Attributes["Name"].Value, member.Attributes["Datatype"].Value);
                                    GetMultiInstanceMember(member, (MultiInstance)blockIMember);
                                }
                            }
                        }
                        else
                        {
                            blockIMember = new Member(member.Attributes?["Name"].Value, member.Attributes?["Datatype"].Value);
                        }
                        if (blockIMember != null)
                        {
                            GetStartValue(member, blockIMember);
                            GetComment(member, blockIMember);

                            memberList.Add(blockIMember);
                        }
                    }
                }
            }

            return memberList;
        }

        /// <summary>Gets the structure child nodes.</summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <param name="structMember">The structure member.</param>
        /// TODO Edit XML Comment Template for GetStructChildNodes
        private void GetStructChildNodes(XmlNodeList childNodes, Struct structMember)
        {
            foreach (XmlNode node in childNodes)
            {
                if (node.Name.Equals("Member"))
                {
                    Member blockIMember = null;

                    if (node.Attributes != null && node.Attributes["Datatype"].Value.Equals("Struct"))
                    {
                        blockIMember = new Struct(node.Attributes["Name"].Value);

                        GetStructChildNodes(node.ChildNodes, (Struct)blockIMember);
                    }
                    else if (node.Attributes != null && (node.LocalName.Equals("Member") && node.HasChildNodes && node.Attributes["Datatype"].Value.Contains("\"") && !node.Attributes["Datatype"].Value.Contains("Array of")))
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Name.Equals("Sections"))
                            {
                                blockIMember = new MultiInstance(node.Attributes["Name"].Value, node.Attributes["Datatype"].Value);
                                GetMultiInstanceMember(node, (MultiInstance)blockIMember);
                            }
                        }
                    }
                    else
                    {
                        if (node.Attributes != null)
                            blockIMember = new Member(node.Attributes["Name"].Value, node.Attributes["Datatype"].Value);
                    }
                    if (blockIMember != null)
                    {
                        GetStartValue(node, blockIMember);
                        GetComment(node, blockIMember);
                        structMember.NestedMembers.Add(blockIMember);
                    }
                }
            }
        }

        /// <summary>Gets the multi instance member.</summary>
        /// <param name="member">The member.</param>
        /// <param name="instance">The instance.</param>
        /// TODO Edit XML Comment Template for GetMultiInstanceMember
        private void GetMultiInstanceMember(XmlNode member, MultiInstance instance)
        {
            foreach (XmlNode node in member.ChildNodes)
            {
                ParseInterface(node.SelectNodes("SI:Section", _ns), instance.InterfaceSections);
            }
        }

        /// <summary>Gets the comment.</summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <param name="blockIMember">The block i member.</param>
        /// TODO Edit XML Comment Template for GetComment
        private void GetComment(XmlNode childNodes, Member blockIMember)
        {
            XmlNodeList commentList = childNodes.SelectNodes("SI:Comment", _ns);

            if (commentList != null)
            {
                foreach (XmlNode comment in commentList)
                {
                    if (comment != null)
                    {
                        XmlNodeList listMultiLanguageText = comment.SelectNodes("SI:MultiLanguageText", _ns);
                        blockIMember.MemberComment.CompositionNameInXml = string.Empty;

                        if (listMultiLanguageText != null)
                        {
                            foreach (XmlNode multiLanguageText in listMultiLanguageText)
                            {
                                if (multiLanguageText.Attributes != null)
                                    blockIMember.MemberComment.MultiLanguageTextItems.Add(multiLanguageText.Attributes["Lang"].Value, multiLanguageText.InnerText);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>Gets the start value.</summary>
        /// <param name="childNodes">The child nodes.</param>
        /// <param name="blockIMember">The block i member.</param>
        /// TODO Edit XML Comment Template for GetStartValue
        private void GetStartValue(XmlNode childNodes, Member blockIMember)
        {
            XmlNodeList startValueList = childNodes.SelectNodes("SI:StartValue", _ns);

            if (startValueList != null)
            {
                foreach (XmlNode startValue in startValueList)
                {
                    if (startValue != null)
                    {
                        blockIMember.MemberDefaultValue = startValue.InnerText;
                    }
                }
            }
        }

        /// <summary>Gets the title or comment.</summary>
        /// <param name="nodeToMultiLanguageText">The node to multi language text.</param>
        /// <param name="textItems">The text items.</param>
        /// TODO Edit XML Comment Template for GetTitleOrComment
        private void GetTitleOrComment(XmlNode nodeToMultiLanguageText, MultiLanguageText textItems)
        {
            XmlNodeList listTextItemValue = nodeToMultiLanguageText.SelectNodes(".//Value");

            if (listTextItemValue != null)
            {
                foreach (XmlNode nodeValue in listTextItemValue)
                {
                    if (nodeValue.Attributes != null)
                        textItems.MultiLanguageTextItems.Add(nodeValue.Attributes["lang"].Value, nodeValue.InnerText);
                }
            }
        }

        /// <summary>Gets the block title or comment.</summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns>MultiLanguageText</returns>
        /// TODO Edit XML Comment Template for GetBlockTitleOrComment
        private MultiLanguageText GetBlockTitleOrComment(string xpath)
        {
            XmlNodeList listTitleOrComment = RootNode.SelectNodes(xpath);
            MultiLanguageText textItems = new MultiLanguageText();

            if (listTitleOrComment != null)
            {
                foreach (XmlNode blockTitleNode in listTitleOrComment)
                {
                    if (blockTitleNode.ParentNode?.ParentNode != null && blockTitleNode.ParentNode.ParentNode.Name.Contains("SW.Blocks"))
                    {
                        if (blockTitleNode.Attributes != null)
                            textItems.CompositionNameInXml = blockTitleNode.Attributes["CompositionName"].Value;
                        GetTitleOrComment(blockTitleNode, textItems);
                    }
                }
            }

            return textItems;
        }

        #endregion private methods
    }
}