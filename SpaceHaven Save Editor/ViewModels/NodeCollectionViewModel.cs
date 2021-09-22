using System;
using System.Collections.Generic;
using System.Reflection;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class NodeCollectionViewModel : ViewModelBase
    {
        public NodeCollectionViewModel()
        {
            NodeCollection = new List<string>();
            Type nodeCollectionType = typeof(NodeCollection);
            PropertyInfo[] properties =
                nodeCollectionType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
                NodeCollection.Add(property.Name + ": " + property.GetValue(nodeCollectionType, null));
        }

        public List<string> NodeCollection { get; set; }
    }
}