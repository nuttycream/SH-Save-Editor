using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using SpaceHaven_Save_Editor.FileHandling;
using SpaceHaven_Save_Editor.ShipData;
using SpaceHaven_Save_Editor.ViewModels.Base;
using Character = SpaceHaven_Save_Editor.CharacterData.Character;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ReadFile _readFile;
        private readonly WriteFile _writeFile;

        private CharacterViewModel _characterViewModel;
        private StorageViewModel _storageViewModel;
        private AboutViewModel _aboutViewModel;
        
        private List<Character> _characters = new();
        private Ship _ship;
        
        public MainWindowViewModel()
        {
            _readFile = new ReadFile();
            _writeFile = new WriteFile();

            OpenFile = new AsyncRelayCommand(Open);
            CreateBackUp = new AsyncRelayCommand(BackUp);
            SaveFile = new AsyncRelayCommand(Save);
            ClearLog = new RelayCommand(ClearLogText);


            _readFile.ProgressList += UpdateProgress;
            _writeFile.ProgressList += UpdateProgress;
            
            _characterViewModel = new CharacterViewModel();
            _storageViewModel = new StorageViewModel();
            _aboutViewModel = new AboutViewModel();
        }

        public BaseViewModel CharacterContent => _characterViewModel;
        public BaseViewModel StorageViewModel => _storageViewModel;
        public BaseViewModel AboutViewModel => _aboutViewModel;
        
        public ICommand OpenFile { get; }
        public ICommand CreateBackUp { get; }
        public ICommand SaveFile { get; }
        public ICommand ClearLog { get; }

        public string FilePath { get; private set; }
        public string BackUpFilePath { get; private set; }
        public string PlayerCredits { get; set; }
        public string ReadLogText { get; private set; }
        

        private void UpdateProgress(string obj)
        {
            ReadLogText += obj + "\n";
        }

        private async Task Save()
        {
            _characterViewModel.Characters = _characters;
            _storageViewModel.Ship = _ship;
            await Task.Run(() => _writeFile.Write(FilePath, _characters, _ship, PlayerCredits));
        }

        private async Task Open()
        {
            _characters = await Task.Run(() => _readFile.LoadSave());
            FilePath = _readFile.FilePath;
            _ship = _readFile.Ship;
            PlayerCredits = _readFile.PlayerCredits;
            _characterViewModel.Characters = _characters;
            _storageViewModel.Ship = _ship;
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