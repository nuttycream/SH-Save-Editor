using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using SpaceHaven_Save_Editor.FileHandling;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class Character
    {
        public Character()
        {
            CharacterName = "";
            CharacterStats = new ObservableCollection<CharacterStat>();
            CharacterAttributes = new ObservableCollection<CharacterAttribute>();
            CharacterSkills = new ObservableCollection<CharacterSkill>();
            CharacterTraits = new ObservableCollection<CharacterTrait>();
        }

        public XmlNode? CharacterXmlNode { get; set; }
        public int CharacterEntityId { get; set; }
        public string CharacterName { get; set; }
        public bool IsAClone;
        public ObservableCollection<CharacterStat> CharacterStats { get; set; }
        public ObservableCollection<CharacterAttribute> CharacterAttributes { get; set; }
        public ObservableCollection<CharacterSkill> CharacterSkills { get; set; }
        public ObservableCollection<CharacterTrait> CharacterTraits { get; set; }

        public Character CloneCharacter(string newName, int entId)
        {
            if (CharacterXmlNode == null) return new Character();
            var newXmlNode = CharacterXmlNode.CloneNode(true);

            newXmlNode.Attributes![NodeCollection.CharacterNameAttribute]!.Value = newName;
            newXmlNode.Attributes![NodeCollection.CharacterEidAttribute]!.Value = entId.ToString();

            return new Character
            {
                CharacterXmlNode = newXmlNode,
                IsAClone = true,
                CharacterEntityId = entId,
                CharacterName = newName,
                CharacterStats = CharacterStats,
                CharacterAttributes = CharacterAttributes,
                CharacterSkills = CharacterSkills,
                CharacterTraits = CharacterTraits
            };
        }

        public override string ToString()
        {
            return CharacterName;
        }
        
    }
}