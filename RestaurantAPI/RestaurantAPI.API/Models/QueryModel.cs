using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.API.Models
{
    public class QueryModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Location2 { get; set; }
        public int Radius { get; set; }
        public DateTime QueryTime { get; set; }
        public DateTime ReservationTime { get; set; }

        public List<string> Keywords { get; set; }
    }
}
