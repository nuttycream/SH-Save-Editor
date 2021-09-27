using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindStorages
    {
        public static List<StorageFacility> ReadStorageFacilities(IEnumerable? storageNodes)
        {
            List<StorageFacility> storageFacilities = new();

            if (storageNodes == null)
                return storageFacilities;


            foreach (XmlNode storageNode in storageNodes)
            {
                var invNodes = storageNode.SelectNodes(".//s[@elementaryId]");
                if (invNodes == null) continue;

                StorageFacility storageFacility = new();
                foreach (XmlNode invNode in invNodes)
                {
                    if (!int.TryParse(Utilities.GetAttributeValue(invNode, NodeCollection.CargoItemAttributeId),
                            out var idResult) ||
                        !int.TryParse(Utilities.GetAttributeValue(invNode, NodeCollection.CargoItemAttributeAmount),
                            out var amountResult)) continue;

                    var cargoName = IdCollection.DefaultItemIDs.FirstOrDefault(c => c.Key == idResult).Value;
                    
                    DataProp cargo = new()
                    {
                        Id = idResult,
                        Name = cargoName,
                        Value = amountResult
                    };
                    storageFacility.Cargo.Add(cargo);
                }

                storageFacilities.Add(storageFacility);
            }

            return storageFacilities;
        }

        public static List<ToolFacility> ReadToolFacilities(IEnumerable? toolStorageNodes)
        {
            List<ToolFacility> toolFacilities = new();
            
            foreach (XmlNode toolStorageNode in toolStorageNodes!)
                if (int.TryParse(Utilities.GetAttributeValue(toolStorageNode, "ft"), out var result))
                    toolFacilities.Add(new ToolFacility(result));

            return toolFacilities;
        }

        public static void WriteStorageFacilities(IEnumerable? storageNodes, List<StorageFacility> storageFacilities)
        {
            if (storageNodes == null) return;
            
            var index = 0;
            foreach (XmlNode storage in storageNodes)
            {
                var invNode = storage.SelectSingleNode(".//inv");
                invNode?.RemoveAll();
                foreach (var cargo in storageFacilities[index].Cargo)
                {
                    var itemTemplate = invNode.OwnerDocument.CreateElement(NodeCollection.CargoItemElementName);
                    itemTemplate.SetAttribute(NodeCollection.CargoItemAttributeId, cargo.Id.ToString());
                    itemTemplate.SetAttribute(NodeCollection.CargoItemAttributeAmount, cargo.Value.ToString());
                    itemTemplate.SetAttribute("onTheWayIn", "0");
                    itemTemplate.SetAttribute("onTheWayOut", "0");
                    invNode.AppendChild(itemTemplate);
                }

                index++;
            }
        }

        public static void WriteToolFacilities(IEnumerable? toolStorageNodes, List<ToolFacility> toolFacilities)
        {
            if (toolStorageNodes == null) return;
            var index = 0;
            foreach (XmlNode toolStorage in toolStorageNodes)
                toolStorage.Attributes!["ft"]!.Value =
                    toolFacilities[index++].BuildingToolsAmount.ToString();
        }
    }
}