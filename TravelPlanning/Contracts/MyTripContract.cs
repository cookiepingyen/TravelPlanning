using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;

namespace TravelPlanning.Contracts
{
    public class MyTripContract
    {
        public interface IMyTripView
        {
            void OnTripsResponse(List<TripDAO> tripDAOs);
        }

        public interface IMyTripPresenter
        {
            void GetTrips();

            Task DeleteTrip(Guid tripID);
        }
    }
}
