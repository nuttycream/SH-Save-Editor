using System.Diagnostics;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class CharacterStat : ReactiveObject
    {
        private int _statValue;

        public CharacterStat(string statName, int value)
        {
            if (NodeCollection.CharacterStats.Contains(statName))
            {
                StatName = statName;
                StatValue = value;
            }
            else
            {
                Debug.Print("Error Invalid Attribute ID.");

                StatName = "Stat Not Found";
                StatValue = -1;
            }
        }

        public string StatName { get; set; }

        public int StatValue
        {
            get => _statValue;
            set => this.RaiseAndSetIfChanged(ref _statValue, value);
        }
    }
}