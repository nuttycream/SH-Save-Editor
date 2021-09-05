using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.Views;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private Game? _game;
        private Character? _selectedCharacter;
        private Ship? _selectedShip;
        private StorageFacility? _selectedStorageFacility;

        public GameViewModel(Game game)
        {
            Game = game;

            OpenStorageWindow = new Interaction<StorageFacility, StorageFacility?>();
            EditCharacterWindow = new Interaction<Character, Character?>();
            CloneCharacterWindow = new Interaction<Unit, string?>();
            OpenResearchWindow = new Interaction<Research, Research?>();

            OpenStorageCommand = ReactiveCommand.CreateFromTask(OpenStorage);
            EditCharacterCommand = ReactiveCommand.CreateFromTask(OpenCharacter);
            CloneCharacterCommand = ReactiveCommand.CreateFromTask(CloneCharacter);
            OpenResearchCommand = ReactiveCommand.CreateFromTask(OpenResearch);
        }
        

        public GameViewModel()
        {

            OpenStorageWindow = new Interaction<StorageFacility, StorageFacility?>();
            EditCharacterWindow = new Interaction<Character, Character?>();
            CloneCharacterWindow = new Interaction<Unit, string?>();
            OpenResearchWindow = new Interaction<Research, Research?>();

            OpenStorageCommand = ReactiveCommand.CreateFromTask(OpenStorage);
            EditCharacterCommand = ReactiveCommand.CreateFromTask(OpenCharacter);
            CloneCharacterCommand = ReactiveCommand.CreateFromTask(CloneCharacter);
            OpenResearchCommand = ReactiveCommand.CreateFromTask(OpenResearch);
        }

        public Game? Game
        {
            get => _game;
            private init => this.RaiseAndSetIfChanged(ref _game, value);
        }

        public Ship? SelectedShip
        {
            get => _selectedShip;
            set => this.RaiseAndSetIfChanged(ref _selectedShip, value);
        }

        public Character? SelectedCharacter
        {
            get => _selectedCharacter;
            set => this.RaiseAndSetIfChanged(ref _selectedCharacter, value);
        }

        public StorageFacility? SelectedStorageFacility
        {
            get => _selectedStorageFacility;
            set => this.RaiseAndSetIfChanged(ref _selectedStorageFacility, value);
        }

        public ReactiveCommand<Unit, Unit> OpenStorageCommand { get; }
        public Interaction<StorageFacility, StorageFacility?> OpenStorageWindow { get; }
        public ReactiveCommand<Unit, Unit> EditCharacterCommand { get; }
        public Interaction<Character, Character?> EditCharacterWindow { get; }
        public ReactiveCommand<Unit, Unit> CloneCharacterCommand { get; }
        public Interaction<Unit, string?> CloneCharacterWindow { get; }
        public ReactiveCommand<Unit, Unit> OpenResearchCommand { get; }
        public Interaction<Research, Research?> OpenResearchWindow { get; }

        public void ViewShipNode()
        {
            if (SelectedShip == null) return;
            var nodeViewer = new NodeViewerWindow(SelectedShip.ShipName, SelectedShip.ShipNode);
            nodeViewer.Show();
        }

        public void ClearStorage() => SelectedStorageFacility?.Cargo.Clear();
        

        private async Task OpenCharacter()
        {
            if (_selectedCharacter == null) return;
            var result = await EditCharacterWindow.Handle(_selectedCharacter);

            if (result != null)
            {
                SelectedCharacter = result;
            }
        }

        private async Task CloneCharacter()
        {
            if (_selectedCharacter == null) return;
            var result = await CloneCharacterWindow.Handle(Unit.Default);

            if (result != null)
            {
                var newId =
                    _selectedShip!.Characters.Select(character => character.CharacterEntityId).Prepend(0).Max() + 1;
                var newCharacter = _selectedCharacter.CloneCharacter(result, newId);
                _selectedShip.Characters.Add(newCharacter);
            }
        }

        private async Task OpenStorage()
        {
            if (SelectedStorageFacility == null) return;

            var result = await OpenStorageWindow.Handle(SelectedStorageFacility);

            if (result != null)
            {
                _selectedShip!.StorageFacilities.Replace(SelectedStorageFacility, result);
            }
        }

        private async Task OpenResearch()
        {
            if (Game == null) return;

            var result = await OpenResearchWindow.Handle(Game.Research);

            if (result != null)
            {
                Game.Research = result;
            }

        }
    }
}