using System.Diagnostics;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class CharacterTrait
    {
        public CharacterTrait(int id)
        {
            if (IdCollection.DefaultTraitIDs.TryGetValue(id, out var traitName))
            {
                ID = id;
                Name = traitName;
            }
            else
            {
                Debug.Print("Error Invalid Trait ID.");

                ID = -1;
                Name = "Invalid Trait";
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}