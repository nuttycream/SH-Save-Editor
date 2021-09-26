using System.Collections.Generic;
using ReactiveUI;

namespace SpaceHaven_Save_Editor.Data
{
    public class GameSettings : ReactiveObject
    {
        private bool _sandBoxMode;

        public GameSettings()
        {
            ModeSettings = new List<AmountSettings>();
        }

        public bool SandBoxMode
        {
            get => _sandBoxMode;
            set => this.RaiseAndSetIfChanged(ref _sandBoxMode, value);
        }

        public List<AmountSettings> ModeSettings { get; set; }
    }

    public class AmountSettings
    {
        public AmountSettings(string name, string amount)
        {
            Name = name;
            Amount = amount;
        }

        public string Name { get; set; }
        public string Amount { get; set; }
    }
}