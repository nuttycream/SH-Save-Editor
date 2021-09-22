using System.Collections.Generic;
using System.Xml;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindFactions
    {
        public static List<Faction> ReadFactions(XmlNodeList factionNodes)
        {
            var factions = new List<Faction>();
            foreach (XmlNode factionNode in factionNodes)
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
    }
}