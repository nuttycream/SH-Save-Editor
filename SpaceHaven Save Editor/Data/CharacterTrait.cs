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
                TraitId = id;
                TraitName = traitName;
            }
            else
            {
                Debug.Print("Error Invalid Trait ID.");

                TraitId = -1;
                TraitName = "Invalid Trait";
            }
        }

        public int TraitId { get; set; }
        public string TraitName { get; set; }

        public override string ToString()
        {
            return TraitName;
        }
    }
}