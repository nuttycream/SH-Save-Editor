using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using SpaceHaven_Save_Editor.CharacterData;
using SpaceHaven_Save_Editor.ID;
using SpaceHaven_Save_Editor.ViewModels.Base;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class CharacterViewModel : BaseViewModel
    {
        public CharacterViewModel()
        {
            Characters = new List<Character>();
            TraitsList = new List<string>();
            AddToTraits = new RelayCommand(AddToTraitsList);
            RemoveTrait = new RelayCommand(RemoveTraitFromList);

            foreach (var traitNode in IDCollections.TraitNodes) TraitsList.Add(traitNode.Name);
        }

        public ICommand AddToTraits { get; }
        public ICommand RemoveTrait { get; }
        public List<Character> Characters { get; set; }
        public Character SelectedCharacter { get; set; }
        public List<string> TraitsList { get; }
        public string SelectedTraitFromCombobox { get; set; }
        public Trait SelectedTraitFromList { get; set; }


        private void AddToTraitsList()
        {
            if (SelectedTraitFromCombobox == null)
            {
                MessageBox.Show("Choose a trait from the combobox to add.");
                return;
            }

            var value = new Trait(SelectedTraitFromCombobox);
            SelectedCharacter?.Traits.Add(value);
        }

        private void RemoveTraitFromList()
        {
            if (SelectedTraitFromList == null)
            {
                MessageBox.Show("No Trait Selected. Select a trait from the list to remove");
                return;
            }

            SelectedCharacter?.Traits.Remove(SelectedTraitFromList);
        }
    }
}