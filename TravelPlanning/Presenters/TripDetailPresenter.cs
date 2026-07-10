using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelPlanning.Contracts.MyTripContract;
using TravelPlanning.Database.Interface;
using static TravelPlanning.Contracts.TripDetailContract;
using TravelPlanning.Database.DAO;

namespace TravelPlanning.Presenters
{
    internal class TripDetailPresenter : ITripDetailPresenter
    {
        public ITripDetailView tripDetailView { get; set; }

        public ITripRepository tripRepository { get; set; }

        public TripDetailPresenter(ITripDetailView tripDetailView, ITripRepository tripRepository)
        {
            this.tripDetailView = tripDetailView;
            this.tripRepository = tripRepository;
        }
        public void GetTripRequest(Guid tripID)
        {
            TripDAO tripDAO = tripRepository.GetTrip(tripID);
            CalTripDays(tripDAO);
            this.tripDetailView.OnTripsResponse(tripDAO.TripDays);
        }

        public void CalTripDays(TripDAO tripDAO)
        {
            for (int i = 0; i < tripDAO.TripDays.Count; i++)
            {
                tripDAO.TripDays[i].Day = i + 1;
            }
        }
    }
}
