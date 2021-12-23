using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.Enums;
using System;
using System.Linq;

namespace NyamNyamWebAPI.Models.ResponseModels
{
    public class ResponseDish
    {
        public int Id;
        public string NameOfDish;
        public string CookingTime;
        public int Servings;
        public string Status;
        public string StartTime;
        public string FinishTime;

        public ResponseDish(OrderedDish dish)
        {
            Id = dish.DishId;
            NameOfDish = dish.Dish.Name;
            TimeSpan minutesSpan = TimeSpan.FromMinutes(dish.Dish.CookingStage.Sum(cs => cs.TimeInMinutes).Value);
            CookingTime = $"{minutesSpan:%h}h {minutesSpan:%m}min";
            Servings = dish.ServingsNumber ?? 0;
            if (dish.StartCookingDT != null)
            {
                if (dish.EndCookingDT != null)
                {
                    Status = FulfillStatuses.Finished;
                }
                else
                {
                    Status = FulfillStatuses.InProcess;
                }
            }
            else
            {
                Status = FulfillStatuses.Waiting;
            }
            StartTime = "Started at " + dish.StartCookingDT?.ToString("hh:mm");
            FinishTime = "Finished at " + dish.EndCookingDT?.ToString("hh:mm");
        }
    }
}