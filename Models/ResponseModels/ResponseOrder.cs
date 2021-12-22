using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.Enums;
using System;
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
            AppointmentDate = order.AppointedDT;
            CreationDate = order.CreatedDT;
            TotalCost = order.OrderedDish
                             .Sum(od =>
                             {
                                 return od.Dish.FinalPriceInCents
                                        * od.ServingsNumber
                                        * 1.0
                                        / 100;
                             }) + "$";
            if (order.OrderedDish.All(od => od.EndCookingDT >= DateTime.Now))
            {
                Status = OrderStatuses.Waiting;
            }
            else if (order.OrderedDish.All(od => od.EndCookingDT < DateTime.Now))
            {
                Status = OrderStatuses.Finished;
            }
            else
            {
                Status = OrderStatuses.InProcess;
            }
            IsEnoughIngredients = (from o in order.OrderedDish
                                   from cs in o.Dish.CookingStage
                                   from ios in cs.IngredientOfStage
                                   where ios.Ingredient.AvailableCount < cs.IngredientOfStage.Where(s => s.IngredientId == ios.IngredientId).Sum(s => s.Quantity)
                                   select o).Count() == 0;
        }

        public int Id;
        public string CustomerFullName;
        public string Dishes;
        public DateTime AppointmentDate;
        public DateTime CreationDate;
        public string TotalCost;
        public string Status;
        public bool IsEnoughIngredients;
    }
}