using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindShips
    {
        private static XmlNodeList? _shipNodes;
        public static List<Ship> ReadShips(XmlNode rootNode)
        {
            _shipNodes = rootNode.SelectNodes(".//ship");
            if (_shipNodes == null) return new List<Ship>();

            List<Ship> ships = (from XmlNode shipNode in _shipNodes
                select new Ship
                {
                    ShipNode = shipNode,
                    ShipName = Utilities.GetAttributeValue(shipNode, "sname"),
                    ShipFaction = shipNode.SelectSingleNode(".//settings[@owner]").Attributes["owner"].Value,
                    ShipState = shipNode.SelectSingleNode(".//settings[@owner]").Attributes["state"].Value,
                    Characters = new ObservableCollection<Character>(FindCharacters.ReadCharacters(shipNode.SelectSingleNode(".//characters"))),
                    StorageFacilities = new ObservableCollection<StorageFacility>(FindStorages.ReadStorageFacilities(shipNode.SelectNodes(".//feat[@eatAllowed]"))),
                    ToolFacilities = new ObservableCollection<ToolFacility>(FindStorages.ReadToolFacilities(shipNode.SelectNodes(".//feat[@ft]")))
                }).ToList();

            return ships;
        }

        public static void WriteShips(List<Ship> ships)
        {
            foreach (XmlNode shipNode in _shipNodes)
            {
                var shipName = Utilities.GetAttributeValue(shipNode, "sname");
                var ship = ships.FirstOrDefault(s => s.ShipName.Equals(shipName));
                if (ship == null) continue;
                
                var storageNodes = shipNode.SelectNodes(".//feat[@eatAllowed]");
                var toolStorageNodes = shipNode.SelectNodes(".//feat[@ft]");
                var characterRootNode = shipNode.SelectSingleNode(".//characters");
                
                FindStorages.WriteStorageFacilities(storageNodes, ship.StorageFacilities.ToList());
                FindStorages.WriteToolFacilities(toolStorageNodes, ship.ToolFacilities.ToList());
                FindCharacters.WriteCharacters(characterRootNode, ship.Characters.ToList());
                
            }
        }
    }
}