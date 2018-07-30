using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Blacklist = new HashSet<Blacklist>();
            Favorite = new HashSet<Favorite>();
            QueryRestaurantJunction = new HashSet<QueryRestaurantJunction>();
            RestaurantKeywordJunction = new HashSet<RestaurantKeywordJunction>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Hours { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Address { get; set; }
        public decimal? Rating { get; set; }
        public decimal? PriceLevel { get; set; }
        public string Owner { get; set; }

        public AppUser OwnerNavigation { get; set; }
        public ICollection<Blacklist> Blacklist { get; set; }
        public ICollection<Favorite> Favorite { get; set; }
        public ICollection<QueryRestaurantJunction> QueryRestaurantJunction { get; set; }
        public ICollection<RestaurantKeywordJunction> RestaurantKeywordJunction { get; set; }
    }
}
