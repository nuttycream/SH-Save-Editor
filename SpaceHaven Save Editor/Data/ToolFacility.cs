using ReactiveUI;

namespace SpaceHaven_Save_Editor.Data
{
    public class ToolFacility : ReactiveObject
    {
        private int _buildingToolsAmount;

        public ToolFacility(int buildingToolsAmount)
        {
            BuildingToolsAmount = buildingToolsAmount;
        }

        public int BuildingToolsAmount
        {
            get => _buildingToolsAmount;
            set => this.RaiseAndSetIfChanged(ref _buildingToolsAmount, value);
        }
    }
}