using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class Keyword
    {
        public Keyword()
        {
            QueryKeywordJunction = new HashSet<QueryKeywordJunction>();
            RestaurantKeywordJunction = new HashSet<RestaurantKeywordJunction>();
        }

        public string Word { get; set; }

        public ICollection<QueryKeywordJunction> QueryKeywordJunction { get; set; }
        public ICollection<RestaurantKeywordJunction> RestaurantKeywordJunction { get; set; }
    }
}
