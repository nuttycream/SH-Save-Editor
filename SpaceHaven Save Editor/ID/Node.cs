namespace SpaceHaven_Save_Editor.ID
{
    public class Node
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Node(int id, string name)
        {
            Name = name;
            ID = id;
        }
    }
}