using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.ShipData
{
    public class Cargo
    {
        public Cargo(int id, int amount)
        {
            foreach (var nodeId in IDCollections.ItemNodes)
            {
                if (nodeId.ID != id) continue;
                CargoId = id;
                CargoName = nodeId.Name;
                CargoAmount = amount;
                break;
            }
        }

        public int CargoId { get; set; }
        public string CargoName { get; set; }
        public int CargoAmount { get; set; }

        public override string ToString()
        {
            return CargoName;
        }
    }
}