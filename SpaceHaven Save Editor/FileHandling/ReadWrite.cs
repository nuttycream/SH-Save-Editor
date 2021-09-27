using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using SpaceHaven_Save_Editor.Data;

namespace SpaceHaven_Save_Editor.FileHandling
{
    public class ReadWrite
    {
        public XmlDocument? SaveFile;
        public Action<string>? UpdateLog;

        private string _filePath;
        private Game _gameSave;
        
        private XmlNode? _shipRootNode;
        private XmlNode? _playerBankNode;
        private XmlNode? _researchRootNode;
        private XmlNode? _factionRootNode;
        private XmlNode? _gameSettingsNode;

        private List<Task> _asynchronousTasks;

        public ReadWrite()
        {
            _filePath = "";
            _gameSave = new Game();
            _asynchronousTasks = new List<Task>();
        }

        public Game ReadXmlData(string fileName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            _filePath = fileName;
            SaveFile = new XmlDocument();
            SaveFile.Load(_filePath);

            _shipRootNode = SaveFile.SelectSingleNode("//game/ships");
            _playerBankNode = SaveFile.SelectSingleNode("//playerBank");
            _researchRootNode = SaveFile.SelectSingleNode("//research/states");
            _factionRootNode = SaveFile.SelectSingleNode("//hostmap/map");
            _gameSettingsNode = SaveFile.SelectSingleNode("//settings[@gm]");
            
            _asynchronousTasks = new List<Task>
            {
                Task.Run(() => _gameSave.Player.Money = int.Parse(Utilities.GetAttributeValue(_playerBankNode, "ca"))),
                Task.Run(() => _gameSave.Ships = FindShips.ReadShips(_shipRootNode)), 
                Task.Run(() => _gameSave.Research.ResearchItems = FindResearch.ReadResearchItems(_researchRootNode)),
                Task.Run(() => _gameSave.Factions = FindFactions.ReadFactions(_factionRootNode)),
                Task.Run(() => _gameSave.GameSettings = FindSettings.ReadGameSettings(_gameSettingsNode!))
            };
            
            Task completed = Task.WhenAll(_asynchronousTasks);
            completed.Wait();

            if (completed.Status != TaskStatus.RanToCompletion) return _gameSave;
            watch.Stop();
            UpdateLog?.Invoke("Parsing completed, it took " + watch.ElapsedMilliseconds + "ms.");

            return _gameSave;
        }
        public void WriteXmlData()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            _asynchronousTasks = new List<Task>
            {
                Task.Run(() => _playerBankNode.Attributes["ca"].Value = _gameSave.Player.Money.ToString()),
                Task.Run(() => FindShips.WriteShips(_gameSave.Ships)),
                Task.Run(() => FindFactions.WriteFactions(_gameSave.Factions)),
                Task.Run(() => FindSettings.WriteGameSettings(_gameSave.GameSettings)),
                Task.Run(() => SaveFile?.Save(_filePath))
            };
            
            Task completed = Task.WhenAll(_asynchronousTasks);
            completed.Wait();

            if (completed.Status != TaskStatus.RanToCompletion) return;
            watch.Stop();
            UpdateLog?.Invoke("Write completed, it took " + watch.ElapsedMilliseconds + "ms.");
        }
    }
}