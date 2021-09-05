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
        public Action<string>? UpdateLog;
        public XmlDocument? SaveFile;

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
            foreach (XmlNode researchNode in researchNodes!)
            {
                var blocksNode = researchNode.SelectSingleNode(".//blocksDone");
                if (blocksNode == null)
                    continue;

                if (!int.TryParse(Utilities.GetAttributeValue(researchNode, "techId"), out var idResult)
                    || !int.TryParse(Utilities.GetAttributeValue(blocksNode, "level1"), out var level1Result)
                    || !int.TryParse(Utilities.GetAttributeValue(blocksNode, "level1"), out var level2Result)
                    || !int.TryParse(Utilities.GetAttributeValue(blocksNode, "level1"), out var level3Result))
                    continue;

                if (!IdCollection.DefaultResearchIDs.ContainsKey(idResult))
                    continue;

                ResearchItem researchItem = new(idResult, level1Result, level2Result, level3Result);
                gameSave.Research.ResearchItems.Add(researchItem);
            }

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

                var characterNodes = shipNode.SelectSingleNode(".//characters");
                List<Character> characters = new();

                if (characterNodes is {HasChildNodes: false})
                {
                    UpdateLog?.Invoke("No Characters found on " + shipName);
                }
                else
                {
                    UpdateLog?.Invoke(characterNodes?.ChildNodes.Count + " Characters found on " + shipName);
                    characters.AddRange(from XmlNode characterNode in characterNodes! select new Character(characterNode));
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
                    storageFacilities = await Task.Run(() => ReadStorageFacilities(storageNodes));
                }

                Ship ship = new(shipName, characters, storageFacilities, isOwnedByPlayer, shipNode);
                UpdateLog?.Invoke("Adding " + shipName + " to ships found.");
                gameSave.Ships.Add(ship);
            }

            UpdateLog?.Invoke("Parse Complete for " + fileName);
            
            return gameSave;
        }

        //TODO: Move this to Ship.cs
        private static List<StorageFacility> ReadStorageFacilities(IEnumerable? storageNodes)
        {
            List<StorageFacility> storageFacilities = new();

            if (storageNodes == null)
                throw new Exception("Something went wrong when trying to access storage nodes list.");


            foreach (XmlNode storageNode in storageNodes)
            {
                var invNodes = Utilities.FindMultipleNodes(storageNode, "//s[@elementaryId]");
                if (invNodes == null) continue;

                StorageFacility storageFacility = new();
                foreach (XmlNode invNode in invNodes)
                {
                    if (!int.TryParse(Utilities.GetAttributeValue(invNode, NodeCollection.CargoItemAttributeId),
                            out var idResult) ||
                        !int.TryParse(Utilities.GetAttributeValue(invNode, NodeCollection.CargoItemAttributeAmount),
                            out var amountResult)) continue;

                    Cargo cargo = new(idResult, amountResult);
                    storageFacility.Cargo.Add(cargo);
                }

                storageFacilities.Add(storageFacility);
            }

            return storageFacilities;
        }
    }
}