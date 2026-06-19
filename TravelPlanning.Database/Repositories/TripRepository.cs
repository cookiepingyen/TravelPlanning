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
        public DatabaseContext db = new DatabaseContext();
        public void CreateTrip(TripDAO tripDAO)
        {
            Trip trip = Mapper.Map<TripDAO, Trip>(tripDAO);
            trip.Ended_time = trip.Started_time.AddDays(tripDAO.Days);
            trip.Id = Guid.NewGuid();
            trip.User_id = Guid.Parse("2CB96EE9-689F-44A6-A730-C14AE767E5C8");
            db.Trip.Add(trip);
            db.SaveChanges();

        }

        public List<TripDAO> GetTrips()
        {
            Guid user_id = Guid.Parse("2CB96EE9-689F-44A6-A730-C14AE767E5C8");

            List<TripDAO> trips = db.Trip
                .Where(x => x.User_id == user_id)
                .Select(x => new TripDAO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Started_time = x.Started_time,
                    End_time = x.Ended_time,
                    Days = x.Days,
                    Cover = x.Cover

                }).OrderBy(f => f.Name).ToList();
            return trips;
        }

    }
}
