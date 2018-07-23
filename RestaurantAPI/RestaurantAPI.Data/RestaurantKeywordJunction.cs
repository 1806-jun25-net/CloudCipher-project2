using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class RestaurantKeywordJunction
    {
        public int RestaurantId { get; set; }
        public string Word { get; set; }

        public Restaurant Restaurant { get; set; }
        public Keyword WordNavigation { get; set; }
    }
}
