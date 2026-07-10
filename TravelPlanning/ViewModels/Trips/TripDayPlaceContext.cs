using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.ViewModels
{
    public class TripDayPlaceContext
    {
        public Guid Id { get; set; }
        public Guid TripDays_id { get; set; }
        public DateTime Travel_time { get; set; }
        public string TravelTimeText => Travel_time.ToString("HH:mm");
        public int Transit_time { get; set; }
        public int Stay_time { get; set; }
        public string Place_name { get; set; }
        public string Place_id { get; set; }
    }
}
