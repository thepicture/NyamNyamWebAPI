using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamWebAPI.Models.ResponseModels
{
    public class ResponseOrder
    {
        public ResponseOrder(Order order)
        {
            Id = order.Id;
            CustomerFullName = order.Client.Name;
            Dishes = string.Join(", ",
                                 order.OrderedDish.Select(od => od.Dish.Name));
            AppointmentDate = order.AppointedDT.ToString("d MMMM");
            TotalCost = order.OrderedDish
                             .Sum(od =>
                             {
                                 return od.Dish.FinalPriceInCents
                                        * od.ServingsNumber
                                        * 1.0
                                        / 100;
                             }) + "$";
            if (order.OrderedDish.All(od => od.StartCookingDT == null && od.EndCookingDT == null))
            {
                Status = FulfillStatuses.Waiting;
            }
            else if (order.OrderedDish.All(od => od.StartCookingDT != null && od.EndCookingDT != null))
            {
                Status = FulfillStatuses.Finished;
            }
            else
            {
                Status = FulfillStatuses.InProcess;
            }
            IsEnoughIngredients = (from o in order.OrderedDish
                                   from cs in o.Dish.CookingStage
                                   from ios in cs.IngredientOfStage
                                   where ios.Ingredient.AvailableCount < cs.IngredientOfStage.Where(s => s.IngredientId == ios.IngredientId).Sum(s => s.Quantity) * o.ServingsNumber
                                   select o).Count() == 0;
            DishesList = order.OrderedDish.ToList().ConvertAll(od => new ResponseDish(od));
        }

        public int Id;
        public string CustomerFullName;
        public string Dishes;
        public string AppointmentDate;
        public string TotalCost;
        public string Status;
        public bool IsEnoughIngredients;
        public IEnumerable<ResponseDish> DishesList;
    }
}