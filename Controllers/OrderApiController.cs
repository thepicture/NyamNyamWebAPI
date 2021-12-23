using Newtonsoft.Json;
using NyamNyamWebAPI.Models.Entities;
using NyamNyamWebAPI.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;

namespace NyamNyamWebAPI.Controllers
{
    public class OrderApiController : ApiController
    {
        private NyamNyamEntities db = new NyamNyamEntities();
        private readonly TimeSpan oldWorldSkillsExerciseDateSubtraction =
            TimeSpan.FromDays(365 * 2);


        // GET api/orderapi
        [SuppressMessage("Style", "IDE0037:Use inferred member name", Justification = "Name of an array is needed for DataContractJsonSerializer")]
        public IHttpActionResult Get()
        {
            DateTime nowOrAfterDate = DateTime.Now
                                      - oldWorldSkillsExerciseDateSubtraction;
            var orders = db.Order.Where(o => o.AppointedDT >= nowOrAfterDate)
                                .OrderByDescending(o => o.AppointedDT)
                                .ToList()
                                .ConvertAll(o => new ResponseOrder(o));
            return Ok(new
            {
                Orders = orders
            });
        }

        // GET api/orderapi/5
        public IHttpActionResult Get(int id)
        {
            return Ok(db.Order.Find(id));
        }

        // POST api/orderapi
        public void Post([FromBody] string value)
        {
        }

        // PUT api/orderapi/5
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}