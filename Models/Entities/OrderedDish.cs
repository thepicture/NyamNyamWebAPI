//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NyamNyamWebAPI.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderedDish
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public Nullable<int> ServingsNumber { get; set; }
        public Nullable<System.DateTime> StartCookingDT { get; set; }
        public Nullable<System.DateTime> EndCookingDT { get; set; }
    
        public virtual Dish Dish { get; set; }
        public virtual Order Order { get; set; }
    }
}
