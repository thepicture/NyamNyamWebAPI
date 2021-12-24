using NyamNyamWebAPI.Models.Entities;
using System;

namespace NyamNyamWebAPI.Models.ResponseModels
{
    public class ResponseIngredient
    {
        public int Id { get; set; }
        public string IngredientName { get; set; }
        public int PriceInCents { get; set; }
        public string UnitText { get; set; }
        public double RequiredQuantity { get; set; }
        public double CountInStock { get; set; }
        public bool IsAvailable { get; set; }
        public double PricePerUnit { get; set; }
        public ResponseIngredient(IngredientOfStage stageIngredient)
        {
            Id = stageIngredient.IngredientId;
            IngredientName = stageIngredient.Ingredient.Name;
            PriceInCents = Convert.ToInt32(Math.Ceiling(stageIngredient.Quantity * stageIngredient.Ingredient.CostInCents));
            UnitText = stageIngredient.Ingredient.Unit.Name;
            RequiredQuantity = stageIngredient.Quantity;
            CountInStock = stageIngredient.Ingredient.AvailableCount;
            IsAvailable = CountInStock >= RequiredQuantity;
            PricePerUnit = stageIngredient.Ingredient.CostInCents;
        }
    }
}