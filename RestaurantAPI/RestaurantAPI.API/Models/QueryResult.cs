using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.API.Models
{
    public class QueryResult
    {
        public QueryModel QueryObject { get; set; }
        public List<RestaurantModel> Restaurants { get; set; }
    }
}
