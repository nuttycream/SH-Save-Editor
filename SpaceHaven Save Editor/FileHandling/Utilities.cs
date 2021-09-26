using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class Utilities
    {
        public static bool validateFile(XmlNode? rootNode, IEnumerable<string> nodesToValidate)
        {
            List<object?> nodes = nodesToValidate.
                Select(node => rootNode?.SelectNodes(node)).Cast<object?>().ToList();

            return nodes.All(node => node != null);
        }
        
        
        public static string GetAttributeValue(XmlNode? node, string attributeName)
        {
            if (node == null)
                return "Err";
            var attribute = node.Attributes?[attributeName];
            return attribute == null ? "0" : attribute.Value;
        }
    }
}