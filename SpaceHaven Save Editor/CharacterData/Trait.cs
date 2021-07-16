using System.Linq;
using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Trait
    {
        public int TraitId;
        private string _traitName;
        public Trait(int traitId)
        {
            if (IDCollections.Traits.TryGetValue(traitId, out var value))
            {
                _traitName = value;
                TraitId = traitId;
            }
            else
            {
                _traitName = traitId + " Not Found";
                TraitId = 0;
            }
        }

        public Trait(string traitName)
        {
            if (IDCollections.Traits.ContainsValue(traitName))
            {
                foreach (var trait in IDCollections.Traits.Where(trait => trait.Value == traitName))
                {
                    TraitId = trait.Key;
                    _traitName = traitName;
                    break;
                }
            }
            else
            {
                TraitId = 0;
                _traitName = traitName + " Not Found";
            }
        }

        public override string ToString() => _traitName;
    }
}