using System;
using System.Collections;
using System.Collections.Generic;
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
                throw new Exception("Something went wrong when trying to access storage nodes list.");


            foreach (XmlNode storageNode in storageNodes)
            {
                var invNodes = Utilities.FindMultipleNodes(storageNode, ".//s[@elementaryId]");
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