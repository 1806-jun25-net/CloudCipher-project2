﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.API.Models
{
    public class RestaurantModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hours { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
        public decimal PriceLevel { get; set; }
        public string Owner { get; set; }

        public List<string> Keywords { get; set; }
    }
}
