using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;

namespace NyamNyamWebAPI.Controllers
{
    public class OrderApiController : ApiController
    {
        private NyamNyamEntities db = new NyamNyamEntities();
        private readonly TimeSpan oldWorldSkillsExerciseDateSubtraction =
            TimeSpan.FromDays(365 * 2);


        // GET api/orderapi
        [SuppressMessage("Style", "IDE0037:Use inferred member name", Justification = "Name of an array is needed for DataContractJsonSerializer")]
        public IHttpActionResult Get()
        {
            DateTime nowOrAfterDate = DateTime.Now
                                      - oldWorldSkillsExerciseDateSubtraction;
            var orders = db.Order.Where(o => o.AppointedDT >= nowOrAfterDate)
                                .OrderByDescending(o => o.AppointedDT)
                                .ToList()
                                .ConvertAll(o => new ResponseOrder(o));
            return Ok(new
            {
                Orders = orders
            });
        }

        // GET api/orderapi/5
        public IHttpActionResult Get(int id)
        {
            return Ok(db.Order.Find(id));
        }

        // GET api/orderapi/1/ingredients
        [Route("api/orderapi/{id}/ingredients")]
        public IHttpActionResult GetIngredients(int? id)
        {
            if (db.Order.Find(id) == null)
            {
                return BadRequest("order was not found in the database");
            }
            var order = db.Order.Find(id);
            var ingredients = new List<ResponseIngredient>();

            foreach (var dish in order.OrderedDish)
            {
                foreach (var stage in dish.Dish.CookingStage)
                {
                    foreach (var stageIngredient in stage.IngredientOfStage)
                    {
                        var currentIngredient = ingredients.FirstOrDefault(i => i.Id == stageIngredient.Ingredient.Id);
                        if (currentIngredient != null)
                        {
                            currentIngredient.RequiredQuantity += stageIngredient.Quantity;
                            currentIngredient.PriceInCents = Convert.ToInt32(Math.Ceiling(currentIngredient.RequiredQuantity * currentIngredient.PricePerUnit));
                            currentIngredient.IsAvailable = currentIngredient.RequiredQuantity <= currentIngredient.CountInStock;
                        }
                        else
                        {
                            currentIngredient = new ResponseIngredient(stageIngredient);
                            ingredients.Add(currentIngredient);
                        }
                    }
                }
            }

            var groups = new List<ResponseIngredientGroup>();
            var inStockGroup = new ResponseIngredientGroup
            {
                Name = "In stock",
                Ingredients = new List<ResponseIngredient>(),
            };
            var notAvailableGroup = new ResponseIngredientGroup
            {
                Name = "Not available",
                Ingredients = new List<ResponseIngredient>(),
            };


            foreach (var ingredient in ingredients)
            {
                if (ingredient.IsAvailable)
                {
                    inStockGroup.Ingredients.Add(ingredient);
                }
                else
                {
                    if (ingredient.CountInStock > 0)
                    {
                        inStockGroup.Ingredients.Add(ingredient);
                    }
                    notAvailableGroup.Ingredients.Add(ingredient);
                }
            }

            groups.Add(inStockGroup);
            groups.Add(notAvailableGroup);

            return Ok(new { GroupList = groups });
        }

        // POST api/orderapi
        public void Post([FromBody] string value)
        {
        }

        // PUT api/orderapi/5
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}