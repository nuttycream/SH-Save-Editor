using System.Diagnostics;
using ReactiveUI;
using SpaceHaven_Save_Editor.References;

namespace SpaceHaven_Save_Editor.Data
{
    public class CharacterSkill : ReactiveObject
    {
        private int _value;

        public CharacterSkill(int id, int value)
        {
            if (IdCollection.DefaultSkillIDs.TryGetValue(id, out var skillName))
            {
                ID = id;
                Name = skillName;
                Value = value;
            }
            else
            {
                Debug.Print("Error Invalid Skill ID.");

                ID = -1;
                Name = "Invalid Skill";
                Value = -1;
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public int Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
    }
}