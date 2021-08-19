using Siemens.Engineering.SW.Blocks;
using System;
using System.Xml;

namespace ProjectTreeRenamer.Utility
{
    internal static class XmlEdit
    {
        public static void RenameXMLContents(string filePath, PlcBlock block)
        {
            switch (block)
            {
                case FB fb:
                    RenameFB(filePath);
                    break;

                case FC fc:
                    RenameFC(filePath);
                    break;

                case OB ob:
                    RenameOB(filePath);
                    break;

                case InstanceDB db:
                    //Ignore instance db's ? - Does editing the code file and importing that update the instance DB? or does the instace DB need to be updated?
                    break;

                case GlobalDB db:
                    RenameGlobalDB(filePath);
                    break;

                case ArrayDB db:
                    //TODO: throw error message at the user, but dont break operation?
                    throw new NotImplementedException("Array DB's are currently unhandled in the rename operation: " + block.Name);

                default:
                    break;
            }
        }

        private static void RenameOB(string filePath)
        {
            //throw new NotImplementedException();
        }

        private static void RenameFC(string filePath)
        {
            //throw new NotImplementedException();
        }

        private static void RenameFB(string filePath)
        {
            //throw new NotImplementedException();
        }

        public static void RenameGlobalDB(string filePath)
        {
            try
            {
                //open document
                XmlDocument document = new XmlDocument();
                document.Load(filePath);

                //select <SW.Blocks.FC> as "root"
                XmlNode swBlocksFc = document.SelectSingleNode("//SW.Blocks.GlobalDB");

                //set XML namespaces
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
                nsmgr.AddNamespace("ns", @"http://www.siemens.com/automation/Openness/SW/Interface/v4");

                //remove <Section Name="Return"/> and move <Member>s to <Section Name="Output">
                XmlNode sectionReturn = swBlocksFc.SelectSingleNode(".//ns:Section[@Name='Return']", nsmgr);
            }
            catch
            {
            }
        }
    }
}