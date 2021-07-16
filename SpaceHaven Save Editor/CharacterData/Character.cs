using System.Collections.Generic;
using System.Collections.ObjectModel;
using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Character
    {
        public Character(string characterName)
        {
            CharacterName = characterName;
            Attributes = new List<AttributeData>();
            Skills = new List<Skill>();
            Traits = new ObservableCollection<Trait>();
            Stats = new List<Stat>();

            FoodLists = new List<FoodList>
            {
                new("Stored"),
                new("Belly")
            };
        }

        public string CharacterName { get; }
        public List<Stat> Stats { get; }
        public FoodList SelectedFoodList { get; set; }
        public List<FoodList> FoodLists { get; }
        public List<Skill> Skills { get; }
        public List<AttributeData> Attributes { get; }
        public ObservableCollection<Trait> Traits { get; }

        public void AddTrait(int id)
        {
            if (IDCollections.Traits.ContainsKey(id)) Traits.Add(new Trait(id));
        }

        public void AddAttribute(int attributeId, int newValue)
        {
            foreach (var (i, value) in IDCollections.Attributes)
            {
                if (i != attributeId) continue;
                Attributes.Add(new AttributeData(i, value, newValue));
                break;
            }
        }

        public string FindAttribute(string attributeId)
        {
            foreach (var attribute in Attributes)
                if (attribute.AttributeId == int.Parse(attributeId))
                    return attribute.AttributeValue.ToString();

            return "";
        }

        public string FindFood(string foodName, bool isStored)
        {
            var foodList = isStored ? FoodLists[0] : FoodLists[1];
            return foodList.FindFood(foodName);
        }

        public void AddFood(string foodName, float value, bool isStored)
        {
            var foodList = isStored ? FoodLists[0] : FoodLists[1];
            foodList.FoodTypeList.Add(new Food(foodName, value));
        }

        public void AddStats(string nodeName, int value)
        {
            Stats.Add(new Stat(nodeName, value));
        }

        public string FindStat(string statName)
        {
            foreach (var stat in Stats)
            {
                if (stat.StatName != statName) continue;
                return stat.StatValue.ToString();
            }

            return "";
        }

        public void AddSkill(int skillId, int skillValue)
        {
            Skills.Add(new Skill(skillId, skillValue));
        }

        public string FindSkill(string skillId)
        {
            foreach (var skill in Skills)
            {
                if (skill.SkillId.ToString() != skillId) continue;
                return skill.SkillValue.ToString();
            }

            return "";
        }

        public override string ToString()
        {
            return CharacterName;
        }
    }
}