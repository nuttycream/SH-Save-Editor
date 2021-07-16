namespace SpaceHaven_Save_Editor.CharacterData
{
    /*
     NOTE: LIMIT CAN BE UP TO 100 HOWEVER ARBITRARY GAME LIMITS ONLY UP TO 6
     0 - 6 
    id="210" Bravery
    id="212" Zest
    id="213" Intelligence
    id="214" Perception 
    */
    public class AttributeData
    {
        public AttributeData(int id, string name, int value)
        {
            AttributeId = id;
            AttributeName = name;
            AttributeValue = value;
        }

        public int AttributeId { get; }
        public string AttributeName { get; set; }
        public int AttributeValue { get; set; }
    }
}