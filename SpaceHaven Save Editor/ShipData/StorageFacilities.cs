using System.Collections.ObjectModel;

namespace SpaceHaven_Save_Editor.ShipData
{
    public class StorageFacilities
    {
        private readonly int _index;

        public StorageFacilities(int index)
        {
            _index = index;
            CargoList = new ObservableCollection<Cargo>();
        }

        //feat eatAllowed=
        public ObservableCollection<Cargo> CargoList { get; set; }

        public override string ToString()
        {
            return "Storage " + (_index + 1);
        }
    }
}