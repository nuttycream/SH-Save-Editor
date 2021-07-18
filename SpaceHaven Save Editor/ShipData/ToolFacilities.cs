namespace SpaceHaven_Save_Editor.ShipData
{
    public class ToolFacilities
    {
        private readonly int _index;

        public ToolFacilities(int index)
        {
            _index = index;
        }

        //feat ft=
        public int BuildingTools { get; set; }

        public override string ToString()
        {
            return "Tool Facility " + (_index + 1);
        }
    }
}