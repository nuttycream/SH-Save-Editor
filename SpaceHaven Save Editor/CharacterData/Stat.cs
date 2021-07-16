namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Stat
    {
        public Stat(string statName, int statValue)
        {
            StatName = statName;
            StatValue = statValue;
        }

        public string StatName { get; set; }
        public int StatValue { get; set; }
    }
}