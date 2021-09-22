using System.Collections.Generic;

namespace SpaceHaven_Save_Editor.Data
{
    public class Faction
    {
        public Faction(string factionName)
        {
            FactionName = factionName;
            Relationships = new List<Relationship>();
        }

        public string FactionName { get; set; }
        public List<Relationship> Relationships { get; set; }
    }

    public class Relationship
    {
        public Relationship(string factionName, string factionStance, int relationshipAmount, bool tradeAccess,
            bool shipAccess, bool visionAccess)
        {
            FactionName = factionName;
            FactionStance = factionStance;

            RelationshipAmount = relationshipAmount;
            TradeAccess = tradeAccess;
            ShipAccess = shipAccess;
            VisionAccess = visionAccess;
        }

        public string FactionName { get; set; }
        public string FactionStance { get; set; }
        public int RelationshipAmount { get; set; }
        public bool TradeAccess { get; set; }
        public bool ShipAccess { get; set; }
        public bool VisionAccess { get; set; }
    }
}