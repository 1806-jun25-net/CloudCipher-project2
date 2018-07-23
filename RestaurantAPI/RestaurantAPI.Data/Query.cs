using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class Query
    {
        public Query()
        {
            QueryKeywordJunction = new HashSet<QueryKeywordJunction>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Location2 { get; set; }
        public int? Radius { get; set; }
        public DateTime QueryTime { get; set; }
        public DateTime? ReservationTime { get; set; }

        public AppUser UsernameNavigation { get; set; }
        public ICollection<QueryKeywordJunction> QueryKeywordJunction { get; set; }
    }
}
