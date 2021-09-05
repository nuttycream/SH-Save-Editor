using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;
using SpaceHaven_Save_Editor.Views;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class StorageViewModel : ViewModelBase
    {
        private readonly IEnumerable<Cargo> _unmodifiedCargoItems;
        private string? _cargoItemComboboxSelection;
        private int _cargoItemComboboxSelectionAmount;
        private int _selectedCargoIndex;

        public StorageViewModel(StorageFacility storageFacility)
        {
            StorageFacility = storageFacility;
            _unmodifiedCargoItems = new List<Cargo>(StorageFacility.Cargo);

            SaveAndExit = ReactiveCommand.Create(() => StorageFacility);
        }

        public StorageViewModel()
        {
            StorageFacility = new StorageFacility();
            _unmodifiedCargoItems = new List<Cargo>();

            SaveAndExit = ReactiveCommand.Create(() => StorageFacility);
        }

        public ReactiveCommand<Unit, StorageFacility> SaveAndExit { get; set; }
        public StorageFacility StorageFacility { get; }

        public int SelectedCargoIndex
        {
            get => _selectedCargoIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedCargoIndex, value);
        }

        public List<string> AllCargoItems { get; } = new(IdCollection.DefaultItemIDs.Values.ToList());

        public string? CargoItemComboboxSelection
        {
            get => _cargoItemComboboxSelection;
            set => this.RaiseAndSetIfChanged(ref _cargoItemComboboxSelection, value);
        }

        public int CargoItemComboboxSelectionAmount
        {
            get => _cargoItemComboboxSelectionAmount;
            set => this.RaiseAndSetIfChanged(ref _cargoItemComboboxSelectionAmount, value);
        }

        public void AddCargoItem()
        {
            if (CargoItemComboboxSelection == null || CargoItemComboboxSelectionAmount <= 0) return;

            var idResult = IdCollection.DefaultItemIDs.FirstOrDefault(x => x.Value == CargoItemComboboxSelection).Key;

            StorageFacility.Cargo.Insert(0, new Cargo(idResult, CargoItemComboboxSelectionAmount));
        }

        public void RemoveCargoItem()
        {
            if (SelectedCargoIndex < 0) return;

            StorageFacility.Cargo.RemoveAt(SelectedCargoIndex);
        }

        public void RemoveAllCargo()
        {
            StorageFacility.Cargo.Clear();
        }

        public void Restore()
        {
            StorageFacility.Cargo.Clear();
            foreach (var cargoItem in _unmodifiedCargoItems)
                StorageFacility.Cargo.Add(cargoItem);
        }

        public void OpenIDReference()
        {
            var idWindow = new IdCollectionView();
            idWindow.Show();
        }
    }
}