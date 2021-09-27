using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindFactions
    {
        private static XmlNodeList? _factionNodes;

        public static List<Faction> ReadFactions(XmlNode factionRootNodes)
        {
            _factionNodes = factionRootNodes.SelectNodes(".//l[@s1]");
            var factions = new List<Faction>();
            if (_factionNodes == null) return factions;

            foreach (XmlNode factionNode in _factionNodes)
            {
                string factionName = Utilities.GetAttributeValue(factionNode, "s1");
                string factionRelationshipName = Utilities.GetAttributeValue(factionNode, "s2");
                string factionRelationshipStance = Utilities.GetAttributeValue(factionNode, "stance");

                int.TryParse(Utilities.GetAttributeValue(factionNode, "relationship"), out var factionRelationship);

                var accessTrade = Utilities.GetAttributeValue(factionNode, "accessTrade") == "true";
                var accessShip = Utilities.GetAttributeValue(factionNode, "accessShip") == "true";
                var accessVision = Utilities.GetAttributeValue(factionNode, "accessVision") == "true";

                Relationship relationship = new(factionRelationshipName, factionRelationshipStance,
                    factionRelationship, accessTrade, accessShip, accessVision);

                var existingFaction = factions.Find(f => f.FactionName == factionName);
                if (existingFaction != null)
                {
                    existingFaction.Relationships.Add(relationship);
                    continue;
                }

                Faction faction = new(factionName);
                faction.Relationships.Add(relationship);
                factions.Add(faction);
            }

            return factions;
        }

        public static void WriteFactions(List<Faction> factions)
        {
            if (_factionNodes == null) return;
            foreach (XmlNode factionNode in _factionNodes)
            {
                var factionName = Utilities.GetAttributeValue(factionNode, "s1");
                var relationshipName = Utilities.GetAttributeValue(factionNode, "s2");
                var faction = factions.FirstOrDefault(f => f.FactionName == factionName);
                var relationshipFaction = faction?.Relationships.FirstOrDefault(r => r.FactionName == relationshipName);
                if (faction == null || relationshipFaction == null) continue;

                factionNode.Attributes["relationship"].Value = relationshipFaction.RelationshipAmount.ToString();
                factionNode.Attributes["accessTrade"].Value = relationshipFaction.TradeAccess ? "true" : "false";
                factionNode.Attributes["accessShip"].Value = relationshipFaction.ShipAccess ? "true" : "false";
                factionNode.Attributes["accessVision"].Value = relationshipFaction.VisionAccess ? "true" : "false";
            }
        }
    }
}