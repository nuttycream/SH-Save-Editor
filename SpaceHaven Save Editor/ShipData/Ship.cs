using System.Collections.Generic;
using PropertyChanged;

namespace SpaceHaven_Save_Editor.ShipData
{
    [AddINotifyPropertyChangedInterface]
    public class Ship
    {
        public Ship()
        {
            StorageFacilities = new List<StorageFacilities>();
            ToolFacilities = new List<ToolFacilities>();
        }

        public List<StorageFacilities> StorageFacilities { get; set; }
        public List<ToolFacilities> ToolFacilities { get; set; }
    }
}