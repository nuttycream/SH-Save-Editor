using System.Collections.Generic;

namespace SpaceHaven_Save_Editor.ID
{
    
    // ReSharper disable once InconsistentNaming
    public static class IDCollections
    {
        public static readonly List<string> Foods = new()
        {
            "protein",
            "carbs",
            "fat",
            "vitamins",
            "toxins"
        };
        
        public static readonly List<string> CharacterStats = new()
        {
            "Health",
            "Rest",
            "Comfort",
            "Oxygen",
            "Mood",
            "Social"
        };

        public static readonly Dictionary<int, string> Attributes = new()
        {
            {210, "Bravery"},
            {212, "Zest"},
            {213, "Intelligence"},
            {214, "Perception"}
        };

        public static readonly Dictionary<int, string> Skills = new()
        {
            {1, "Piloting"},
            {2, "Mining"},
            {3, "Botany"},
            {4, "Construct"},
            {5, "Industry"},
            {6, "Medical"},
            {7, "Gunner"},
            {8, "Shielding"},
            {9, "Operations"},
            {10, "Weapons"},
            {12, "Logistics"},
            {13, "Maintenance"},
            {14, "Navigation"},
            {16, "Research"}
        };
        
        public static readonly Dictionary<int, string> Traits = new()
        {
            {2082, "Alien Lover"},
            {1037, "Anti-Social"},
            {1036, "Blood Lust"},
            {1048, "Charming"},
            {656, "Clumsy"},
            {1046, "Confident"},
            {1039, "Fast Learner"},
            {1562, "Gourmand"},
            {1041, "Hard Working"},
            {191, "Hero"},
            {1533, "Iron Stomach"},
            {1044, "Iron-Willed"},
            {1040, "Lazy"},
            {1535, "Minimalist"},
            {1038, "Needy"},
            {1047, "Neurotic"},
            {1534, "Nyctophilia"},
            {1043, "Peace-Loving"},
            {1042, "Psychopath"},
            {1035, "Smart"},
            {1045, "Spacefarer"},
            {1034, "Suicidal"},
            {1560, "Talkative"},
            {655, "Wimp"}
        };

        public static readonly Dictionary<int, string> Items = new()
        {
            {15, "Root Vegetables"},
            {16, "Water"},
            {40, "Ice"},
            {71, "Bio Matter"},
            {127, "Rubble"},
            {157, "Base Metals"},
            {158, "Energium"},
            {162, "Infrablock"},
            {169, "Noble Metals"},
            {170, "Carbon"},
            {171, "Raw Chemicals"},
            {172, "Hyperium"},
            {173, "Electronic Component"},
            {174, "Energy Rod"},
            {175, "Plastics"},
            {176, "Chemicals"},
            {177, "Fabrics"},
            {178, "Hyperfuel"},
            {179, "Processed Food/Verarbeitete Lebensmittel"},
            {706, "Fruits"},
            {707, "Artificial Meat/Artifikal meat"},
            {712, "Space Food"},
            {725, "Assault Rifle"},
            {728, "SMG"},
            {729, "Shotgun"},
            {760, "Five-Seven Pistol"},
            {930, "Techblock/Tech-Blöcke"},
            {984, "Monster Meat"},
            {985, "Human Meat"},
            {1759, "Hull Block"},
            {1873, "Infra Scrap"},
            {1874, "Soft Scrap"},
            {1886, "Hull Scrap"},
            {1919, "Energy Block"},
            {1920, "Superblock"},
            {1921, "Soft Block"},
            {1922, "Steel Plates"},
            {1924, "Optronics Component"},
            {1925, "Quantronics Component"},
            {1926, "Energy Cell"},
            {1932, "Fibers"},
            {1946, "Tech Scrap"},
            {1947, "Energy Scrap"},
            {1954, "Human Corpse"},
            {1955, "Monster Corpse"},
            {2053, "Medical Supplies"},
            {2058, "IV Fluid"},
            {2657, "Nuts and Seeds"},
            {2475, "Fertilizer"}
        };
    }
}