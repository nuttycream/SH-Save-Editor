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
            CharacterStats = new ObservableCollection<DataProp>();
            CharacterAttributes = new ObservableCollection<DataProp>();
            CharacterSkills = new ObservableCollection<DataProp>();
            CharacterTraits = new ObservableCollection<DataProp>();
        }

        public XmlNode? CharacterXmlNode { get; set; }
        public int CharacterEntityId { get; set; }
        public string CharacterName { get; set; }
        public string FactionSide { get; set; }
        public bool IsCrewman { get; set; }
        public bool IsAClone { get; private init; }
        public ObservableCollection<DataProp> CharacterStats { get; set; }
        public ObservableCollection<DataProp> CharacterAttributes { get; set; }
        public ObservableCollection<DataProp> CharacterSkills { get; set; }
        public ObservableCollection<DataProp> CharacterTraits { get; set; }

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