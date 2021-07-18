using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;
using SpaceHaven_Save_Editor.CharacterData;
using SpaceHaven_Save_Editor.ID;
using SpaceHaven_Save_Editor.ShipData;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public class WriteFile
    {
        private List<Character> _characters;
        private string _playerCredits;
        private string _savePath;
        private Ship _ship;
        public Action<string> ProgressList { get; set; }

        public async Task Write(string savePath, IEnumerable<Character> characters, Ship ship, string playerCredits)
        {
            _savePath = savePath;
            _characters = new List<Character>(characters);
            _ship = ship;
            _playerCredits = playerCredits;
            await Task.Run(WriteToXml);
        }

        private void WriteToXml()
        {
            AddProgress("Saving File...");
            var saveFile = new XmlDocument();
            if (_savePath != null)
                saveFile.Load(_savePath);

            var nodeList = saveFile.ChildNodes;

            WritePlayer(nodeList);
            WriteCharacter(nodeList);
            WriteStorages(saveFile);

            saveFile.Save(_savePath!);
            AddProgress("File Saved at: " + _savePath);
        }

        private void WritePlayer(XmlNodeList nodeList)
        {
            var playerBank = FindNode(nodeList, NodeCollections.PlayerBankNode);
            if (playerBank?.Attributes == null) return;
            AddProgress("Found Player Bank Node... Writing credits: " + _playerCredits);
            if (playerBank.Attributes[NodeCollections.PlayerBankAttribute] != null)
                playerBank.Attributes[NodeCollections.PlayerBankAttribute].Value = _playerCredits;
        }

        private void WriteCharacter(XmlNodeList nodeList)
        {
            foreach (var character in _characters)
            {
                var characterNode = FindNodeWithAttributeValue(nodeList, NodeCollections.CharacterNode,
                    NodeCollections.CharacterAttributeName, character.CharacterName);
                if (characterNode == null) continue;
                AddProgress("Found Character Node: " + character.CharacterName + " Writing Data... ");
                var childNodes = characterNode.ChildNodes;
                foreach (var characterStat in NodeCollections.CharacterStats)
                {
                    var statNode = FindNode(childNodes, characterStat);
                    if (statNode?.Attributes == null) continue;
                    AddProgress("Found prop node... Writing: " + characterStat);
                    if (statNode.Attributes["v"] != null)
                        statNode.Attributes["v"].Value = character.FindStat(characterStat);
                }

                var foodNode = FindNode(childNodes, NodeCollections.CharacterFoodNode);
                if (foodNode != null)
                {
                    AddProgress("Found Food Node... Writing...");
                    foreach (XmlNode node in foodNode.ChildNodes)
                        if (node.Name == NodeCollections.CharacterStoredFoodNode && node.Attributes != null)
                            foreach (XmlAttribute attribute in node.Attributes)
                                attribute.Value = character.FindFood(attribute.Name, true);

                        else if (node.Name == NodeCollections.CharacterStomachFoodNode && node.Attributes != null)
                            foreach (XmlAttribute attribute in node.Attributes)
                                attribute.Value = character.FindFood(attribute.Name, false);
                }

                //attributes
                var attributesNode = FindNode(childNodes, NodeCollections.CharacterAttributesNode);
                if (attributesNode != null)
                {
                    AddProgress("Found Attributes Node... Writing...");
                    foreach (XmlNode node in attributesNode.ChildNodes)
                        if (node.Attributes?["points"] != null)
                            node.Attributes["points"].Value = character.FindAttribute(node.Attributes["id"]?.Value);
                }

                //traits
                var traitsNode = FindNode(childNodes, NodeCollections.CharacterTraitsNode);
                if (traitsNode != null)
                {
                    AddProgress("Found Traits Node... Writing...");
                    if (traitsNode is {HasChildNodes: true}) traitsNode.RemoveAll();

                    foreach (var characterTrait in character.Traits)
                    {
                        var traitTemplate = traitsNode.OwnerDocument?.CreateElement("t");
                        traitTemplate?.SetAttribute("id", characterTrait.TraitId.ToString());
                        traitsNode.AppendChild(traitTemplate ?? throw new InvalidOperationException());
                    }
                }

                //Skills
                var skillsNode = FindNode(childNodes, NodeCollections.CharacterSkillsNode);
                if (skillsNode != null)
                {
                    AddProgress("Found Skills Node... Writing...");
                    foreach (XmlNode node in skillsNode.ChildNodes)
                    {
                        if (node.Attributes?["sk"] == null || node.Attributes["level"] == null) continue;
                        node.Attributes["level"].Value = character.FindSkill(node.Attributes["sk"].Value);
                    }
                }
            }
        }

        private void WriteStorages(XmlNode saveFile)
        {
            var storageCount = 0;
            var storageNodes = saveFile.SelectNodes(NodeCollections.StoragesXPath);
            if (storageNodes == null)
            {
                Debug.Print("Storage Nodes Not Found");
                return;
            }

            AddProgress("Found Storage Nodes... Writing...");
            foreach (XmlNode storageNode in storageNodes)
            {
                var storageFacility = _ship.StorageFacilities[storageCount];
                var inventoryNode = storageNode.FirstChild;

                if (inventoryNode is {HasChildNodes: true}) inventoryNode.RemoveAll();

                foreach (var cargo in storageFacility.CargoList)
                {
                    var itemTemplate = inventoryNode.OwnerDocument.CreateElement(NodeCollections.CargoItemElementName);
                    itemTemplate.SetAttribute(NodeCollections.CargoItemAttributeId, cargo.CargoId.ToString());
                    itemTemplate.SetAttribute(NodeCollections.CargoItemAttributeAmount, cargo.CargoAmount.ToString());
                    itemTemplate.SetAttribute("onTheWayIn", "0");
                    itemTemplate.SetAttribute("onTheWayOut", "0");
                    inventoryNode.AppendChild(itemTemplate);
                }

                storageCount++;
            }


            var toolNodes = saveFile.SelectNodes(NodeCollections.ToolsXPath);
            var toolCount = 0;
            if (toolNodes == null) return;
            AddProgress("Found Tool Nodes... Writing...");
            foreach (XmlNode toolNode in toolNodes)
            {
                if (toolNode.Attributes?["ft"] == null) continue;
                toolNode.Attributes["ft"].Value = _ship.ToolFacilities[toolCount].BuildingTools.ToString();
                toolCount++;
            }
        }

        private static XmlNode FindNode(XmlNodeList list, string nodeName)
        {
            if (list.Count <= 0) return null;
            foreach (XmlNode node in list)
            {
                if (node.Name.Equals(nodeName))
                    return node;
                if (!node.HasChildNodes) continue;
                var nodeFound = FindNode(node.ChildNodes, nodeName);
                if (nodeFound != null)
                    return nodeFound;
            }

            return null;
        }

        private static XmlNode FindNodeWithAttributeValue(XmlNodeList list, string nodeName, string attributeName,
            string attributeValue)
        {
            if (list.Count <= 0) return null;
            foreach (XmlNode node in list)
            {
                if (node.Name.Equals(nodeName) && node.Attributes?[attributeName] != null &&
                    node.Attributes?[attributeName].Value == attributeValue)
                    return node;
                if (!node.HasChildNodes) continue;
                var nodeFound = FindNodeWithAttributeValue(node.ChildNodes, nodeName, attributeName, attributeValue);
                if (nodeFound != null)
                    return nodeFound;
            }

            return null;
        }

        private void AddProgress(string progressText)
        {
            ProgressList?.Invoke(progressText);
        }
    }
}