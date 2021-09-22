using System.Collections.Generic;

namespace SpaceHaven_Save_Editor.Data
{
    public class GameSettings
    {
        public GameSettings()
        {
            ModeSettings = new List<AmountSettings>();
        }

        public bool SandBoxMode { get; set; }
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