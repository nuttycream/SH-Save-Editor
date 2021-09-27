using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using SpaceHaven_Save_Editor.Data;
using SpaceHaven_Save_Editor.References;
using SpaceHaven_Save_Editor.Views;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class CharacterViewModel : ViewModelBase
    {
        public CharacterViewModel(Character character)
        {
            Character = character;
            SaveAndExit = ReactiveCommand.Create(() => Character);
        }

        public CharacterViewModel()
        {
            Character = new Character();
            SaveAndExit = ReactiveCommand.Create(() => Character);
        }

        public ReactiveCommand<Unit, Character> SaveAndExit { get; }
        public Character Character { get; }

        public bool isPlayerFaction => Character.FactionSide == "Player";

        public List<string> AllTraits { get; } = IdCollection.DefaultTraitIDs.Values.ToList();
        public string? SelectedCharacterTraitFromComboBox { get; set; }
        public int SelectedCharacterTrait { get; set; }

        public void Max(string type)
        {
            switch (type)
            {
                case "Stats":
                    foreach (var characterStat in Character.CharacterStats) characterStat.Value = 150;
                    break;
                case "Skills":
                    foreach (var characterSkill in Character.CharacterSkills) characterSkill.Value = 3;
                    break;
                case "Attributes":
                    foreach (var characterAttribute in Character.CharacterAttributes) characterAttribute.Value = 6;
                    break;
            }
        }

        public void SetToCrewman()
        {
            Character.IsCrewman = true;
        }

        public void SetFaction()
        {
            Character.FactionSide = "Player";
        }

        public void ViewXmlNode()
        {
            var xmlNodeViewer = new NodeViewerWindow(Character.CharacterName, Character.CharacterXmlNode!);
            xmlNodeViewer.Show();
        }

        public void AddSelectedTrait()
        {
            if (SelectedCharacterTraitFromComboBox == null) return;

            var newTrait = IdCollection.DefaultTraitIDs.FirstOrDefault(x
                => x.Value == SelectedCharacterTraitFromComboBox);

            Character.CharacterTraits.Add(new DataProp
            {
                Id = newTrait.Key,
                Name = newTrait.Value
            });
        }

        public void RemoveSelectedTrait()
        {
            if (SelectedCharacterTrait == -1) return;
            Character.CharacterTraits.RemoveAt(SelectedCharacterTrait);
        }
    }
}