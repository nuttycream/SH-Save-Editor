using System;
using System.Linq;
using System.Xml;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public class WriteFile
    {
        public Action<string>? UpdateLog;

        private static void ThrowNotFoundErr(string exceptionText)
        {
            throw new Exception("Could not find any " + exceptionText +
                                " nodes. Verify if the correct save file has been selected.");
        }

        public void WriteXmlData(Game game, ref XmlDocument xmlDocument, string savePath)
        {
            var shipNodes = xmlDocument.GetElementsByTagName("ship");
            var playerBankNode = Utilities.FindNode(xmlDocument, NodeCollection.PlayerBankNode);
            var researchNodes = Utilities.FindMultipleNodes(xmlDocument, "//l[@techId]");

            if (shipNodes.Count == 0)
                ThrowNotFoundErr("Ship");
            else if (playerBankNode == null)
                ThrowNotFoundErr("Player Bank");
            else if (researchNodes == null)
                ThrowNotFoundErr("Research");


            UpdateLog?.Invoke("Updating player bank node with " + game.Player.Money);
            playerBankNode!.Attributes![NodeCollection.PlayerBankAttribute]!.Value = game.Player.Money.ToString();

            UpdateLog?.Invoke("Updating " + game.Research.ResearchItems.Count + " research nodes.");
            foreach (XmlNode researchNode in researchNodes!)
            {
                var blocksNode = researchNode.SelectSingleNode(".//blocksDone");
                if (blocksNode == null)
                    continue;

                int.TryParse(researchNode!.Attributes!["techId"]!.Value, out var idResult);
                var researchItem = game.Research.ResearchItems.FirstOrDefault(r
                    => r.ResearchItemId.Equals(idResult));
                if (researchItem == null) continue;
                blocksNode.Attributes!["level1"]!.Value = researchItem.Basic.ToString();
                blocksNode.Attributes["level2"]!.Value = researchItem.Intermediate.ToString();
                blocksNode.Attributes["level3"]!.Value = researchItem.Advanced.ToString();
            }

            foreach (XmlNode shipNode in shipNodes)
            {
                var shipName = shipNode.Attributes![NodeCollection.ShipName]!.Value;
                var ship = game.Ships.FirstOrDefault(s => s.ShipName.Equals(shipName));
                if (ship == null) continue;

                UpdateLog?.Invoke("Writing ship " + ship.ShipName);

                var storages = shipNode.SelectNodes(NodeCollection.StoragesXPath);
                if (storages != null)
                {
                    UpdateLog?.Invoke("Writing " + storages.Count + " storage facilities on " + shipName);
                    WriteStorages(storages, ship);
                }

                var characterRootNode = shipNode.SelectSingleNode(".//characters");
                if (!characterRootNode!.HasChildNodes) continue;

                var cloneCount = 0;
                foreach (var character in ship.Characters)
                {
                    var characterNode = shipNode.SelectSingleNode(".//c[@name='" + character.CharacterName + "']");
                    if (characterNode == null && character.IsAClone)
                    {
                        characterRootNode!.AppendChild(character.CharacterXmlNode);
                        characterNode = shipNode.SelectSingleNode(".//c[@name='" + character.CharacterName + "']");
                        cloneCount++;
                    }
                    else if (characterNode == null)
                    {
                        continue;
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

                UpdateLog?.Invoke("Written " + ship.Characters.Count + " with " + cloneCount + " new characters");
            }


            xmlDocument.Save(savePath);
            UpdateLog?.Invoke("File Saved at " + savePath);
        }

        private void WriteStorages(XmlNodeList storages, Ship ship)
        {
            var index = 0;
            foreach (XmlNode storage in storages)
            {
                var invNode = storage.SelectSingleNode(".//inv");
                invNode?.RemoveAll();
                foreach (var cargo in ship.StorageFacilities[index].Cargo)
                {
                    var itemTemplate = invNode.OwnerDocument.CreateElement(NodeCollection.CargoItemElementName);
                    itemTemplate.SetAttribute(NodeCollection.CargoItemAttributeId, cargo.CargoId.ToString());
                    itemTemplate.SetAttribute(NodeCollection.CargoItemAttributeAmount, cargo.CargoAmount.ToString());
                    itemTemplate.SetAttribute("onTheWayIn", "0");
                    itemTemplate.SetAttribute("onTheWayOut", "0");
                    invNode.AppendChild(itemTemplate);
                }

                index++;
            }
        }

        private void WriteStats(XmlNode statNodes, Character character)
        {
            foreach (var characterStat in character.CharacterStats)
            {
                var statNode = statNodes.SelectSingleNode(".//" + characterStat.StatName + "[@v]");
                if (statNode == null) continue;
                statNode.Attributes!["v"]!.Value = characterStat.StatValue.ToString();
            }
        }

        private void WriteAttributes(XmlNodeList attributeNodes, Character character)
        {
            foreach (XmlNode attributeNode in attributeNodes)
            {
                int.TryParse(attributeNode.Attributes!["id"]!.Value, out var attributeId);
                var attribute = character.CharacterAttributes.FirstOrDefault(s => s.AttributeId == attributeId);
                if (attribute == null) continue;
                attributeNode.Attributes["points"]!.Value = attribute.AttributeValue.ToString();
            }
        }

        private void WriteTraits(XmlNode traitNodesRoot, Character character)
        {
            traitNodesRoot.RemoveAll();
            foreach (var characterTrait in character.CharacterTraits)
            {
                var itemTemplate = traitNodesRoot.OwnerDocument!.CreateElement("t");
                itemTemplate.SetAttribute("id", characterTrait.TraitId.ToString());
                traitNodesRoot.AppendChild(itemTemplate);
            }
        }

        private void WriteSkills(XmlNodeList skillNodes, Character character)
        {
            foreach (XmlNode skillNode in skillNodes)
            {
                int.TryParse(skillNode.Attributes!["sk"]!.Value, out var skillId);
                var skill = character.CharacterSkills.FirstOrDefault(s => s.SkillId == skillId);
                if (skill == null) continue;
                skillNode.Attributes["level"]!.Value = skill.SkillValue.ToString();
            }
        }
    }
}