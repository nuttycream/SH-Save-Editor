using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.ShipData
{
    public class Cargo
    {
        public Cargo(int id, int amount)
        {
            if (!IDCollections.Items.ContainsKey(id)) return;

            CargoId = id;
            CargoAmount = amount;

            foreach (var (key, value) in IDCollections.Items)
                if (key == id)
                    CargoName = value;
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