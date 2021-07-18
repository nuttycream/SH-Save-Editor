using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using SpaceHaven_Save_Editor.ID;
using SpaceHaven_Save_Editor.Utilities;
using SpaceHaven_Save_Editor.ViewModels.Base;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            SaveSettings = new RelayCommand(Save);
            LoadSettings = new RelayCommand(Load);

            if (!SaveUtility.SaveExists("config"))
            {
                UseDefaults();
            }
        }

        private void UseDefaults()
        {
            foreach (var (key, value) in IDCollections.DefaultAttributeIDs)
                IDCollections.AttributeNodes.Add(new Node(key, value));

            foreach (var (key, value) in IDCollections.DefaultSkillIDs)
                IDCollections.SkillNodes.Add(new Node(key, value));

            foreach (var (key, value) in IDCollections.DefaultTraitIDs)
                IDCollections.TraitNodes.Add(new Node(key, value));
            
            foreach (var (key, value) in IDCollections.DefaultItemIDs)
                IDCollections.ItemNodes.Add(new Node(key, value));
        }

        private void Save()
        {
            SaveUtility.Save(IDCollections.AttributeNodes, nameof(IDCollections.AttributeNodes));
            SaveUtility.Save(IDCollections.ItemNodes, nameof(IDCollections.ItemNodes));
            SaveUtility.Save(IDCollections.TraitNodes, nameof(IDCollections.TraitNodes));
            SaveUtility.Save(IDCollections.SkillNodes, nameof(IDCollections.SkillNodes));
            
            MessageBox.Show("Saved!");
        }

        private void Load()
        {
            IDCollections.AttributeNodes = SaveUtility.Load<ObservableCollection<Node>>(nameof(IDCollections.AttributeNodes));
            IDCollections.ItemNodes = SaveUtility.Load<ObservableCollection<Node>>(nameof(IDCollections.ItemNodes));
            IDCollections.TraitNodes = SaveUtility.Load<ObservableCollection<Node>>(nameof(IDCollections.TraitNodes));
            IDCollections.SkillNodes = SaveUtility.Load<ObservableCollection<Node>>(nameof(IDCollections.SkillNodes));
            
            IDCollections.AttributeNodes.Add(new Node(1, "Test"));
            
            MessageBox.Show("Loaded!");
        }

        public ICommand SaveSettings { get; set; }
        public ICommand LoadSettings { get; set; }
    }
}