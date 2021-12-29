using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.ResponseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace NyamNyamWebAPI.Controllers
{
    public class CookingStagesController : ApiController
    {
        private NyamNyamEntities db = new NyamNyamEntities();

        // GET: api/CookingStages?dishId=1&orderId=1
        [ResponseType(typeof(List<ResponseCookingStage>))]
        public async Task<IHttpActionResult> GetCookingStage(int? dishId, int? orderId)
        {
            OrderedDish orderedDish = await db.OrderedDish.FindAsync(new object[] { orderId, dishId });
            if (orderedDish == null)
            {
                return NotFound();
            }

            int sequentialNumber = 1;
            bool isDishCookingInProgress = orderedDish.StartCookingDT != null && orderedDish.EndCookingDT == null;
            return base.Ok(new
            {
                RecipeName = orderedDish.Dish.Name,
                IsInProgress = isDishCookingInProgress,
                ServingsCount = orderedDish.Dish.BaseServingsQuantity,
                CookingStages = orderedDish.Dish.CookingStage.ToList().ConvertAll(cs => new ResponseCookingStage(cs, sequentialNumber++, orderedDish))
            });
        }

        // GET api/<controller>/Finish?dishId=1&orderId=1
        [Route("api/CookingStages/Finish")]
        [HttpGet]
        public IHttpActionResult IncrementQuantity(int? dishId, int? orderId)
        {
            if (dishId == null || orderId == null)
            {
                return BadRequest("attempt to make finishing of non-existing ordered dish");
            }
            OrderedDish orderedDish = db.OrderedDish.Find(new object[] { orderId, dishId});
            if (orderedDish == null)
            {
                return BadRequest("the given ordered dish was not found in the db");
            }
            orderedDish.EndCookingDT = System.DateTime.Now;
            db.SaveChanges();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}