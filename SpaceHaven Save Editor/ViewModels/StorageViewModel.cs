using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using SpaceHaven_Save_Editor.ID;
using SpaceHaven_Save_Editor.ShipData;
using SpaceHaven_Save_Editor.ViewModels.Base;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class StorageViewModel : BaseViewModel
    {
        public StorageViewModel()
        {

            AddToStorage = new RelayCommand(AddToStorageList);
            RemoveFromStorage = new RelayCommand(RemoveFromStorageList);
        }

        public ICommand AddToStorage { get; }
        public ICommand RemoveFromStorage { get; }
        public string SelectedItem { get; set; }
        public int SelectedItemAmount { get; set; }
        public Ship Ship { get; set; }
        public List<string> ItemList => IDCollections.GetItemList();
        public StorageFacilities SelectedStorageFacility { get; set; }
        public Cargo SelectedCargoItem { get; set; }
        public ObservableCollection<Cargo> CargoList => SelectedStorageFacility?.CargoList;
        public ToolFacilities SelectedToolFacility { get; set; }

        private void AddToStorageList()
        {
            if (SelectedStorageFacility == null)
                MessageBox.Show("Select a storage facility to add items.");
            else
                foreach (var itemNode in IDCollections.ItemNodes)
                {
                    if (itemNode.Name != SelectedItem) continue;
                    SelectedStorageFacility.CargoList.Add(new Cargo(itemNode.ID, SelectedItemAmount));
                    break;
                }
        }

        private void RemoveFromStorageList()
        {
            if (SelectedCargoItem == null || SelectedStorageFacility == null) return;

            SelectedStorageFacility.CargoList.Remove(SelectedCargoItem);
        }
    }
}