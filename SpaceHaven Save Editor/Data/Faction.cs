using System.Collections.Generic;
using ReactiveUI;

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

        public override string ToString()
        {
            return FactionName;
        }
    }

    public class Relationship : ReactiveObject
    {
        private int _relationshipAmount;
        private string _factionStance;

        public Relationship(string factionName, string factionStance, int relationshipAmount, bool tradeAccess,
            bool shipAccess, bool visionAccess)
        {
            FactionName = factionName;
            _factionStance = factionStance;

            RelationshipAmount = relationshipAmount;
            TradeAccess = tradeAccess;
            ShipAccess = shipAccess;
            VisionAccess = visionAccess;
        }

        public string FactionName { get; set; }

        public string FactionStance
        {
            get => _factionStance;
            set => this.RaiseAndSetIfChanged(ref _factionStance, value);
        }

        public int RelationshipAmount
        {
            get => _relationshipAmount;
            set
            {
                this.RaiseAndSetIfChanged(ref _relationshipAmount, value);
                UpdateStance();
            }
        }

        private void UpdateStance()
        {
            if (FactionStance == "Player") return;

            FactionStance = RelationshipAmount switch
            {
                >= 25 => "Allies",
                <= -25 => "Enemies",
                _ => "Neutral"
            };
        }

        public bool TradeAccess { get; set; }
        public bool ShipAccess { get; set; }
        public bool VisionAccess { get; set; }
    }
}