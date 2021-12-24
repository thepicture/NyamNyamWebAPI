using System.Collections.Generic;

namespace NyamNyamWebAPI.Models.ResponseModels
{
    public class ResponseIngredientGroup
    {
        public string Name;

        public ResponseIngredientGroup()
        {
        }

        public List<ResponseIngredient> Ingredients;
    }
}