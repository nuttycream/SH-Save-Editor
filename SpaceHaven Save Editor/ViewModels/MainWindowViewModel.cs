using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using SpaceHaven_Save_Editor.CharacterData;
using SpaceHaven_Save_Editor.FileHandling;
using SpaceHaven_Save_Editor.ShipData;
using SpaceHaven_Save_Editor.ViewModels.Base;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ReadFile _readFile;
        private readonly WriteFile _writeFile;

        private BaseViewModel _characterViewModel;
        private BaseViewModel _storageContent;
        
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
        }

        public BaseViewModel CharacterContent => _characterViewModel;
        public BaseViewModel StorageContent => _storageContent;
        
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
            await Task.Run(() => _writeFile.Write(FilePath, _characters, _ship, PlayerCredits));
        }

        private async Task Open()
        {
            _characters = await Task.Run(() => _readFile.LoadSave());
            FilePath = _readFile.FilePath;
            _ship = _readFile.Ship;
            PlayerCredits = _readFile.PlayerCredits;
            _storageContent = new StorageViewModel(ref _ship);
            _characterViewModel = new CharacterViewModel(ref _characters);
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