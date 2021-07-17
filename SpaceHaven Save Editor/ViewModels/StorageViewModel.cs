using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            ItemList = new List<string>(IDCollections.Items.Values.ToList());

            AddToStorage = new RelayCommand(AddToStorageList);
            RemoveFromStorage = new RelayCommand(RemoveFromStorageList);
        }

        public ICommand AddToStorage { get; }
        public ICommand RemoveFromStorage { get; }
        public string SelectedItem { get; set; }
        public int SelectedItemAmount { get; set; }
        public Ship Ship { get; set; }
        public List<string> ItemList { get; }
        public StorageFacilities SelectedStorageFacility { get; set; }
        public Cargo SelectedCargoItem { get; set; }
        public ObservableCollection<Cargo> CargoList => SelectedStorageFacility?.CargoList;
        public ToolFacilities SelectedToolFacility { get; set; }

        private void AddToStorageList()
        {
            foreach (var (key, value) in IDCollections.Items)
            {
                if (SelectedStorageFacility == null)
                    break;
                if (value != SelectedItem) continue;
                SelectedStorageFacility.CargoList.Add(new Cargo(key, SelectedItemAmount));
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