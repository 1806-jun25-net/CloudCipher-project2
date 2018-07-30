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
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int? Radius { get; set; }
        public DateTime QueryTime { get; set; }

        public List<string> Keywords { get; set; }
    }
}
