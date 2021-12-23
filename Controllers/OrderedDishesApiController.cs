using NyamNyamWebAPI.Models.Entities;
using System;
using System.Web.Http;

namespace NyamNyamWebAPI.Controllers
{
    public class OrderedDishesApiController : ApiController
    {
        private NyamNyamEntities db = new NyamNyamEntities();
        // GET api/ordereddishesapi/startcooking/?orderid=1&dishid=1
        [ActionName("startcooking")]
        [HttpGet]
        public IHttpActionResult StartCookingOrderedDish(int? orderId,
                        int? dishId)
        {
            if (orderId == null || dishId == null)
            {
                return BadRequest("order id and dish id must not be null");
            }
            var currentDate = DateTime.Now;
            var orderedDish = db.OrderedDish.Find(new object[] { orderId, dishId });
            orderedDish.StartCookingDT = currentDate;
            db.SaveChanges();
            return Ok();
        }
    }
}