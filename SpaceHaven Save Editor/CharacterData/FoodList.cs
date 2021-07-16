using System.Collections.Generic;
using System.Globalization;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class FoodList
    {
        private readonly string _foodType;

        public FoodList(string name)
        {
            _foodType = name;
            FoodTypeList = new List<Food>();
        }

        public List<Food> FoodTypeList { get; }

        public string FindFood(string foodName)
        {
            foreach (var food in FoodTypeList)
            {
                if (food.FoodName != foodName) continue;
                return food.Amount.ToString(CultureInfo.CurrentCulture);
            }

            return "";
        }

        public override string ToString()
        {
            return _foodType;
        }
    }
}