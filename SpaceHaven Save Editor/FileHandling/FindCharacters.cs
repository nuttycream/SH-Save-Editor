using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindCharacters
    {
        public static List<Character> ReadCharacters(XmlNode characterRootNode)
        {
            var characterNodes = characterRootNode.SelectNodes(".//c[@cid]");
            
            return characterNodes == null ? new List<Character>() : (from XmlNode characterNode in characterNodes select ReadCharacter(characterNode)).ToList();
        }

        private static Character ReadCharacter(XmlNode characterNode)
        {
            Character character = new()
            {
                CharacterXmlNode = characterNode,
                CharacterName = Utilities.GetAttributeValue(characterNode, NodeCollection.CharacterNameAttribute),
                FactionSide = Utilities.GetAttributeValue(characterNode, "side"),
                isCrewman = Utilities.GetAttributeValue(characterNode, "side") != "NotSet"
            };

            if (int.TryParse(Utilities.GetAttributeValue(characterNode, NodeCollection.CharacterEidAttribute),
                out var result))
                character.CharacterEntityId = result;
            else
                throw new Exception("Error at attempt to parsing character entity id.");

            var statsNode = characterNode.SelectSingleNode(".//props");
            var attributeNodes = characterNode.SelectNodes(".//a[@points]");
            var traitNodes = characterNode.SelectNodes(".//t[@id]");
            var skillNodes = characterNode.SelectNodes(".//s[@sk]");

            if (statsNode == null || attributeNodes == null || traitNodes == null || skillNodes == null)
                throw new Exception("Error at attempt to find all of " + character.CharacterName + "'s nodes.");
            
            character.CharacterStats = ReadStats(statsNode);
            character.CharacterAttributes = ReadAttributes(attributeNodes);
            character.CharacterSkills = ReadSkills(skillNodes);
            character.CharacterTraits = ReadTraits(traitNodes);
            
            return character;
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