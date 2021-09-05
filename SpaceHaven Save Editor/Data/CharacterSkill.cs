using System.Diagnostics;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class CharacterSkill : ReactiveObject
    {
        private int _skillValue;

        public CharacterSkill(int id, int value)
        {
            if (IdCollection.DefaultSkillIDs.TryGetValue(id, out var skillName))
            {
                SkillId = id;
                SkillName = skillName;
                SkillValue = value;
            }
            else
            {
                Debug.Print("Error Invalid Skill ID.");

                SkillId = -1;
                SkillName = "Invalid Skill";
                SkillValue = -1;
            }
        }

        public int SkillId { get; set; }
        public string SkillName { get; set; }

        public int SkillValue
        {
            get => _skillValue;
            set => this.RaiseAndSetIfChanged(ref _skillValue, value);
        }
    }
}