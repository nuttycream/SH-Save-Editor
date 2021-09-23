using System;
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

            return characterNodes == null
                ? new List<Character>()
                : (from XmlNode characterNode in characterNodes select ReadCharacter(characterNode)).ToList();
        }

        private static Character ReadCharacter(XmlNode characterNode)
        {
            Character character = new()
            {
                CharacterXmlNode = characterNode,
                CharacterName = Utilities.GetAttributeValue(characterNode, NodeCollection.CharacterNameAttribute),
                FactionSide = Utilities.GetAttributeValue(characterNode, "side"),
                IsCrewman = Utilities.GetAttributeValue(characterNode, "side") != "NotSet"
            };

            if (int.TryParse(Utilities.GetAttributeValue(characterNode, NodeCollection.CharacterEidAttribute),
                out var result))
                character.CharacterEntityId = result;
            else
                throw new Exception("Error at attempt to parsing character entity id.");

            var statsNode = characterNode.SelectSingleNode(".//props");
            var attributesNode = characterNode.SelectSingleNode(".//attr");
            var traitsNode = characterNode.SelectSingleNode(".//traits");
            var skillsNode = characterNode.SelectSingleNode(".//skills");

            if (statsNode == null || attributesNode == null || traitsNode == null || skillsNode == null)
                throw new Exception("Error at attempt to find all of " + character.CharacterName + "'s nodes.");

            character.CharacterStats = ReadStats(statsNode);
            character.CharacterAttributes = ReadAttributes(attributesNode);
            character.CharacterSkills = ReadSkills(skillsNode);
            character.CharacterTraits = ReadTraits(traitsNode);

            return character;
        }

        private static ObservableCollection<DataProp> ReadStats(XmlNode xmlNode)
        {
            ObservableCollection<DataProp> characterStats = new();

            foreach (var characterStat in NodeCollection.CharacterStats)
            {
                var statNode = xmlNode.SelectSingleNode(".//" + characterStat + "[@v]");
                if (statNode == null) continue;
                if (int.TryParse(Utilities.GetAttributeValue(statNode, "v"), out var result))
                    characterStats.Add(new DataProp
                    {
                        Name = characterStat,
                        Value = result
                    });
            }

            return characterStats;
        }

        private static ObservableCollection<DataProp> ReadAttributes(XmlNode attributesNode)
        {
            ObservableCollection<DataProp> characterAttributes = new();

            foreach (var (key, value) in IdCollection.DefaultAttributeIDs)
            {
                var attributeNode = attributesNode.SelectSingleNode(".//a[@id='" + key + "']");
                if (attributeNode == null) continue;
                if (int.TryParse(Utilities.GetAttributeValue(attributeNode, "points"), out var result))
                    characterAttributes.Add(new DataProp
                    {
                        Id = key,
                        Name = value,
                        Value = result
                    });
            }

            return characterAttributes;
        }

        private static ObservableCollection<DataProp> ReadTraits(XmlNode traitsNode)
        {
            ObservableCollection<DataProp> characterTraits = new();

            foreach (var (key, value) in from trait in IdCollection.DefaultTraitIDs
                let traitNode = traitsNode.SelectSingleNode(".//t[@id='" + trait.Key + "']")
                where traitNode != null
                select trait)
                characterTraits.Add(new DataProp
                {
                    Id = key,
                    Name = value
                });
            return characterTraits;
        }

        private static ObservableCollection<DataProp> ReadSkills(XmlNode skillsNode)
        {
            ObservableCollection<DataProp> characterSkills = new();

            foreach (var (key, value) in IdCollection.DefaultSkillIDs)
            {
                var attributeNode = skillsNode.SelectSingleNode(".//s[@sk='" + key + "']");
                if (attributeNode == null) continue;
                if (int.TryParse(Utilities.GetAttributeValue(attributeNode, "level"), out var result))
                    characterSkills.Add(new DataProp
                    {
                        Id = key,
                        Name = value,
                        Value = result
                    });
            }

            return characterSkills;
        }
    }
}