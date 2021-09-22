using System.Xml;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindSettings
    {
        public static GameSettings ReadGameSettings(XmlNode rootGameSettingsNode)
        {
            GameSettings gameSettings = new();

            var sandBoxNode = rootGameSettingsNode.SelectSingleNode(".//diff[@sandbox]");
            var modeSettings = rootGameSettingsNode.SelectSingleNode(".//modeSettings");

            if (sandBoxNode != null)
                gameSettings.SandBoxMode = Utilities.GetAttributeValue(sandBoxNode, "sandbox") == "true";

            if (modeSettings != null)
                foreach (XmlAttribute modeSettingsAttribute in modeSettings.Attributes!)
                    gameSettings.ModeSettings.Add(new AmountSettings(modeSettingsAttribute.Name,
                        modeSettingsAttribute.Value));

            return gameSettings;
        }
    }
}