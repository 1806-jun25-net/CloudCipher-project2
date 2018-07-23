using System;
using System.Collections.Generic;

namespace RestaurantAPI.Data
{
    public partial class Blacklist
    {
        public int RestaurantId { get; set; }
        public string Username { get; set; }

        public Restaurant Restaurant { get; set; }
        public AppUser UsernameNavigation { get; set; }
    }
}
