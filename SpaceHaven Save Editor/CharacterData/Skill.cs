using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Skill
    {
        public readonly int SkillId;

        public Skill(int skillId, int skillValue)
        {
            foreach (var skillNode in IDCollections.SkillNodes)
            {
                if (skillNode.ID != skillId) continue;
                SkillName = skillNode.Name;
                SkillId = skillId;
                SkillValue = skillValue;
                break;
            }
        }

        public string SkillName { get; set; }
        public int SkillValue { get; set; }
    }
}