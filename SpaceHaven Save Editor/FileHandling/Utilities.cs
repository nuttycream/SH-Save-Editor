using System.Xml;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class Utilities
    {
        public static string GetAttributeValue(XmlNode? node, string attributeName)
        {
            if (node == null)
                return "Err";
            var attribute = node.Attributes?[attributeName];
            return attribute == null ? "0" : attribute.Value;
        }
    }
}