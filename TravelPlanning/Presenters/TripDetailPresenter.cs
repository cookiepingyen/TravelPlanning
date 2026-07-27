using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelPlanning.Contracts.MyTripContract;
using TravelPlanning.Database.Interface;
using static TravelPlanning.Contracts.TripDetailContract;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Models.DAO;

namespace TravelPlanning.Presenters
{
    internal class TripDetailPresenter : ITripDetailPresenter
    {
        public ITripDetailView tripDetailView { get; set; }

        public ITripRepository tripRepository { get; set; }

        public ITripDayPlaceRepository tripDayPlaceRepository { get; set; }

        public TripDetailPresenter(ITripDetailView tripDetailView, ITripRepository tripRepository, ITripDayPlaceRepository tripDayPlaceRepository)
        {
            this.tripDetailView = tripDetailView;
            this.tripRepository = tripRepository;
            this.tripDayPlaceRepository = tripDayPlaceRepository;
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


        public async void CreateTripDay(Guid tripID)
        {
            TripDaysDAO tripDaysDAO = await tripDayPlaceRepository.CreateTripDayAsync(tripID);
            this.tripDetailView.OnCreateTripDaysResponse(tripDaysDAO);
        }


        public async void DeleteTripDay(Guid tripID)
        {
            await tripDayPlaceRepository.DeleteTripDayAsync(tripID);
        }

        public async void AddTripDayPlace(TripDayPlaceDAO tripDayPlace)
        {
            await tripDayPlaceRepository.CreateTripDayPlaceAsync(tripDayPlace);
            this.tripDetailView.OnCreateTripDayPlaceResponse(tripDayPlace);
        }

        public async void UpdateTripDayPlace(TripDayPlaceDAO tripDayPlace)
        {
            await tripDayPlaceRepository.UpdateTripDayPlaceAsync(tripDayPlace);

            List<TripDayPlaceDAO> tripDayPlaceDAOs = await CalTripDayPlaceTimes(tripDayPlace.TripDays_id);

            await tripDayPlaceRepository.UpdateTripDayPlacesAsync(tripDayPlaceDAOs);

            this.tripDetailView.OnUpdateTripDayPlaceResponse(tripDayPlaceDAOs);
        }

        public async void DeleteTripDayPlace(Guid tripID)
        {
            await tripDayPlaceRepository.DeleteTripDayPlaceAsync(tripID);
        }


        private async Task<List<TripDayPlaceDAO>> CalTripDayPlaceTimes(Guid TripDays_id)
        {
            List<TripDayPlaceDAO> tripDayPlaceDAOs = await tripDayPlaceRepository.GetTripDayPlacesAsync(TripDays_id);
            tripDayPlaceDAOs = tripDayPlaceDAOs.OrderBy(x => x.Start_time).ToList();

            List<TripDayPlaceDAO> newTripDayPlaceDAOS = tripDayPlaceDAOs.Aggregate(new List<TripDayPlaceDAO>(), (datas, trip) =>
            {

                if (datas.Count == 0 || trip.Is_custom)
                {
                    datas.Add(trip);
                    return datas;
                }

                TripDayPlaceDAO last_place = datas.Last();
                trip.Start_time = last_place.Start_time.AddMinutes(last_place.Stay_time + last_place.Transit_time);
                datas.Add(trip);

                return datas;
            });

            return newTripDayPlaceDAOS;

        }
    }
}
