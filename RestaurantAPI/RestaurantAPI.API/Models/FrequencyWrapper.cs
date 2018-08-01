using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.API.Models
{
    //Object to potentially be used to pass objects to the front end along with their frequency for analytics
    public class FrequencyWrapper<T>
    {
        public T Obj { get; set; }
        public int Frequency { get; set; }
    }
}
