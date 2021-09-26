using System.Xml;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public static class FindSettings
    {
        private static XmlNode? _rootGameSettingsNode;
        public static GameSettings ReadGameSettings(XmlNode rootGameSettingsNode)
        {
            GameSettings gameSettings = new();
            _rootGameSettingsNode = rootGameSettingsNode;
            
            var sandBoxNode = _rootGameSettingsNode.SelectSingleNode(".//diff[@sandbox]");
            var modeSettings = _rootGameSettingsNode.SelectSingleNode(".//modeSettings");

            if (sandBoxNode != null)
                gameSettings.SandBoxMode = Utilities.GetAttributeValue(sandBoxNode, "sandbox") == "true";

            if (modeSettings == null) return gameSettings;
            foreach (XmlAttribute modeSettingsAttribute in modeSettings.Attributes!)
                gameSettings.ModeSettings.Add(new AmountSettings(modeSettingsAttribute.Name,
                    modeSettingsAttribute.Value));

            return gameSettings;
        }

        public static void WriteGameSettings(GameSettings gameSettings)
        {
            var sandBoxNode = _rootGameSettingsNode.SelectSingleNode(".//diff[@sandbox]");
            sandBoxNode.Attributes["sandbox"].Value = gameSettings.SandBoxMode ? "true" : "false";
            
        }
    }
}