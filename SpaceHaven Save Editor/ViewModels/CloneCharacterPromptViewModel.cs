using System.Reactive;
using ReactiveUI;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class CloneCharacterPromptViewModel : ViewModelBase
    {
        public CloneCharacterPromptViewModel()
        {
            Continue = ReactiveCommand.Create(() => CharacterName);
        }
        
        public ReactiveCommand<Unit, string> Continue { get; set; }
        public string CharacterName { get; set; } = "Enter New Character Name";


    }
}