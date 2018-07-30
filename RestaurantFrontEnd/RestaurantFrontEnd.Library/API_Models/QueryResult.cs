using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantFrontEnd.Library.API_Models
{
    public class QueryResult
    {
        public Query QueryObject { get; set; }
        public List<string> Restaurants { get; set; }

    }
}
