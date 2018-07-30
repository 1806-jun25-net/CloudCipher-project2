using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantFrontEnd.Library.API_Models
{
    public class Query
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int Radius { get; set; }
        public DateTime QueryTime { get; set; }

        public List<string> Keywords { get; set; }

    }
}
