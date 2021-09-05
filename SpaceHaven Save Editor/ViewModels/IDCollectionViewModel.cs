using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.ViewModels
{
    public class IdCollectionViewModel : ViewModelBase
    {
        private Dictionary<int, string> _selectedIdList;
        private string? _selectedIdListText;

        public IdCollectionViewModel()
        {
            SelectedIdListText = "Select from the list";
            _selectedIdList = new Dictionary<int, string>();
            SelectedIdList = new Dictionary<int, string>
            {
                {0, "Empty"}
            };

            ShowSaveFileDialog = new Interaction<Unit, string?>();

            SaveFile = ReactiveCommand.CreateFromTask(SaveFileAsync);
        }

        public ReactiveCommand<Unit, Unit> SaveFile { get; }
        public Interaction<Unit, string?> ShowSaveFileDialog { get; }

        public string? SelectedIdListText
        {
            get => _selectedIdListText;
            set => this.RaiseAndSetIfChanged(ref _selectedIdListText, value);
        }

        public Dictionary<int, string> SelectedIdList
        {
            get => _selectedIdList;
            set => this.RaiseAndSetIfChanged(ref _selectedIdList, value);
        }

        private async Task SaveFileAsync()
        {
            var fileName = await ShowSaveFileDialog.Handle(Unit.Default);

            if (fileName == null) return;


            string idList = string.Join(Environment.NewLine, _selectedIdList);
            idList = idList.Insert(0, SelectedIdListText + Environment.NewLine);
            await File.WriteAllTextAsync(fileName, idList);
        }


        public void SwitchToResearch()
        {
            SelectedIdList = IdCollection.DefaultResearchIDs;
            SelectedIdListText = "Research";
        }

        public void SwitchToAttributes()
        {
            SelectedIdList = IdCollection.DefaultAttributeIDs;
            SelectedIdListText = "Attributes";
        }

        public void SwitchToSkills()
        {
            SelectedIdList = IdCollection.DefaultSkillIDs;
            SelectedIdListText = "Skills";
        }

        public void SwitchToItems()
        {
            SelectedIdList = IdCollection.DefaultItemIDs;
            SelectedIdListText = "Items";
        }

        public void SwitchToTraits()
        {
            SelectedIdList = IdCollection.DefaultTraitIDs;
            SelectedIdListText = "Traits";
        }
    }
}