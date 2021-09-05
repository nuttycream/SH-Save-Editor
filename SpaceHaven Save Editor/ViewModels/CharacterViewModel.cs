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
            SaveAndExit = ReactiveCommand.Create(() => Character);
        }

        public ReactiveCommand<Unit, Character> SaveAndExit { get; }
        public Character Character { get; } = null!;
        public List<string> AllTraits { get; } = IdCollection.DefaultTraitIDs.Values.ToList();
        public string? SelectedCharacterTraitFromComboBox { get; set; }
        public CharacterTrait? SelectedCharacterTrait { get; set; }

        public void Max(string type)
        {
            switch (type)
            {
                case "Stats":
                    foreach (var characterStat in Character.CharacterStats) characterStat.StatValue = 150;
                    break;
                case "Skills":
                    foreach (var characterSkill in Character.CharacterSkills) characterSkill.SkillValue = 3;
                    break;
                case "Attributes":
                    foreach (var characterAttribute in Character.CharacterAttributes) characterAttribute.AttributeValue = 6;
                    break;
            }
        }

        public void Clear(string type)
        {
            switch (type)
            {
                case "Stats":
                    foreach (var characterStat in Character.CharacterStats) characterStat.StatValue = 0;
                    break;
                case "Skills":
                    foreach (var characterSkill in Character.CharacterSkills) characterSkill.SkillValue = 0;
                    break;
                case "Attributes":
                    foreach (var characterAttribute in Character.CharacterAttributes) characterAttribute.AttributeValue = 0;
                    break;
            }
        }
        public void ViewXmlNode()
        {
            var xmlNodeViewer = new NodeViewerWindow(Character.CharacterName, Character.CharacterXmlNode);
            xmlNodeViewer.Show();
        }

        public void AddSelectedTrait()
        {
            if (SelectedCharacterTraitFromComboBox == null) return;
            Character.CharacterTraits.Add(new CharacterTrait(IdCollection.DefaultTraitIDs.FirstOrDefault(x
                => x.Value == SelectedCharacterTraitFromComboBox).Key));
        }

        public void RemoveSelectedTrait()
        {
            if (SelectedCharacterTrait == null) return;
            Character.CharacterTraits.Remove(SelectedCharacterTrait);
        }
    }
}