using AutoMapper.QueryableExtensions;
using AutoMapper;
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
    public class TripRepository : ITripRepository
    {
        public DatabaseContext db = new DatabaseContext();
        public void CreateTrip(TripDAO tripDAO)
        {
            Trip trip = Utility.Mapper.Map<TripDAO, Trip>(tripDAO);
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


        public TripDAO GetTrip(Guid TripID)
        {
            //TripDAO tripDAO = db.Trip
            //    .Where(x => x.Id == TripID)
            //    .Select(x => new TripDAO()
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        Started_time = x.Started_time,
            //        End_time = x.Ended_time,
            //        Days = x.Days,
            //        Cover = x.Cover,
            //        TripDays = x.TripDays.Select(y => new TripDaysDAO
            //        {
            //            Id = y.Id,
            //            Trip_id = y.Trip_id,
            //            Date = y.Date.Value,
            //            Startime = y.Startime.Value,
            //            TripDayPlaces = y.TripDayPlace.Select(z => new TripDayPlaceDAO
            //            {
            //                Id = z.Id,
            //                TripDays_id = z.TripDays_id,
            //                Travel_time = z.Travel_time,
            //                Transit_time = z.Transit_time,
            //                Stay_time = z.Stay_time,
            //                Place_name = z.Place_name,
            //                Place_id = z.Place_id,
            //            }).ToList(),
            //        }).ToList()

            //    }).FirstOrDefault();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Trip, TripDAO>();
                cfg.CreateMap<TripDays, TripDaysDAO>()
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.Value))
                    .ForMember(dest => dest.Startime, opt => opt.MapFrom(src => src.Startime.Value))
                    .ForMember(dest => dest.TripDayPlaces, opt => opt.MapFrom(src => src.TripDayPlace));
                cfg.CreateMap<TripDayPlace, TripDayPlaceDAO>();
            });
            var mapper = config.CreateMapper();

            TripDAO tripDAO = db.Trip
                .Where(x => x.Id == TripID)
                .ProjectTo<TripDAO>(config)
                .FirstOrDefault();



            return tripDAO;
        }



        public async Task DeleteTripAsync(Guid tripID)
        {
            Trip trip = await db.Trip.FirstOrDefaultAsync(x => x.Id == tripID) ?? throw new KeyNotFoundException();
            List<TripDays> tripDays = db.TripDays.Where(x => x.Trip_id == tripID).ToList();

            foreach (TripDays tripDay in tripDays)
            {
                List<TripDayPlace> TripDayPlaces = db.TripDayPlace.Where(x => x.TripDays_id == tripDay.Id).ToList();
                db.TripDayPlace.RemoveRange(TripDayPlaces);
            }


            db.TripDays.RemoveRange(tripDays);
            db.Trip.Remove(trip);
            await db.SaveChangesAsync();
        }
    }
}
