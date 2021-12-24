using NyamNyamWebAPI.Models.Entities;
using System.Collections.Generic;
using System.Web.Http;

namespace NyamNyamWebAPI.Controllers
{
    public class IngredientsApiController : ApiController
    {
        public NyamNyamEntities db = new NyamNyamEntities();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // GET api/<controller>/5/increment?incrementValue=1.2
        [Route("api/ingredientsapi/{ingredientId}/increment")]
        [HttpGet]
        public IHttpActionResult IncrementQuantity(double? incrementValue,
                                                   int ingredientId)
        {
            if (incrementValue == null || incrementValue <= 0)
            {
                return BadRequest("attempt to make nonsense increment of the quantity");
            }
            Ingredient ingredient = db.Ingredient.Find(ingredientId);
            if (ingredient == null)
            {
                return BadRequest("ingredient was not found in db. " +
                    "an ingredient's id should be an existing element to increment.");
            }
            ingredient.AvailableCount += incrementValue.Value;
            db.SaveChanges();
            return Ok(ingredient.AvailableCount);
        }
    }
}