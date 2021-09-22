using System.Collections.Generic;
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
        private Ship _selectedShip;
        private List<string> _storageFacilities;
        private int _selectedStorageFacilityIndex;
        private List<string> _characterList;
        private int _selectedCharacterIndex;

        public GameViewModel(Game game)
        {
            Game = game;
            _selectedShip = game.Ships[0];
            _storageFacilities = _selectedShip.StorageFacilities.Select(s => s.StorageFacilityName).ToList();
            _characterList = _selectedShip.Characters.Select(c => c.CharacterNameToShow).ToList();

            EditFactionsCommand = ReactiveCommand.CreateFromTask(OpenFactions);
            EditHazardsCommand = ReactiveCommand.CreateFromTask(OpenHazards);
            OpenStorageCommand = ReactiveCommand.CreateFromTask(OpenStorage);
            EditCharacterCommand = ReactiveCommand.CreateFromTask(OpenCharacter);
            CloneCharacterCommand = ReactiveCommand.CreateFromTask(CloneCharacter);
            OpenResearchCommand = ReactiveCommand.CreateFromTask(OpenResearch);

            EditFactionsWindow = new Interaction<List<Faction>, List<Faction>?>();
            EditHazardsWindow = new Interaction<List<AmountSettings>, List<AmountSettings>?>();
            OpenStorageWindow = new Interaction<StorageFacility, StorageFacility?>();
            EditCharacterWindow = new Interaction<Character, Character?>();
            CloneCharacterWindow = new Interaction<Unit, string?>();
            OpenResearchWindow = new Interaction<Research, Research?>();

            
        }

        public GameViewModel()
        {
            
        }

        public Game? Game { get; }

        public Ship SelectedShip
        {
            get => _selectedShip;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedShip, value);
                StorageFacilities = SelectedShip.StorageFacilities.Select(s => s.StorageFacilityName).ToList();
                CharacterList = SelectedShip.Characters.Select(c => c.CharacterNameToShow).ToList();
            }
        }

        public List<string> CharacterList
        {
            get => _characterList;
            set => this.RaiseAndSetIfChanged(ref _characterList, value);
        }

        public int SelectedCharacterIndex
        {
            get => _selectedCharacterIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedCharacterIndex, value);
        }

        public List<string> StorageFacilities
        {
            get => _storageFacilities;
            set => this.RaiseAndSetIfChanged(ref _storageFacilities, value);
        }

        public int SelectedStorageFacilityIndex
        {
            get => _selectedStorageFacilityIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedStorageFacilityIndex, value);
        }
        
        public ReactiveCommand<Unit, Unit> EditFactionsCommand { get; }
        public ReactiveCommand<Unit, Unit> EditHazardsCommand { get; }
        
        public Interaction<List<Faction>, List<Faction>?> EditFactionsWindow { get; }
        public Interaction<List<AmountSettings>, List<AmountSettings>?> EditHazardsWindow { get; }
        
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
            var nodeViewer = new NodeViewerWindow(SelectedShip.ShipName, SelectedShip.ShipNode);
            nodeViewer.Show();
        }

        
        private async Task OpenFactions()
        {
            var result = await EditFactionsWindow.Handle(Game.Factions);
        }

        private async Task OpenHazards()
        {
            var result = await EditHazardsWindow.Handle(Game.GameSettings.ModeSettings);
        }
        
        private async Task OpenCharacter()
        {
            if (_selectedCharacterIndex == -1) return;

            var selectedCharacter = SelectedShip.Characters[_selectedCharacterIndex];
            var result = await EditCharacterWindow.Handle(selectedCharacter);

            if (result != null)
            {
                SelectedShip.Characters.Replace(selectedCharacter, result);
                CharacterList = SelectedShip.Characters.Select(c => c.CharacterNameToShow).ToList();
            }
        }

        private async Task CloneCharacter()
        {
            if (_selectedCharacterIndex == -1) return;
            var result = await CloneCharacterWindow.Handle(Unit.Default);

            if (result != null)
            {
                var selectedCharacter = SelectedShip.Characters[_selectedCharacterIndex];
                var newId =
                    _selectedShip!.Characters.Select(character => character.CharacterEntityId).Prepend(0).Max() + 1;
                var newCharacter = selectedCharacter.CloneCharacter(result, newId);
                _selectedShip.Characters.Add(newCharacter);
                CharacterList = SelectedShip.Characters.Select(c => c.CharacterNameToShow).ToList();
            }
        }

        private async Task OpenStorage()
        {
            if (_selectedStorageFacilityIndex == -1) return;

            var selectedStorageFacility = SelectedShip.StorageFacilities[_selectedStorageFacilityIndex];
            var result = await OpenStorageWindow.Handle(selectedStorageFacility);

            if (result != null)
            {
                _selectedShip!.StorageFacilities.Replace(selectedStorageFacility, result);
                StorageFacilities = SelectedShip.StorageFacilities.Select(s => s.StorageFacilityName).ToList();
            }
        }

        private async Task OpenResearch()
        {
            if (Game == null) return;

            var result = await OpenResearchWindow.Handle(Game.Research);

            if (result != null) Game.Research = result;
        }
    }
}