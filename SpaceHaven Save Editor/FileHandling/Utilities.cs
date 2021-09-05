using System.Xml;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class Utilities
    {
        public static XmlNodeList? FindMultipleNodes(XmlNode rootNode, string nodesName)
        {
            return rootNode.SelectNodes("." + nodesName);
        }

        public static XmlNode? FindNode(XmlNode rootNode, string nodeName)
        {
            if (!rootNode.HasChildNodes)
                return null;
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node.Name == nodeName)
                    return node;

                var nodeFound = FindNode(node, nodeName);
                if (nodeFound != null)
                    return nodeFound;
            }

            return null;
        }

        public static XmlNode? FindNode(XmlNode rootNode, string nodeName, string attributeName)
        {
            if (!rootNode.HasChildNodes)
                return null;
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node.Name == nodeName && node.Attributes?[attributeName] != null)
                    return node;

                var nodeFound = FindNode(node, nodeName, attributeName);
                if (nodeFound != null)
                    return nodeFound;
            }

            return null;
        }

        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            var attribute = node.Attributes?[attributeName];
            return attribute == null ? "0" : attribute.Value;
        }
    }
}