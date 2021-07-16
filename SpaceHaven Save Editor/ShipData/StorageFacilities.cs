using System.Collections.ObjectModel;

namespace SpaceHaven_Save_Editor.ShipData
{
    public class StorageFacilities
    {
        public readonly int Index;

        public StorageFacilities(int index)
        {
            Index = index;
            CargoList = new ObservableCollection<Cargo>();
        }

        //feat eatAllowed=
        public ObservableCollection<Cargo> CargoList { get; set; }

        public override string ToString()
        {
            return "Storage " + (Index + 1);
        }
    }
}