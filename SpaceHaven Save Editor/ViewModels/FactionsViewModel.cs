using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class FactionsViewModel : ViewModelBase
    {
        private Faction? _selectedFaction;

        public FactionsViewModel(IEnumerable<Faction> factions)
        {
            Factions = new List<Faction>(factions);
            SaveAndExit = ReactiveCommand.Create(() => Factions);
        }

        public FactionsViewModel()
        {
            Factions = new List<Faction>();
            SaveAndExit = ReactiveCommand.Create(() => Factions);
        }
        
        public ReactiveCommand<Unit, List<Faction>> SaveAndExit { get; set; }

        public List<Faction> Factions { get; set; }

        public Faction? SelectedFaction
        {
            get => _selectedFaction;
            set => this.RaiseAndSetIfChanged(ref _selectedFaction, value);
        }
    }
}