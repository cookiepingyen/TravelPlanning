using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Database.Models.DAO
{
    public class TripDaysDAO
    {
        public Guid Id { get; set; }
        public Guid Trip_id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Startime { get; set; }
        public List<TripDayPlaceDAO> TripDayPlaces { get; set; }
    }
}
