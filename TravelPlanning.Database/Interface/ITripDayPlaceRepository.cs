using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Models.DAO;

namespace TravelPlanning.Database.Interface
{
    public interface ITripDayPlaceRepository
    {
        Task CreateTripDayPlaceAsync(TripDayPlaceDAO tripDayPlace);
        Task DeleteTripDayPlaceAsync(Guid id);
        Task<List<TripDayPlaceDAO>> GetTripDayPlacesAsync(Guid tripDayId);

        Task<TripDaysDAO> CreateTripDayAsync(Guid TripID);

        Task DeleteTripDayAsync(Guid id);

        Task<List<TripDaysDAO>> GetTripDaysAsync(Guid tripId);

    }
}
