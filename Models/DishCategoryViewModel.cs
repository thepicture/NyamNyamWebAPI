using NyamNyamWebAPI.Models.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NyamNyamWebAPI.Models
{
    public class DishCategoryViewModel
    {
        public List<Dish> Dishes { get; set; }
        public SelectList Categories { get; set; }
        public string SearchString { get; set; }
        public string Category { get; set; }
    }
}