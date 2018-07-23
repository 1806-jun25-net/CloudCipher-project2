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
            RestaurantKeywordJunction = new HashSet<RestaurantKeywordJunction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Hours { get; set; }
        public string Location { get; set; }
        public string Location2 { get; set; }
        public string Owner { get; set; }

        public AppUser OwnerNavigation { get; set; }
        public ICollection<Blacklist> Blacklist { get; set; }
        public ICollection<Favorite> Favorite { get; set; }
        public ICollection<RestaurantKeywordJunction> RestaurantKeywordJunction { get; set; }
    }
}
