using System.Collections.Generic;
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

            //UseDefaults();
        }

        public ICommand SaveSettings { get; set; }
        public ICommand LoadSettings { get; set; }

        private void UseDefaults()
        {
            IDCollections.SetToDefaults();
        }

        private void Save()
        {
            SaveUtility.Save(IDCollections.GetIdList(), "ids");

            MessageBox.Show("Saved!");
        }

        private void Load()
        {
            IDCollections.SetIdList(SaveUtility.Load<Dictionary<string, ObservableCollection<Node>>>("ids"));
        }
    }
}