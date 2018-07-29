using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class AppUser
    {
        public AppUser()
        {
            Blacklist = new HashSet<Blacklist>();
            Favorite = new HashSet<Favorite>();
            Query = new HashSet<Query>();
            Restaurant = new HashSet<Restaurant>();
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Blacklist> Blacklist { get; set; }
        public ICollection<Favorite> Favorite { get; set; }
        public ICollection<Query> Query { get; set; }
        public ICollection<Restaurant> Restaurant { get; set; }
    }
}
