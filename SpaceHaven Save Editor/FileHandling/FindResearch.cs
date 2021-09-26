using System.Collections.ObjectModel;
using System.Xml;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindResearch
    {
        public static ObservableCollection<ResearchItem> ReadResearchItems(XmlNode researchRootNode)
        {
            var researchNodes = researchRootNode.SelectNodes(".//l[@techId]");
            var researchItems = new ObservableCollection<ResearchItem>();
            if (researchNodes == null) return researchItems;

            foreach (XmlNode researchNode in researchNodes)
            {
                var blocksNode = researchNode.SelectSingleNode(".//blocksDone");
                if (blocksNode == null)
                    continue;

                if (!int.TryParse(Utilities.GetAttributeValue(researchNode, "techId"), out var idResult)
                    || !int.TryParse(Utilities.GetAttributeValue(blocksNode, "level1"), out var level1Result)
                    || !int.TryParse(Utilities.GetAttributeValue(blocksNode, "level1"), out var level2Result)
                    || !int.TryParse(Utilities.GetAttributeValue(blocksNode, "level1"), out var level3Result)
                    || !IdCollection.DefaultResearchIDs.ContainsKey(idResult))
                    continue;

                ResearchItem researchItem = new(idResult, level1Result, level2Result, level3Result);
                researchItems.Add(researchItem);
            }

            return researchItems;
        }
    }
}