using System.Collections.Generic;

namespace SpaceHaven_Save_Editor.References
{
    public static class NodeCollection
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
            "Food",
            "Rest",
            "Comfort",
            "Oxygen",
            "Mood",
            "Social"
        };

        public static string CharactersRootNode { get; set; } = "characters";
        public static string CharacterNode { get; set; } = "c";
        public static string CharacterNameAttribute { get; set; } = "name";
        public static string CharacterEidAttribute { get; set; } = "entId";
        public static string CharacterStatsNode { get; set; } = "props";

        public static string CharacterStatsAttribute { get; set; } = "v";

        //public static string CharacterStoredFoodNode { get; set; } = "stored";
        //public static string CharacterStomachFoodNode { get; set; } = "belly";
        public static string CharacterPersonalNode { get; set; } = "pers";
        public static string CharacterAttributesNode { get; set; } = "attr";
        public static string CharacterAttributesAttributeID { get; set; } = "id";
        public static string CharacterAttributesAttributePoints { get; set; } = "points";
        public static string CharacterTraitsNode { get; set; } = "traits";
        public static string CharacterTraitsAttributeID { get; set; } = "id";
        public static string CharacterSkillsNode { get; set; } = "skills";
        public static string CharacterSkillsAttributesID { get; set; } = "sk";
        public static string CharacterSkillsAttributesLevel { get; set; } = "level";

        public static string PlayerBankNode { get; set; } = "playerBank";
        public static string PlayerBankAttribute { get; set; } = "ca";

        public static string ShipNode { get; set; } = "ships";
        public static string ShipName { get; set; } = "sname";
        public static string ShipSettings { get; set; } = "settings";
        public static string ShipOwnerAttribute { get; set; } = "owner";
        public static string StorageNode { get; set; } = "feat";
        public static string StoragesXPath { get; set; } = ".//feat[@eatAllowed]";
        public static string ToolsXPath { get; set; } = ".//feat[@ft]";
        public static string CargoItemElementName { get; set; } = "s";
        public static string CargoItemAttributeId { get; set; } = "elementaryId";
        public static string CargoItemAttributeAmount { get; set; } = "inStorage";
        public static string StorageAttributeForTools { get; set; } = "ft";
    }
}