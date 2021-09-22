using System.Collections.ObjectModel;
using System.Xml;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class Character
    {
        public Character()
        {
            CharacterName = "";
            FactionSide = "";
            CharacterStats = new ObservableCollection<CharacterProp>();
            CharacterAttributes = new ObservableCollection<CharacterProp>();
            CharacterSkills = new ObservableCollection<CharacterProp>();
            CharacterTraits = new ObservableCollection<CharacterProp>();
        }

        public XmlNode? CharacterXmlNode { get; set; }
        public int CharacterEntityId { get; set; }
        public string CharacterName { get; set; }
        public string FactionSide { get; set; }
        public bool IsCrewman { get; set; }
        public bool IsAClone { get; private init; }
        public ObservableCollection<CharacterProp> CharacterStats { get; set; }
        public ObservableCollection<CharacterProp> CharacterAttributes { get; set; }
        public ObservableCollection<CharacterProp> CharacterSkills { get; set; }
        public ObservableCollection<CharacterProp> CharacterTraits { get; set; }

        public string CharacterNameToShow =>
            CharacterName + " [" + (IsCrewman ? "Crewman" : "Prisoner/Refugee") +
            (IsAClone ? ", Clone]" : "]");

        public Character CloneCharacter(string newName, int entId)
        {
            if (CharacterXmlNode == null) return new Character();
            var newXmlNode = CharacterXmlNode.CloneNode(true);

            newXmlNode.Attributes![NodeCollection.CharacterNameAttribute]!.Value = newName;
            newXmlNode.Attributes![NodeCollection.CharacterEidAttribute]!.Value = entId.ToString();

            return new Character
            {
                CharacterXmlNode = newXmlNode,
                FactionSide = FactionSide,
                IsCrewman = IsCrewman,
                IsAClone = true,
                CharacterEntityId = entId,
                CharacterName = newName,
                CharacterStats = CharacterStats,
                CharacterAttributes = CharacterAttributes,
                CharacterSkills = CharacterSkills,
                CharacterTraits = CharacterTraits
            };
        }
    }
}