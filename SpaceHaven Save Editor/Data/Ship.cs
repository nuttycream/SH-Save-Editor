using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using ReactiveUI;

namespace SpaceHaven_Save_Editor.Data
{
    public class Ship : ReactiveObject
    {
        private string _shipFaction;
        private string _shipState;

        public Ship(string shipName, IEnumerable<Character> characters, IEnumerable<ToolFacility> toolFacilities, IEnumerable<StorageFacility> storageFacilities, 
            XmlNode shipNode, string shipFaction, string shipState)
        {
            ShipName = shipName;
            Characters = new ObservableCollection<Character>(characters);
            ToolFacilities = new ObservableCollection<ToolFacility>(toolFacilities);
            StorageFacilities = new ObservableCollection<StorageFacility>(storageFacilities);
            ShipNode = shipNode;

            _shipFaction = shipFaction;
            _shipState = shipState;
        }

        public string ShipName { get; }

        public string ShipFaction
        {
            get => _shipFaction;
            set => this.RaiseAndSetIfChanged(ref _shipFaction, value);
        }

        public string ShipState
        {
            get => _shipState;
            set => this.RaiseAndSetIfChanged(ref _shipState, value);
        }

        public XmlNode ShipNode { get; }
        public ObservableCollection<Character> Characters { get; set; }
        public ObservableCollection<ToolFacility> ToolFacilities { get; set; }
        public ObservableCollection<StorageFacility> StorageFacilities { get; set; }

        public override string ToString()
        {
            return ShipName;
        }
    }
}