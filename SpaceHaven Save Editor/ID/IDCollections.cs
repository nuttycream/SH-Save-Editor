using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SpaceHaven_Save_Editor.ID
{
    // ReSharper disable once InconsistentNaming
    public static class IDCollections
    {
        private static ObservableCollection<Node> _attributeNodes = new();
        private static ObservableCollection<Node> _itemNodes = new();
        private static ObservableCollection<Node> _skillNodes = new();
        private static ObservableCollection<Node> _traitNodes = new();

        private static readonly Dictionary<int, string> DefaultAttributeIDs = new()
        {
            {210, "Bravery"},
            {212, "Zest"},
            {213, "Intelligence"},
            {214, "Perception"}
        };

        private static readonly Dictionary<int, string> DefaultSkillIDs = new()
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

        private static readonly Dictionary<int, string> DefaultTraitIDs = new()
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

        private static readonly Dictionary<int, string> DefaultItemIDs = new()
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

        private static readonly Dictionary<ObservableCollection<Node>, Dictionary<int, string>> Defaults = new()
        {
            {AttributeNodes, DefaultAttributeIDs}, {ItemNodes, DefaultItemIDs}, {SkillNodes, DefaultSkillIDs},
            {TraitNodes, DefaultTraitIDs}
        };

        public static ObservableCollection<Node> AttributeNodes
        {
            get => _attributeNodes;
            set
            {
                _attributeNodes = value;
                NotifyStaticPropertyChanged("AttributeNodes");
            }
        }

        public static ObservableCollection<Node> ItemNodes
        {
            get => _itemNodes;
            set
            {
                _itemNodes = value;
                NotifyStaticPropertyChanged("ItemNodes");
            }
        }

        public static ObservableCollection<Node> SkillNodes
        {
            get => _skillNodes;
            set
            {
                _skillNodes = value;
                NotifyStaticPropertyChanged("SkillNodes");
            }
        }

        public static ObservableCollection<Node> TraitNodes
        {
            get => _traitNodes;
            set
            {
                _traitNodes = value;
                NotifyStaticPropertyChanged("TraitNodes");
            }
        }

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void NotifyStaticPropertyChanged(string name)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }

        public static void SetToDefaults()
        {
            foreach (var (keys, values) in Defaults)
            foreach (var (key, value) in values)
                keys.Add(new Node(key, value));
        }

        public static Dictionary<string, ObservableCollection<Node>> GetIdList()
        {
            var idList = new Dictionary<string, ObservableCollection<Node>>
            {
                {"Attributes", AttributeNodes},
                {"Items", ItemNodes},
                {"Skills", SkillNodes},
                {"Traits", TraitNodes}
            };

            return idList;
        }

        public static void SetIdList(Dictionary<string, ObservableCollection<Node>> idList)
        {
            foreach (var (key, value) in idList)
                switch (key)
                {
                    case "Attributes":
                        AttributeNodes = value;
                        break;
                    case "Items":
                        ItemNodes = value;
                        break;
                    case "Skills":
                        SkillNodes = value;
                        break;
                    case "Traits":
                        TraitNodes = value;
                        break;
                }
        }

        public static IEnumerable<string> GetItemList()
        {
            var itemList = new List<string>();
            foreach (var node in _itemNodes) itemList.Add(node.Name);

            return itemList;
        }
    }
}