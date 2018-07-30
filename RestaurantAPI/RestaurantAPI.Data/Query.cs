using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class Query
    {
        public Query()
        {
            QueryKeywordJunction = new HashSet<QueryKeywordJunction>();
            QueryRestaurantJunction = new HashSet<QueryRestaurantJunction>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int? Radius { get; set; }
        public DateTime QueryTime { get; set; }

        public AppUser UsernameNavigation { get; set; }
        public ICollection<QueryKeywordJunction> QueryKeywordJunction { get; set; }
        public ICollection<QueryRestaurantJunction> QueryRestaurantJunction { get; set; }
    }
}
