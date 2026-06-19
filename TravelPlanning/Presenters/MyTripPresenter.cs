using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.Interface;
using static TravelPlanning.Contracts.MyTripContract;

namespace TravelPlanning.Presenters
{
    public class MyTripPresenter : IMyTripPresenter
    {
        public IMyTripView myTripView { get; set; }

        public ITripRepository tripRepository { get; set; }

        public MyTripPresenter(IMyTripView myTripView, ITripRepository tripRepository)
        {
            this.myTripView = myTripView;
            this.tripRepository = tripRepository;
        }


        public void GetTrips()
        {
            var trips = tripRepository.GetTrips();
            this.myTripView.OnTripsResponse(trips);
        }
    }
}
