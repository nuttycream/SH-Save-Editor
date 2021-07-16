namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Stat
    {
        public string StatName { get; set; }
        public int StatValue { get; set; }
        
        public Stat(string statName, int statValue)
        {
            StatName = statName;
            StatValue = statValue;
        }
    }
}