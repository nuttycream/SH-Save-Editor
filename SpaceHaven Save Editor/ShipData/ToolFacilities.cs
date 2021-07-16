namespace SpaceHaven_Save_Editor.ShipData
{
    public class ToolFacilities
    {
        public readonly int Index;

        public ToolFacilities(int index)
        {
            Index = index + 1;
        }

        //feat ft=
        public int BuildingTools { get; set; }

        public override string ToString()
        {
            return "Tool Facility " + (Index + 1);
        }
    }
}