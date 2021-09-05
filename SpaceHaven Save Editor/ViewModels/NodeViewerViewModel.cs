using System.Xml;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class NodeViewerViewModel : ViewModelBase
    {
        public NodeViewerViewModel(string xmlNodeName, XmlNode xmlNode)
        {
            XmlNodeName = xmlNodeName;
            XmlNodeData = xmlNode.InnerXml;
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