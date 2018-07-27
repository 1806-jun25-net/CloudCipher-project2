using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantFrontEnd.Library.API_Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Hours { get; set; }
        public string Location { get; set; }
        public string Location2 { get; set; }
        public string Owner { get; set; }
    }
}
