using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Interface;
using TravelPlanning.Database.Models.DAO;
using TravelPlanning.Database.Utility;

namespace TravelPlanning.Database.Repositories
{
    public class TripDayPlaceRepository : ITripDayPlaceRepository
    {
        public DatabaseContext db = new DatabaseContext();

        public async Task CreateTripDayPlaceAsync(TripDayPlaceDAO tripDayPlaceDAO)
        {
            TripDayPlace tripDayPlace = Mapper.Map<TripDayPlaceDAO, TripDayPlace>(tripDayPlaceDAO);
            tripDayPlace.Id = Guid.NewGuid();

            db.TripDayPlace.Add(tripDayPlace);
            await db.SaveChangesAsync();
        }

        public async Task DeleteTripDayPlaceAsync(Guid id)
        {
            TripDayPlace tripDayPlace = await db.TripDayPlace.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException();
            db.TripDayPlace.Remove(tripDayPlace);
            await db.SaveChangesAsync();
        }

        public async Task<List<TripDayPlaceDAO>> GetTripDayPlacesAsync(Guid tripDayId)
        {

            List<TripDayPlace> tripDayPlaces = await db.TripDayPlace.Where(x => x.TripDays_id == tripDayId).ToListAsync();
            List<TripDayPlaceDAO> tripDayPlaceDAOs = Mapper.Map<TripDayPlace, TripDayPlaceDAO>(tripDayPlaces).ToList();
            return tripDayPlaceDAOs;
        }
    }
}
