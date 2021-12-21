using NyamNyamWebAPI.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NyamNyamWebAPI.Controllers
{
    public class OrderedDishesController : Controller
    {
        private NyamNyamEntities db = new NyamNyamEntities();

        // GET: OrderedDishes
        public ActionResult Index()
        {
            var orderedDish = db.OrderedDish.Include(o => o.Dish).Include(o => o.Order);
            return View(orderedDish.ToList());
        }

        // GET: OrderedDishes/Create
        public ActionResult Create()
        {
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name");
            ViewBag.OrderId = new SelectList(db.Order, "Id", "AppointedAddress");
            return View();
        }

        // POST: OrderedDishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,DishId,ServingsNumber,StartCookingDT,EndCookingDT")] OrderedDish orderedDish,
                                   int? orderId)
        {
            if (ModelState.IsValid)
            {
                db.OrderedDish.Add(orderedDish);
                db.Order.Find(orderId).OrderedDish.Add(orderedDish);
                db.SaveChanges();
                return RedirectToAction("Details", "Orders", new { id = orderedDish.OrderId });
            }

            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", orderedDish.DishId);
            ViewBag.OrderId = new SelectList(db.Order, "Id", "AppointedAddress", orderedDish.OrderId);
            return View(orderedDish);
        }

        // GET: OrderedDishes/Edit/5
        public ActionResult Edit(int? orderId, int? dishId)
        {
            if (orderId == null || dishId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderedDish orderedDish = db.OrderedDish.Find(new object[] { orderId, dishId });
            if (orderedDish == null)
            {
                return HttpNotFound();
            }
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", orderedDish.DishId);
            ViewBag.OrderId = new SelectList(db.Order, "Id", "AppointedAddress", orderedDish.OrderId);
            return View(orderedDish);
        }

        // POST: OrderedDishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,DishId,ServingsNumber,StartCookingDT,EndCookingDT")] OrderedDish orderedDish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderedDish).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Orders", new { id = orderedDish.OrderId });
            }
            ViewBag.DishId = new SelectList(db.Dish, "Id", "Name", orderedDish.DishId);
            ViewBag.OrderId = new SelectList(db.Order, "Id", "AppointedAddress", orderedDish.OrderId);
            return View(orderedDish);
        }

        // GET: OrderedDishes/Delete/5
        public ActionResult Delete(int? orderId, int? dishId)
        {
            if (orderId == null || dishId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderedDish orderedDish = db.OrderedDish.Find(new object[] { orderId, dishId });
            if (orderedDish == null)
            {
                return HttpNotFound();
            }
            return View(orderedDish);
        }

        // POST: OrderedDishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? orderId, int? dishId)
        {
            OrderedDish orderedDish = db.OrderedDish.Find(new object[] { orderId, dishId });
            db.OrderedDish.Remove(orderedDish);
            db.SaveChanges();
            return RedirectToAction("Details", "Orders", new { id = orderId });
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
