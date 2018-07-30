using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class QueryRestaurantJunction
    {
        public int QueryId { get; set; }
        public string RestaurantId { get; set; }

        public Query Query { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
