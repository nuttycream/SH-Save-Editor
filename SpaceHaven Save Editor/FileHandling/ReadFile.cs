using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Win32;
using SpaceHaven_Save_Editor.CharacterData;
using SpaceHaven_Save_Editor.ID;
using SpaceHaven_Save_Editor.ShipData;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public class ReadFile
    {
        private List<Character> _characters = new();
        private int _storageCount, _toolCount;
        public Action<string> ProgressList { get; set; }
        public string FilePath { get; private set; }
        public string PlayerCredits { get; set; }
        public Ship Ship { get; set; }

        public async Task<List<Character>> LoadSave()
        {
            Ship = new Ship();
            _storageCount = 0;
            _toolCount = 0;
            var fileDialog = new OpenFileDialog
            {
                Title = "Open Save File",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            var result = fileDialog.ShowDialog();
            if (result != true) return _characters;
            try
            {
                FilePath = fileDialog.FileName;
                _characters = await Task.Run(() => ReadXmlData(fileDialog));
            }
            catch (Exception except)
            {
                ProgressList?.Invoke("Error: Invalid Save File\n" + except);
            }

            return _characters;
        }
        
        private async Task<List<Character>> ReadXmlData(OpenFileDialog dialogData)
        {
            var fileStream = dialogData.OpenFile();
            var settings = new XmlReaderSettings {Async = true};
            var reader = XmlReader.Create(fileStream, settings);
            var doc = new XmlDocument();
            var characters = new List<Character>();

            if (reader.Settings == null) return characters;
            
            while (await reader.ReadAsync())
            {
                
                if (reader.Name.Equals("c") && reader.GetAttribute("name") != null)
                {
                    var name = reader.GetAttribute("name");
                    var characterNode = doc.ReadNode(reader);
                    AddProgress("Found Character Node for " + name + ". Reading inner text.");
                    characters.Add(await Task.Run(() => HandleCharacterNode(characterNode, name)));
                }

                if (reader.Name.Equals("playerBank") && reader.GetAttribute("ca") != null)
                {
                    var amount = reader.GetAttribute("ca");
                    PlayerCredits = amount;
                }

                if (reader.Name.Equals("feat") && reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.GetAttribute("eatAllowed") != null)
                    {
                        AddProgress("Found Storage Node.");
                        var storageNode = doc.ReadNode(reader);
                        Ship.StorageFacilities.Add(await Task.Run(() => ReadStorage(storageNode)));
                    }

                    if (reader.GetAttribute("ft") == null) continue;
                    {
                        AddProgress("Found Tools Node.");
                        var toolNode = doc.ReadNode(reader);
                        Ship.ToolFacilities.Add(await Task.Run(() => ReadTools(toolNode)));
                    }
                }
            }
            
            reader.Dispose();
            return characters;
        }

        private ToolFacilities ReadTools(XmlNode storageNode)
        {
            var newToolFacility = new ToolFacilities(_toolCount++)
            {
                BuildingTools = (int) GetValueFloat(storageNode, "ft")
            };
            
            return newToolFacility;
        }

        private StorageFacilities ReadStorage(XmlNode storageNode)
        {
            var newStorageFacility = new StorageFacilities(_storageCount++)
                {CargoList = new ObservableCollection<Cargo>()};
            foreach (XmlNode invNode in storageNode.ChildNodes)
            {
                if (!invNode.HasChildNodes) continue;
                foreach (XmlNode cargo in invNode.ChildNodes)
                {
                    if (cargo.NodeType != XmlNodeType.Element || cargo.Attributes == null) continue;
                    newStorageFacility.CargoList.Add(
                        new Cargo((int) GetValueFloat(cargo, "elementaryId"),
                            (int) GetValueFloat(cargo, "inStorage")));
                }
            }

            return newStorageFacility;
        }

        private async Task<Character> HandleCharacterNode(XmlNode node, string name)
        {
            var tasks = new List<Task>();
            var newCharacter = new Character(name);
            foreach (XmlNode rootNode in node.ChildNodes)
            {
                switch (rootNode.Name)
                {
                    case "props":
                        tasks.Add(Task.Run(() => ReadStats(rootNode, ref newCharacter)));
                        AddProgress("Found 'props' node.");
                        break;
                    case "pers":
                        tasks.Add(Task.Run(() => ReadPers(rootNode, ref newCharacter)));
                        AddProgress("Found 'pers' node.");
                        break;
                }
            }

            await Task.WhenAll(tasks);

            return newCharacter;
        }

        private void ReadPers(XmlNode rootNode, ref Character newCharacter)
        {
            foreach (XmlNode persNode in rootNode.ChildNodes)
            {
                switch (persNode.Name)
                {
                    case "attr":
                    {
                        AddProgress("Found 'attr' node. Adding attributes to character.");
                        foreach (XmlNode attributesNode in persNode.ChildNodes)
                        {
                            if (attributesNode.Attributes?["id"] == null) continue;
                            newCharacter.AddAttribute((int) GetValueFloat(attributesNode, "id"),
                                (int) GetValueFloat(attributesNode, "points"));
                        }

                        break;
                    }
                    case "traits":
                    {
                        AddProgress("Found 'traits' node. Adding traits to character.");
                        foreach (XmlNode traitNode in persNode.ChildNodes)
                        {
                            if (traitNode.Name != "t") continue;
                            newCharacter.AddTrait((int) GetValueFloat(traitNode, "id"));
                        }

                        break;
                    }
                    case "skills":
                    {
                        AddProgress("Found 'skills' node. Adding skills to character.");
                        foreach (XmlNode skillsNode in persNode.ChildNodes)
                            if (skillsNode.Attributes?["level"] != null)
                                newCharacter.AddSkill((int) GetValueFloat(skillsNode, "sk"),
                                    (int) GetValueFloat(skillsNode, "level"));
                        break;
                    }
                }
            }
        }

        private void ReadStats(XmlNode rootNode, ref Character newCharacter)
        {
            foreach (XmlNode propsNode in rootNode.ChildNodes)
            {
                if (propsNode.NodeType == XmlNodeType.Whitespace) continue;
                foreach (var characterStat in IDCollections.CharacterStats)
                {
                    if (characterStat != propsNode.Name) continue;
                    newCharacter.AddStats(propsNode.Name, (int) GetValueFloat(propsNode, "v"));
                    break;
                }

                if (propsNode.Name != "Food") continue;
                AddProgress("Found 'Food' node.");
                foreach (XmlNode foodNode in propsNode.ChildNodes)
                {
                    switch (foodNode.Name)
                    {
                        case "stored":
                            AddProgress("Found 'Food' sub node 'stored'.");
                            ReadFood(ref newCharacter, foodNode, true);
                            break;
                        case "belly":
                            AddProgress("Found 'Food' sub node 'belly'.");
                            ReadFood(ref newCharacter, foodNode, false);
                            break;
                    }
                }
            }
        }

        private static void ReadFood(ref Character newCharacter, XmlNode foodNode, bool stored)
        {
            var foodAttributeCollection = foodNode.Attributes;
            if (foodAttributeCollection == null) return;
            foreach (XmlAttribute foodAttribute in foodAttributeCollection)
            {
                foreach (var food in IDCollections.Foods)
                {
                    if (foodAttribute.Name != food) continue;
                    newCharacter.AddFood(foodAttribute.Name, float.Parse(foodAttribute.Value), stored);
                    break;
                }
            }
        }

        private static float GetValueFloat(XmlNode node, string attributeName)
        {
            var valueAttribute = node.Attributes?[attributeName];
            if (valueAttribute == null) 
                return 0.0f;
            var value = valueAttribute.Value;
            return float.Parse(value);
        }

        private void AddProgress(string progressText) => ProgressList?.Invoke(progressText);

        public string CreateBackUp()
        {
            var backupPath = FilePath + "-backup";
            File.Copy(FilePath, FilePath + "-backup", true);
            AddProgress("Backup Created at " + backupPath);
            return backupPath;
        }
    }
}