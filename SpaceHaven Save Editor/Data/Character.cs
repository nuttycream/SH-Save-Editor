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
        public bool IsACharacterClone;

        public Character(XmlNode xmlNode)
        {
            CharacterXmlNode = xmlNode;
            CharacterName = Utilities.GetAttributeValue(CharacterXmlNode, NodeCollection.CharacterNameAttribute);
            if (int.TryParse(Utilities.GetAttributeValue(CharacterXmlNode, NodeCollection.CharacterEidNode),
                out var result))
                CharacterEntityId = result;
            else
                throw new Exception("Error at attempt to parsing character entity id.");

            var statsNode = CharacterXmlNode.SelectSingleNode(".//props");
            var attributeNodes = CharacterXmlNode.SelectNodes(".//a[@points]");
            var traitNodes = CharacterXmlNode.SelectNodes(".//t[@id]");
            var skillNodes = CharacterXmlNode.SelectNodes(".//s[@sk]");

            if (statsNode == null || attributeNodes == null || traitNodes == null || skillNodes == null)
                throw new Exception("Error at attempt to find all of " + CharacterName + " nodes.");

            CharacterStats = ReadStats(statsNode);
            CharacterAttributes = ReadAttributes(attributeNodes);
            CharacterSkills = ReadSkills(skillNodes);
            CharacterTraits = ReadTraits(traitNodes);
        }

        public XmlNode CharacterXmlNode { get; }
        public int CharacterEntityId { get; set; }
        public string CharacterName { get; set; }

        public ObservableCollection<CharacterStat> CharacterStats { get; set; }
        public ObservableCollection<CharacterAttribute> CharacterAttributes { get; set; }
        public ObservableCollection<CharacterSkill> CharacterSkills { get; set; }
        public ObservableCollection<CharacterTrait> CharacterTraits { get; set; }

        public Character CloneCharacter(string newName, int entId)
        {
            var newXmlNode = CharacterXmlNode.CloneNode(true);

            newXmlNode.Attributes![NodeCollection.CharacterNameAttribute]!.Value = newName;
            newXmlNode.Attributes![NodeCollection.CharacterEidNode]!.Value = entId.ToString();

            return new Character(newXmlNode)
            {
                IsACharacterClone = true,
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

        private static ObservableCollection<CharacterStat> ReadStats(XmlNode xmlNode)
        {
            ObservableCollection<CharacterStat> characterStats = new();

            foreach (var characterStat in NodeCollection.CharacterStats)
            {
                var statNode = xmlNode.SelectSingleNode(".//" + characterStat + "[@v]");
                if (statNode == null) continue;
                if (int.TryParse(Utilities.GetAttributeValue(statNode, "v"), out var result))
                    characterStats.Add(new CharacterStat(characterStat, result));
            }

            return characterStats;
        }

        private static ObservableCollection<CharacterAttribute> ReadAttributes(IEnumerable nodeList)
        {
            ObservableCollection<CharacterAttribute> characterAttributes = new();

            foreach (XmlNode xmlNode in nodeList)
                if (int.TryParse(Utilities.GetAttributeValue(xmlNode, "points"), out var pointsResult) &&
                    int.TryParse(Utilities.GetAttributeValue(xmlNode, "id"), out var idResult))
                    characterAttributes.Add(new CharacterAttribute(idResult, pointsResult));
            return characterAttributes;
        }

        private static ObservableCollection<CharacterTrait> ReadTraits(IEnumerable traitNodes)
        {
            ObservableCollection<CharacterTrait> characterTraits = new();

            foreach (XmlNode xmlNode in traitNodes)
                if (int.TryParse(Utilities.GetAttributeValue(xmlNode, "id"), out var idResult))
                    characterTraits.Add(new CharacterTrait(idResult));
            return characterTraits;
        }

        private static ObservableCollection<CharacterSkill> ReadSkills(IEnumerable nodeList)
        {
            ObservableCollection<CharacterSkill> characterSkills = new();

            foreach (XmlNode xmlNode in nodeList)
                if (int.TryParse(Utilities.GetAttributeValue(xmlNode, "sk"), out var idResult) &&
                    int.TryParse(Utilities.GetAttributeValue(xmlNode, "level"), out var pointsResult))
                    characterSkills.Add(new CharacterSkill(idResult, pointsResult));
            return characterSkills;
        }
    }
}