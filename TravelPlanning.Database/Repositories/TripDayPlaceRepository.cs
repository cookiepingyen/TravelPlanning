using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
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
            tripDayPlaceDAO.Id = tripDayPlace.Id;

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

        public async Task<TripDaysDAO> CreateTripDayAsync(Guid TripID)
        {

            List<TripDays> tripDayList = db.TripDays.Where(x => x.Trip_id == TripID)
                .OrderBy(x => x.Date)
                .ToList();

            TripDays lastDay = tripDayList.LastOrDefault();


            DateTime nextDay = lastDay?.Date.Value.AddDays(1) ?? db.Trip.Find(TripID).Started_time;

            TripDays tripDays = new TripDays()
            {
                Id = Guid.NewGuid(),
                Trip_id = TripID,
                Date = nextDay,
                Startime = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 8, 0, 0),
            };

            db.TripDays.Add(tripDays);
            await db.SaveChangesAsync();

            TripDaysDAO tripDaysDAO = Mapper.Map<TripDays, TripDaysDAO>(tripDays);
            tripDaysDAO.Day = tripDayList.Count + 1;
            return tripDaysDAO;
        }

        public async Task DeleteTripDayAsync(Guid id)
        {
            // 刪除日期
            TripDays tripDays = await db.TripDays.FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException();
            Guid trip_id = tripDays.Trip_id;

            List<TripDayPlace> TripDayPlaces = db.TripDayPlace.Where(x => x.TripDays_id == tripDays.Id).ToList();
            db.TripDayPlace.RemoveRange(TripDayPlaces);

            db.TripDays.Remove(tripDays);

            await db.SaveChangesAsync();
            // 重新計算所有日期
            List<TripDays> TripDayList = db.TripDays.Where(x => x.Trip_id == trip_id)
                .OrderBy(x => x.Date)
                .ToList();

            DateTime? firstDate = TripDayList.First().Date;

            for (int i = 0; i < TripDayList.Count; i++)
            {
                TripDayList[i].Date = firstDate.Value.AddDays(i);
                var date = TripDayList[i].Date.Value;
                TripDayList[i].Startime = new DateTime(date.Year, date.Month, date.Day, TripDayList[i].Startime.Value.Hour, TripDayList[i].Startime.Value.Minute, 0);
            }

            await db.SaveChangesAsync();
        }

        public async Task<List<TripDaysDAO>> GetTripDaysAsync(Guid tripId)
        {
            List<TripDays> tripDays = await db.TripDays.Where(x => x.Trip_id == tripId).ToListAsync();
            List<TripDaysDAO> tripDaysDAO = Mapper.Map<TripDays, TripDaysDAO>(tripDays).ToList();
            return tripDaysDAO;
        }

    }
}
