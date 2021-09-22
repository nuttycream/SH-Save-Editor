using System.Diagnostics;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class CharacterAttribute : ReactiveObject
    {
        private int _value;

        public CharacterAttribute(int id, int value)
        {
            if (IdCollection.DefaultAttributeIDs.TryGetValue(id, out var attributeName))
            {
                ID = id;
                Name = attributeName;
                Value = value;
            }
            else
            {
                Debug.Print("Error Invalid Attribute ID.");

                ID = -1;
                Name = "Invalid Attribute";
                Value = -1;
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public int Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }
}