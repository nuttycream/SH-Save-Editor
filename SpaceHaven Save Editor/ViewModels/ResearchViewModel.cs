using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class ResearchViewModel : ViewModelBase
    {
        private Research _research;
        public ResearchViewModel(Research research)
        {
            _research = research;
            SaveAndExit = ReactiveCommand.Create(() => _research);
        }

        public ResearchViewModel()
        {
            _research = new Research();
            SaveAndExit = ReactiveCommand.Create(() => _research);
        }

        public Research Research
        {
            get => _research;
            set => this.RaiseAndSetIfChanged(ref _research, value);
        }

        public ReactiveCommand<Unit, Research> SaveAndExit { get; }

        public void SetAllToMax()
        {

            foreach (var researchItem in Research.ResearchItems)
            {
                researchItem.Basic = 999;
                researchItem.Advanced = 999;
                researchItem.Intermediate = 999;
            }
        }
        
        public void SetAllToMin()
        {

            foreach (var researchItem in Research.ResearchItems)
            {
                researchItem.Basic = 0;
                researchItem.Advanced = 0;
                researchItem.Intermediate = 0;
            }
        }
    }
}