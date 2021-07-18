using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Food
    {
        public Food(string foodName, string amount)
        {
            if (NodeCollections.Foods.Contains(foodName))
            {
                FoodName = foodName;
                Amount = amount;
            }
            else
            {
                FoodName = foodName + " Not Found";
                Amount = amount;
            }
        }

        public string FoodName { get; set; }
        public string Amount { get; set; }
    }
}