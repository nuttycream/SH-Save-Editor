using SpaceHaven_Save_Editor.ID;

namespace SpaceHaven_Save_Editor.CharacterData
{
    public class Food
    {
        public string FoodName { get; set; }
        public float Amount { get; set; }
        
        public Food(string foodName, float amount)
        {
            if (IDCollections.Foods.Contains(foodName))
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
    }
}