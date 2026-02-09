using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Interface;
using TravelPlanning.Database.Utility;

namespace TravelPlanning.Database.Repositories
{
    public class TripRepository : ITripRepository
    {

        public void CreateTrip(TripDAO tripDAO)
        {
            DatabaseContext db = new DatabaseContext();
            Trip trip = Mapper.Map<TripDAO, Trip>(tripDAO);
            trip.Ended_time = trip.Started_time.Value.AddDays(tripDAO.Days);
            trip.Id = Guid.NewGuid();
            trip.User_id = Guid.Parse("2CB96EE9-689F-44A6-A730-C14AE767E5C8");
            db.Trip.Add(trip);
            db.SaveChanges();

        }

    }
}
