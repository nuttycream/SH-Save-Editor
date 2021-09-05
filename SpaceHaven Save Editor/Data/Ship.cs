using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using ReactiveUI;

namespace SpaceHaven_Save_Editor.Data
{
    public class Ship : ReactiveObject
    {
        private bool _ownedByPlayer;

        public Ship(string shipName, IEnumerable<Character> characters, IEnumerable<StorageFacility> storageFacilities,
            bool ownedByPlayer, XmlNode shipNode)
        {
            ShipName = shipName;
            Characters = new ObservableCollection<Character>(characters);
            StorageFacilities = new ObservableCollection<StorageFacility>(storageFacilities);
            _ownedByPlayer = ownedByPlayer;
            ShipNode = shipNode;
        }

        public string ShipName { get; }

        public bool OwnedByPlayer
        {
            get => _ownedByPlayer;
            set => this.RaiseAndSetIfChanged(ref _ownedByPlayer, value);
        }

        public XmlNode ShipNode { get; }
        public ObservableCollection<Character> Characters { get; set; }
        public ObservableCollection<StorageFacility> StorageFacilities { get; set; }

        public override string ToString()
        {
            return ShipName;
        }
    }
}