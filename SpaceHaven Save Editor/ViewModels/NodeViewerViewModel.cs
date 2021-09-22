using System.IO;
using System.Text;
using System.Xml;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class NodeViewerViewModel : ViewModelBase
    {
        public NodeViewerViewModel(string xmlNodeName, XmlNode xmlNode)
        {
            XmlNodeName = xmlNodeName;

            MemoryStream mStream = new();
            XmlTextWriter writer = new(mStream, Encoding.Unicode) {Formatting = Formatting.Indented};

            xmlNode.WriteContentTo(writer);
            writer.Flush();
            mStream.Flush();
            mStream.Position = 0;

            StreamReader sReader = new(mStream);
            string formattedXml = sReader.ReadToEnd();

            XmlNodeData = formattedXml;

            mStream.Close();
            writer.Close();
        }

        public NodeViewerViewModel()
        {
            XmlNodeName = "";
            XmlNodeData = "";
        }

        public string XmlNodeName { get; }
        public string XmlNodeData { get; }
    }
}