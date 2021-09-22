using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public class ReadFile
    {
        public XmlDocument? SaveFile;
        public Action<string>? UpdateLog;

        private static void ThrowNotFoundErr(string exceptionText)
        {
            throw new Exception("Could not find any " + exceptionText +
                                " nodes. Verify if the correct save file has been selected.");
        }

        public async Task<Game> ReadXmlData(string fileName)
        {
            Game gameSave = new();

            SaveFile = new XmlDocument();
            SaveFile.Load(fileName);

            var shipNodes = SaveFile.GetElementsByTagName("ship");
            var playerBankNode = SaveFile.SelectSingleNode("//playerBank");
            var researchNodes = Utilities.FindMultipleNodes(SaveFile, "//l[@techId]");
            var gameFactions = SaveFile.SelectNodes("//l[@s1]");
            var gameSettings = SaveFile.SelectSingleNode("//settings[@gm]");

            if (shipNodes.Count == 0)
                ThrowNotFoundErr("Ship");
            else if (playerBankNode == null)
                ThrowNotFoundErr("Player Bank");
            else if (researchNodes == null)
                ThrowNotFoundErr("Research");
            if (gameSettings == null)
                ThrowNotFoundErr("Game Settings");
            if (gameFactions == null)
                ThrowNotFoundErr("Factions");

            if (playerBankNode != null &&
                int.TryParse(Utilities.GetAttributeValue(playerBankNode, NodeCollection.PlayerBankAttribute),
                    out var value))
            {
                UpdateLog?.Invoke("Player bank node found with " + value);
                gameSave.Player.Money = value;
            }


            UpdateLog?.Invoke(researchNodes!.Count + " research nodes found.");
            gameSave.Research.ResearchItems = FindResearch.ReadResearchItems(researchNodes);
            UpdateLog?.Invoke(gameSave.Research.ResearchItems.Count + " research items have been verified and added.");

            UpdateLog?.Invoke(shipNodes!.Count + " ships found");
            foreach (XmlElement shipNode in shipNodes)
            {
                var shipName = Utilities.GetAttributeValue(shipNode, NodeCollection.ShipName);
                var ownedByPlayerNode = shipNode.SelectSingleNode(".//settings[@owner]");

                if (ownedByPlayerNode == null)
                    ThrowNotFoundErr("Ship Settings");

                var shipOwner =
                    Utilities.GetAttributeValue(ownedByPlayerNode!, NodeCollection.ShipOwnerAttribute);
                var shipState = Utilities.GetAttributeValue(ownedByPlayerNode!, "state");

                var characterRootNode = shipNode.SelectSingleNode(".//characters");
                var characters = new List<Character>();

                if (characterRootNode is {HasChildNodes: false} or null)
                {
                    UpdateLog?.Invoke("No Characters found on " + shipName);
                }
                else
                {
                    UpdateLog?.Invoke(characterRootNode.ChildNodes.Count + " Characters found on " + shipName);
                    characters = await Task.Run(() => FindCharacters.ReadCharacters(characterRootNode));
                }

                var storageNodes = shipNode.SelectNodes(".//feat[@eatAllowed]");
                var toolStorageNodes = shipNode.SelectNodes(".//feat[@ft]");
                List<ToolFacility> toolFacilities = new();
                List<StorageFacility> storageFacilities = new();

                if (toolStorageNodes is {Count: 0})
                {
                    UpdateLog?.Invoke("No Tool Facilities found on " + shipName);
                }
                else
                {
                    UpdateLog?.Invoke(toolStorageNodes!.Count + " Tool Facilities found on " + shipName);
                    foreach (XmlNode toolStorageNode in toolStorageNodes!)
                        if (int.TryParse(Utilities.GetAttributeValue(toolStorageNode, "ft"), out var result))
                            toolFacilities.Add(new ToolFacility(result));
                }

                if (storageNodes is {Count: 0})
                {
                    UpdateLog?.Invoke("No Storage Facilities found on " + shipName);
                }
                else
                {
                    UpdateLog?.Invoke(storageNodes!.Count + " Storage Facilities found on " + shipName);
                    storageFacilities = await Task.Run(() => FindStorages.ReadStorageFacilities(storageNodes));
                }

                Ship ship = new()
                {
                    ShipName = shipName,
                    ShipFaction = shipOwner,
                    ShipState = shipState,
                    ShipNode = shipNode,
                    Characters = new ObservableCollection<Character>(characters),
                    StorageFacilities = new ObservableCollection<StorageFacility>(storageFacilities),
                    ToolFacilities = new ObservableCollection<ToolFacility>(toolFacilities)
                };
                UpdateLog?.Invoke("Adding " + shipName + " to ships found.");
                gameSave.Ships.Add(ship);
            }

            gameSave.Factions = FindFactions.ReadFactions(gameFactions!);
            gameSave.GameSettings = FindSettings.ReadGameSettings(gameSettings!);

            UpdateLog?.Invoke("Parse Complete for " + fileName);

            return gameSave;
        }
    }
}