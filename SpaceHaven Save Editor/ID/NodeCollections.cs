using System.Collections.Generic;

namespace SpaceHaven_Save_Editor.ID
{
    public static class NodeCollections
    {
        public static List<string> Foods = new()
        {
            "protein",
            "carbs",
            "fat",
            "vitamins",
            "toxins"
        };

        public static List<string> CharacterStats = new()
        {
            "Health",
            "Rest",
            "Comfort",
            "Oxygen",
            "Mood",
            "Social"
        };

        public static string CharacterNode { get; set; } = "c";
        public static string CharacterAttributeName { get; set; } = "name";
        public static string CharacterStatsNode { get; set; } = "props";
        public static string CharacterFoodNode { get; set; } = "Food";
        public static string CharacterStoredFoodNode { get; set; } = "stored";
        public static string CharacterStomachFoodNode { get; set; } = "belly";
        public static string CharacterPersonalNode { get; set; } = "pers";
        public static string CharacterAttributesNode { get; set; } = "attr";
        public static string CharacterTraitsNode { get; set; } = "traits";
        public static string CharacterSkillsNode { get; set; } = "skills";

        public static string PlayerBankNode { get; set; } = "playerBank";
        public static string PlayerBankAttribute { get; set; } = "ca";

        public static string StorageNode { get; set; } = "feat";
        public static string StoragesXPath { get; set; } = "//feat[@eatAllowed]";
        public static string ToolsXPath { get; set; } = "//feat[@ft]";
        public static string StorageAttributeForCargo { get; set; } = "eatAllowed";
        public static string CargoItemElementName { get; set; } = "s";
        public static string CargoItemAttributeId { get; set; } = "elementaryId";
        public static string CargoItemAttributeAmount { get; set; } = "inStorage";
        public static string StorageAttributeForTools { get; set; } = "ft";
    }
}