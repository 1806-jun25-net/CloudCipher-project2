using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class Favorite
    {
        public string RestaurantId { get; set; }
        public string Username { get; set; }

        public Restaurant Restaurant { get; set; }
        public AppUser UsernameNavigation { get; set; }
    }
}
