using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Models.DAO;

namespace TravelPlanning.Contracts
{
    public class TripDetailContract
    {
        public interface ITripDetailView
        {
            void OnTripsResponse(List<TripDaysDAO> tripDays);

            void OnCreateTripDaysResponse(TripDaysDAO tripDays);
        }

        public interface ITripDetailPresenter
        {
            void GetTripRequest(Guid tripID);
            void CreateTripDay(Guid tripID);
            void DeleteTripDay(Guid tripID);
        }
    }
}
