using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using SpaceHaven_Save_Editor.CharacterData;
using SpaceHaven_Save_Editor.FileHandling;
using SpaceHaven_Save_Editor.ID;
using SpaceHaven_Save_Editor.ShipData;
using SpaceHaven_Save_Editor.ViewModels.Base;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ReadFile _readFile;
        private readonly WriteFile _writeFile;

        public ICommand OpenFile { get; }
        public ICommand CreateBackUp { get; }
        public ICommand SaveFile { get; }
        public ICommand ClearLog { get; }
        public ICommand AddToTraits { get; }
        public ICommand RemoveTrait { get; }
        public ICommand AddToStorage { get; }
        public ICommand RemoveFromStorage { get; }
        
        public string FilePath { get; private set; }
        public string BackUpFilePath { get; private set; }
        public List<Character> Characters { get; private set; }
        public Character SelectedCharacter { get; set; }
        public List<string> TraitsList { get; }
        public string SelectedTraitFromCombobox { get; set; }
        public Trait SelectedTraitFromList { get; set; }
        
        public string SelectedItem { get; set; }
        public int SelectedItemAmount { get; set; }
        public Ship Ship { get; private set; }
        public List<string> ItemList { get; }
        public StorageFacilities SelectedStorageFacility { get; set; }
        public Cargo SelectedCargoItem { get; set; }
        public ObservableCollection<Cargo> CargoList => SelectedStorageFacility?.CargoList;
        public ToolFacilities SelectedToolFacility { get; set; }
        public string PlayerCredits { get; set; }
        public string ReadLogText { get; private set; }

        public MainWindowViewModel()
        {
            Characters = new List<Character>();
            ItemList = new List<string>(IDCollections.Items.Values.ToList());
            TraitsList = new List<string>(IDCollections.Traits.Values.ToList());
            _readFile = new ReadFile();
            _writeFile = new WriteFile();

            OpenFile = new AsyncRelayCommand(Open);
            CreateBackUp = new AsyncRelayCommand(BackUp);
            SaveFile = new AsyncRelayCommand(Save);
            ClearLog = new RelayCommand(ClearLogText);
            
            AddToTraits = new RelayCommand(AddToTraitsList);
            RemoveTrait = new RelayCommand(RemoveTraitFromList);
            
            AddToStorage = new RelayCommand(AddToStorageList);
            RemoveFromStorage = new RelayCommand(RemoveFromStorageList);
   

            _readFile.ProgressList += UpdateProgress;
            _writeFile.ProgressList += UpdateProgress;
            
        }
        
        private void AddToTraitsList()
        {
            if (SelectedTraitFromCombobox == null)
            {
                MessageBox.Show("Choose a trait from the combobox to add.");
                return;
            }
            
            var value = new Trait(SelectedTraitFromCombobox);
            SelectedCharacter?.Traits.Add(value);

        }

        private void RemoveTraitFromList()
        {
            if (SelectedTraitFromList == null)
            {
                MessageBox.Show("No Trait Selected. Select a trait from the list to remove");
                return;
            }
            SelectedCharacter?.Traits.Remove(SelectedTraitFromList);
        }
        
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

        private void UpdateProgress(string obj)
        {
            ReadLogText += obj + "\n";
        }

        private async Task Save()
        {
            await Task.Run(() => _writeFile.Write(FilePath, Characters, Ship, PlayerCredits));
        }

        private async Task Open()
        {
            Characters = await Task.Run(() => _readFile.LoadSave());
            FilePath = _readFile.FilePath;
            Ship = _readFile.Ship;
            PlayerCredits = _readFile.PlayerCredits;
        }

        private async Task BackUp()
        {
            BackUpFilePath = await Task.Run(() => _readFile.CreateBackUp());
        }

        private void ClearLogText()
        {
            ReadLogText = "Log Cleared\n";
        }
    }
}