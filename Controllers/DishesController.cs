using NyamNyamWebAPI.Models;
using NyamNyamWebAPI.Models.Entities;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NyamNyamWebAPI.Controllers
{
    public class DishesController : Controller
    {
        private NyamNyamEntities db = new NyamNyamEntities();

        // GET: Dishes
        public ActionResult Index(string nameSearchText,
                                  string category,
                                  string priceFromText,
                                  string priceToText)
        {
            var dishes = from d in db.Dish
                         select d;
            var categoryList = dishes.Select(d => d.Category.Name).Distinct();
            ViewBag.category = new SelectList(categoryList);

            if (!string.IsNullOrWhiteSpace(nameSearchText))
            {
                dishes = dishes.Where(d => d.Name.Contains(nameSearchText));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                dishes = dishes.Where(d => d.Category.Name == category);
            }

            if (double.TryParse(priceFromText, out var priceFrom)
                && double.TryParse(priceToText, out var priceTo)
                && priceFrom <= priceTo)
            {
                dishes = from d in dishes
                         where d.FinalPriceInCents > priceFrom * 100
                               && d.FinalPriceInCents < priceTo * 100
                         select d;
            }

            var dishCategoryVM = new DishCategoryViewModel
            {
                Categories = new SelectList(db.Category.ToList().Select(c => c.Name)),
                Dishes = dishes.ToList()
            };

            return View(dishCategoryVM);
        }

        // GET: Dishes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dish.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
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
