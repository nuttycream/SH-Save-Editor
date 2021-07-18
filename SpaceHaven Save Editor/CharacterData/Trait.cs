using System.Linq;
using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Trait
    {
        private readonly string _traitName;
        public int TraitId;

        public Trait(int traitId)
        {
            if (IDCollections.DefaultTraitIDs.TryGetValue(traitId, out var value))
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
            if (IDCollections.DefaultTraitIDs.ContainsValue(traitName))
            {
                foreach (var trait in IDCollections.DefaultTraitIDs.Where(trait => trait.Value == traitName))
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

        public override string ToString()
        {
            return _traitName;
        }
    }
}