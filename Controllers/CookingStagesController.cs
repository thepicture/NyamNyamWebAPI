using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.ResponseModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace NyamNyamWebAPI.Controllers
{
    public class CookingStagesController : ApiController
    {
        private NyamNyamEntities db = new NyamNyamEntities();

        // GET: api/CookingStages
        public IQueryable<CookingStage> GetCookingStage()
        {
            return db.CookingStage;
        }

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
            return Ok(new
            {
                RecipeName = orderedDish.Dish.Name,
                ServingsCount = orderedDish.Dish.BaseServingsQuantity,
                CookingStages = orderedDish.Dish.CookingStage.ToList().ConvertAll(cs => new ResponseCookingStage(cs, sequentialNumber++, orderedDish))
            });
        }

        // PUT: api/CookingStages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCookingStage(int id, CookingStage cookingStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cookingStage.Id)
            {
                return BadRequest();
            }

            db.Entry(cookingStage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CookingStageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CookingStages
        [ResponseType(typeof(CookingStage))]
        public async Task<IHttpActionResult> PostCookingStage(CookingStage cookingStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CookingStage.Add(cookingStage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cookingStage.Id }, cookingStage);
        }

        // DELETE: api/CookingStages/5
        [ResponseType(typeof(CookingStage))]
        public async Task<IHttpActionResult> DeleteCookingStage(int id)
        {
            CookingStage cookingStage = await db.CookingStage.FindAsync(id);
            if (cookingStage == null)
            {
                return NotFound();
            }

            db.CookingStage.Remove(cookingStage);
            await db.SaveChangesAsync();

            return Ok(cookingStage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CookingStageExists(int id)
        {
            return db.CookingStage.Count(e => e.Id == id) > 0;
        }
    }
}