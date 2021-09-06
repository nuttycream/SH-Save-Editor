using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var playerBankNode = Utilities.FindNode(SaveFile, NodeCollection.PlayerBankNode);
            var researchNodes = Utilities.FindMultipleNodes(SaveFile, "//l[@techId]");

            if (shipNodes.Count == 0)
                ThrowNotFoundErr("Ship");
            else if (playerBankNode == null)
                ThrowNotFoundErr("Player Bank");
            else if (researchNodes == null)
                ThrowNotFoundErr("Research");

            if (playerBankNode != null &&
                int.TryParse(Utilities.GetAttributeValue(playerBankNode, NodeCollection.PlayerBankAttribute),
                    out var value))
            {
                UpdateLog?.Invoke("Player bank node found with " + value);
                gameSave.Player.Money = value;
            }
            

            UpdateLog?.Invoke(researchNodes!.Count + " research nodes found.");
            gameSave.Research.ResearchItems = ReadResearch.ReadResearchItems(researchNodes);

            UpdateLog?.Invoke(gameSave.Research.ResearchItems.Count + " research items have been verified and added.");

            UpdateLog?.Invoke(shipNodes!.Count + " ships found");
            foreach (XmlElement shipNode in shipNodes)
            {
                var shipName = Utilities.GetAttributeValue(shipNode, NodeCollection.ShipName);
                var ownedByPlayerNode = Utilities.FindNode(shipNode, NodeCollection.ShipSettings,
                    NodeCollection.ShipOwnerAttribute);

                if (ownedByPlayerNode == null)
                    ThrowNotFoundErr("Ship Settings");

                var isOwnedByPlayer =
                    Utilities.GetAttributeValue(ownedByPlayerNode!, NodeCollection.ShipOwnerAttribute) == "Player";

                var characterRootNode = shipNode.SelectSingleNode(".//characters");
                var characters = new List<Character>();

                if (characterRootNode is {HasChildNodes: false} or null)
                    UpdateLog?.Invoke("No Characters found on " + shipName);
                else
                {
                    UpdateLog?.Invoke(characterRootNode.ChildNodes.Count + " Characters found on " + shipName);
                    characters = await Task.Run(() => Characters.ReadCharacters(characterRootNode));
                }

                var storageNodes = Utilities.FindMultipleNodes(shipNode, NodeCollection.StoragesXPath);
                List<StorageFacility> storageFacilities = new();

                if (storageNodes is {Count: 0})
                {
                    UpdateLog?.Invoke("No Storage Facilities found on " + shipName);
                }
                else
                {
                    UpdateLog?.Invoke(storageNodes!.Count + " Storage Facilities found on " + shipName);
                    storageFacilities = await Task.Run(() => Storages.ReadStorageFacilities(storageNodes));
                }

                Ship ship = new(shipName, characters, storageFacilities, isOwnedByPlayer, shipNode);
                UpdateLog?.Invoke("Adding " + shipName + " to ships found.");
                gameSave.Ships.Add(ship);
            }

            UpdateLog?.Invoke("Parse Complete for " + fileName);

            return gameSave;
        }
    }
}