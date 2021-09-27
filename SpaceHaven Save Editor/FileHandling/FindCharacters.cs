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
        #region Read

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

        #endregion

        #region Write

        public static void WriteCharacters(XmlNode? rootNode, IEnumerable<Character> characters)
        {
            if (rootNode == null) return;

            foreach (var character in characters)
            {
                var characterNode = rootNode.SelectSingleNode(".//c[@name='" + character.CharacterName + "']");
                if (characterNode == null && character.IsAClone)
                {
                    rootNode!.AppendChild(character.CharacterXmlNode);
                    characterNode = rootNode.SelectSingleNode(".//c[@name='" + character.CharacterName + "']");
                }
                else if (characterNode == null)
                {
                    continue;
                }

                if (character.FactionSide == "Player" && character.IsCrewman)
                {
                    characterNode.Attributes.RemoveNamedItem("oside");
                    characterNode.Attributes.RemoveNamedItem("owside");
                    characterNode.Attributes["side"].Value = "Player";
                }

                var statsNode = characterNode?.SelectSingleNode(".//props");
                var attributesNodes = characterNode?.SelectNodes(".//a[@points]");
                var traitNodesRoot = characterNode?.SelectSingleNode(".//traits");
                var skillsNodes = characterNode?.SelectNodes(".//s[@sk]");

                if (statsNode == null || attributesNodes == null || traitNodesRoot == null || skillsNodes == null)
                    throw new Exception("Error at attempt to find all of " + character.CharacterName + " nodes.");

                WriteStats(statsNode, character);
                WriteAttributes(attributesNodes, character);
                WriteTraits(traitNodesRoot, character);
                WriteSkills(skillsNodes, character);
            }
        }

        private static void WriteStats(XmlNode statNodes, Character character)
        {
            foreach (var characterStat in character.CharacterStats)
            {
                var statNode = statNodes.SelectSingleNode(".//" + characterStat.Name + "[@v]");
                if (statNode == null) continue;
                statNode.Attributes!["v"]!.Value = characterStat.Value.ToString();
            }
        }

        private static void WriteAttributes(IEnumerable attributeNodes, Character character)
        {
            foreach (XmlNode attributeNode in attributeNodes)
            {
                int.TryParse(attributeNode.Attributes!["id"]!.Value, out var attributeId);
                var attribute = character.CharacterAttributes.FirstOrDefault(s => s.Id == attributeId);
                if (attribute == null) continue;
                attributeNode.Attributes["points"]!.Value = attribute.Value.ToString();
            }
        }

        private static void WriteTraits(XmlNode traitNodesRoot, Character character)
        {
            traitNodesRoot.RemoveAll();
            foreach (var characterTrait in character.CharacterTraits)
            {
                var itemTemplate = traitNodesRoot.OwnerDocument!.CreateElement("t");
                itemTemplate.SetAttribute("id", characterTrait.Id.ToString());
                traitNodesRoot.AppendChild(itemTemplate);
            }
        }

        private static void WriteSkills(IEnumerable skillNodes, Character character)
        {
            foreach (XmlNode skillNode in skillNodes)
            {
                int.TryParse(skillNode.Attributes!["sk"]!.Value, out var skillId);
                var skill = character.CharacterSkills.FirstOrDefault(s => s.Id == skillId);
                if (skill == null) continue;
                skillNode.Attributes["level"]!.Value = skill.Value.ToString();
            }
        }

        #endregion
    }
}