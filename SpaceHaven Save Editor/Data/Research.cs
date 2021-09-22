using System.Collections.ObjectModel;
using System.Diagnostics;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class Research
    {
        public Research()
        {
            ResearchItems = new ObservableCollection<ResearchItem>();
        }

        public ObservableCollection<ResearchItem> ResearchItems { get; set; }
    }

    public class ResearchItem : ReactiveObject
    {
        private int _advanced;
        private int _basic;
        private int _intermediate;
        public int MaxAdvanced;
        public int MaxBasic;
        public int MaxIntermediate;

        public ResearchItem(int researchItemId, int level1Value, int level2Value, int level3Value)
        {
            if (IdCollection.DefaultResearchIDs.TryGetValue(researchItemId, out var researchItemName))
            {
                ResearchItemId = researchItemId;
                ResearchItemName = researchItemName;

                Basic = level1Value;
                Intermediate = level2Value;
                Advanced = level3Value;
            }
            else
            {
                Debug.Print("Unknown Research ID");

                ResearchItemId = -1;
                ResearchItemName = "Unknown Research ID";

                Basic = -1;
                Intermediate = -1;
                Advanced = -1;
            }
        }

        public ResearchItem()
        {
        }

        public int ResearchItemId { get; set; }
        public string ResearchItemName { get; set; }

        public int Basic
        {
            get => _basic;
            set => this.RaiseAndSetIfChanged(ref _basic, value);
        }

        public int Intermediate
        {
            get => _intermediate;
            set => this.RaiseAndSetIfChanged(ref _intermediate, value);
        }

        public int Advanced
        {
            get => _advanced;
            set => this.RaiseAndSetIfChanged(ref _advanced, value);
        }
    }
}