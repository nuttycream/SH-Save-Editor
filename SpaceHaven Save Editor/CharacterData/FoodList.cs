using System.Collections.Generic;
using System.Globalization;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class FoodList
    {
        private readonly string _foodType;
        public List<Food> FoodTypeList { get; }

        public FoodList(string name)
        {
            _foodType = name;
            FoodTypeList = new List<Food>();
        }

        public string FindFood(string foodName)
        {
            foreach (var food in FoodTypeList)
            {
                if (food.FoodName != foodName) continue;
                return food.Amount.ToString(CultureInfo.CurrentCulture);
            }
            return "";
        }

        public override string ToString() => _foodType;
    }
}