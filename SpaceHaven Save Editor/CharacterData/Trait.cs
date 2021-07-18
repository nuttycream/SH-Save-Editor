using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Trait
    {
        private readonly string _traitName;
        public int TraitId;

        public Trait(int traitId)
        {
            foreach (var traitNode in IDCollections.TraitNodes)
            {
                if (traitNode.ID != traitId) continue;
                _traitName = traitNode.Name;
                TraitId = traitId;
                break;
            }
        }

        public Trait(string traitName)
        {
            foreach (var traitNode in IDCollections.TraitNodes)
            {
                if (traitNode.Name != traitName) continue;
                _traitName = traitName;
                TraitId = traitNode.ID;
                break;
            }
        }

        public override string ToString()
        {
            return _traitName;
        }
    }
}