using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Skill
    {
        public string SkillName { get; set; }
        public int SkillValue { get; set; }
        public readonly int SkillId;
        
        public Skill(int skillId, int skillValue)
        {
            if(IDCollections.Skills.TryGetValue(skillId, out var skillName))
            {
                SkillName = skillName;
                SkillId = skillId;
                SkillValue = skillValue;
            }
            else
            {
                SkillName = skillId + " Not Found";
                SkillId = skillId;
                SkillValue = 0;
            }
        }
    }
}