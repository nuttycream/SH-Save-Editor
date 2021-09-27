using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Dialogs;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.FileHandling;
using SpaceHaven_Save_Editor.Views;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ReadWrite _readWrite;
        private bool _isBusy;
        private bool _autoBackup;
        private string _fileNameTitle;
        private string? _filePath;
        private Game _game;
        private GameViewModel? _gameViewModel;
        private string _textData;

        public Action? SaveLoaded;

        public MainWindowViewModel()
        {
            ShowOpenFileDialog = new Interaction<Unit, string?>();

            OpenFile = ReactiveCommand.CreateFromTask(OpenFileAsync);

            _game = new Game();
            _readWrite = new ReadWrite();
            _readWrite.UpdateLog += UpdateLog;
            _textData = "";
            _fileNameTitle = "SpaceHaven Save Editor";
            _autoBackup = true;
        }
        
        public ReactiveCommand<Unit, Unit> OpenFile { get; }
        public Interaction<Unit, string?> ShowOpenFileDialog { get; }

        public GameViewModel? GameViewModel
        {
            get => _gameViewModel;
            set => this.RaiseAndSetIfChanged(ref _gameViewModel, value);
        }
        
        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        public string TextData
        {
            get => _textData;
            set => this.RaiseAndSetIfChanged(ref _textData, value);
        }

        public string FileNameTitle
        {
            get => _fileNameTitle;
            set => this.RaiseAndSetIfChanged(ref _fileNameTitle, value);
        }

        public bool AutoBackup
        {
            get => _autoBackup;
            set => this.RaiseAndSetIfChanged(ref _autoBackup, value);
        }

        private async Task OpenFileAsync()
        {
            IsBusy = true;
            _filePath = await ShowOpenFileDialog.Handle(Unit.Default);
            if (_filePath == null)
            {
                IsBusy = false;
                return;
            }
            
            
            UpdateLog("Parsing " + _filePath);
            try
            {
                _game = await Task.Run(() => _readWrite.ReadXmlData(_filePath));
                GameViewModel = new GameViewModel(_game);
                SaveLoaded?.Invoke();
                FileNameTitle = "Editing: " + _filePath;
            }
            catch (Exception exception)
            {
                UpdateLog(exception.Message);
            }
            IsBusy = false;
        }

        private void UpdateLog(string obj)
        {
            TextData = obj;
        }

        public async Task SaveFile()
        {
            if (_filePath == null || _readWrite.SaveFile == null) return;

            IsBusy = true;
            if (_autoBackup)
                CreateBackUp();

            try
            {
                await Task.Run(() => _readWrite.WriteXmlData());
            }
            catch (Exception exception)
            {
                UpdateLog(exception.Message);
            }
            IsBusy = false;
        }

        public void CreateBackUp()
        {
            if (!File.Exists(_filePath) || _filePath == null)
            {
                UpdateLog("There's no save to back-up " + _filePath);
                return;
            }

            var backupPath = _filePath + "-backup@" + DateTime.Now.ToString("HHmmss");
            File.Copy(_filePath, backupPath, true);
            UpdateLog("Backup Created at " + backupPath);
        }

        public void OpenIdCollections()
        {
            var idWindow = new IdCollectionView();
            idWindow.Show();
        }

        public void OpenNodeCollections()
        {
            var nodeWindow = new NodeCollectionView();
            nodeWindow.Show();
        }

        public void GotoGithub()
        {
            AboutAvaloniaDialog.OpenBrowser("https://github.com/nuttycream/SH-Save-Editor");
        }
    }
}