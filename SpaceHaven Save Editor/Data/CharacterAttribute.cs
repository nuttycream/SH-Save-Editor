using System.Diagnostics;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class CharacterAttribute : ReactiveObject
    {
        private int _attributeValue;

        public CharacterAttribute(int id, int value)
        {
            if (IdCollection.DefaultAttributeIDs.TryGetValue(id, out var attributeName))
            {
                AttributeId = id;
                AttributeName = attributeName;
                AttributeValue = value;
            }
            else
            {
                Debug.Print("Error Invalid Attribute ID.");

                AttributeId = -1;
                AttributeName = "Invalid Attribute";
                AttributeValue = -1;
            }
        }

        public int AttributeId { get; set; }
        public string AttributeName { get; set; }

        public int AttributeValue
        {
            get => _attributeValue;
            set => this.RaiseAndSetIfChanged(ref _attributeValue, value);
        }
    }
}