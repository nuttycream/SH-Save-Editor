using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace SpaceHaven_Save_Editor.Utilities
{
    public static class SaveUtility
    {
        private const string SaveExtension = ".txt";
        private static readonly string SettingsFolderPath = Directory.GetCurrentDirectory() + "/settings/";

        public static bool SaveExists(string key, string saveExt = SaveExtension) => 
            File.Exists(SettingsFolderPath + key + saveExt);

        public static void Save<T>(T objectToSave, string key)
        {
            if (!Directory.Exists(SettingsFolderPath))
                Directory.CreateDirectory(SettingsFolderPath);

            var serializer = new JsonSerializerOptions {WriteIndented = true};
            var json = JsonSerializer.Serialize(objectToSave, serializer);
            File.WriteAllText(SettingsFolderPath + key + SaveExtension, json);
            Debug.Print("Saving " + key);
        }

        public static T Load<T>(string key) where T : new()
        {
            var fileText = File.ReadAllText(SettingsFolderPath + key + SaveExtension);
            var dataToLoadInto = JsonSerializer.Deserialize<T>(fileText);
            
            Debug.Print(key + " found, loading...");
            return dataToLoadInto;
        }

        private static void DeleteAllFiles()
        {
            var directory = new DirectoryInfo(SettingsFolderPath);
            directory.Delete(true);
            Directory.CreateDirectory(SettingsFolderPath);
            Debug.Print("Resetting Save Files");
        }
    }
}