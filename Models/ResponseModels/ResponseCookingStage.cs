using NyamNyamWebAPI.Models.Entities;
using System.Linq;

namespace NyamNyamWebAPI.Models.ResponseModels
{
    public class ResponseCookingStage
    {
        public int SequentialNumber;
        public double CookingTimeInMinutes;
        public ResponseCookingStageIngredient[] Ingredients;
        public string Description;
        public int OrderId;
        public int DishId;

        public ResponseCookingStage(CookingStage stage,
            int sequentialNumber,
            OrderedDish dish)
        {
            SequentialNumber = sequentialNumber;
            CookingTimeInMinutes = stage.TimeInMinutes ?? 0;
            Ingredients = stage.IngredientOfStage
                .Select(ios => new ResponseCookingStageIngredient
                {
                    Name = ios.Ingredient.Name,
                    Quantity = ios.Quantity,
                    Unit = ios.Ingredient.Unit.Name
                })
            .Distinct()
            .ToArray();
            Description = stage.ProcessDescription;
            OrderId = dish.OrderId;
            DishId = dish.DishId;
        }
    }
}