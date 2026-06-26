using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Models.DAO;

namespace TravelPlanning.Database.Interface
{
    public interface ITripDayRepository
    {
        Task<TripDaysDAO> CreateTripDayAsync(Guid TripID);
        Task<List<TripDaysDAO>> GetTripDaysAsync(Guid tripId);

        Task DeleteTripDayAsync(Guid id);
    }
}
