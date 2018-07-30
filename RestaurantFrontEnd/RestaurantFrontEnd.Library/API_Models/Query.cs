using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantFrontEnd.Library.API_Models
{
    public class Query
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Location2 { get; set; }
        public int Radius { get; set; }
        public DateTime QueryTime { get; set; }
        public DateTime ReservationTime { get; set; }

        public List<string> Keywords { get; set; }

    }
}
