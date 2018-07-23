using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class QueryKeywordJunction
    {
        public int QueryId { get; set; }
        public string Word { get; set; }

        public Query Query { get; set; }
        public Keyword WordNavigation { get; set; }
    }
}
