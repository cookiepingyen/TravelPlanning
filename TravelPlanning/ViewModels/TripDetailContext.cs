using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Enums;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Routes;
using IOCServiceCollection;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Models.DAO;
using TravelPlanning.Models.DTO;
using static TravelPlanning.Contracts.MyTripContract;
using static TravelPlanning.Contracts.TripDetailContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class TripDetailContext : ITripDetailView
    {
        public Guid TripID { get; set; }

        public ObservableCollection<TripDaysContext> tripDaysContexts { get; set; }

        public ICommand AddDayBtnCommand { get; set; }
        public ICommand DeleteDayBtnCommand { get; set; }
        public ICommand SelectDayCommand { get; set; }

        public ITripDetailPresenter tripDetailPresenter { get; set; }

        public TripDaysContext CurrentDay { get; set; }

        public IGoogleAPIContext GoogleAPIContext { get; set; }

        public TripDetailContext(Guid tripID, PresenterFactory presenterFactory, IGoogleAPIContext googleAPIContext)
        {
            TripID = tripID;

            GoogleAPIContext = googleAPIContext;

            this.tripDetailPresenter = presenterFactory.Create<ITripDetailPresenter, ITripDetailView>(this);

            tripDetailPresenter.GetTripRequest(tripID);



            this.AddDayBtnCommand = new RelayCommand<Guid>(tripDetailPresenter.CreateTripDay);

            this.DeleteDayBtnCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                tripDaysContexts.Remove(tripDay);
                var firstDay = tripDaysContexts.OrderBy(x => x.Day).First();
                firstDay.IsChecked = true;

                for (int i = 1; i < tripDaysContexts.Count; i++)
                {
                    TripDaysContext x = tripDaysContexts[i];
                    x.Day = i + 1;
                    x.Date = firstDay.Date.AddDays(i);
                    x.IsChecked = false;
                }
            });


            this.SelectDayCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                CurrentDay = tripDay;
            });



        }

        public async Task<PlaceDetailResModel> GetPlaceDetail(string selectedItem, bool with_all_field)
        {
            PlaceDetailRequest placeDetailRequest = new PlaceDetailRequest();
            placeDetailRequest.placeId = selectedItem;

            if (!with_all_field)
            {
                placeDetailRequest.fields = new PlaceDetailInputFields[] { PlaceDetailInputFields.name, PlaceDetailInputFields.formatted_address, PlaceDetailInputFields.type };
            }

            PlaceDetailResModel placeDetailResModel = await GoogleAPIContext.Place.PlaceDetail(placeDetailRequest);

            return placeDetailResModel;
        }

        public async void OnTripsResponse(List<TripDaysDAO> tripDays)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TripDaysDAO, TripDaysContext>();
                cfg.CreateMap<TripDayPlaceDAO, TripDayPlaceContext>();
            });
            var mapper = config.CreateMapper();

            List<TripDaysContext> days = mapper.Map<List<TripDaysContext>>(tripDays);

            tripDaysContexts = new ObservableCollection<TripDaysContext>(days);
            CurrentDay = tripDaysContexts[1];

            if (CurrentDay.TripDayPlaces.Count >= 2)
            {
                string StartPlaceId = CurrentDay.TripDayPlaces.First().Place_id;
                string EndPlaceId = CurrentDay.TripDayPlaces.Last().Place_id;

                RoutesRequest routesRequest = new RoutesRequest(StartPlaceId, EndPlaceId, addressType: AddressType.PlaceId);
                if (CurrentDay.TripDayPlaces.Count > 2)
                {
                    List<string> intermediates = new List<string>();
                    for (int i = 1; i < CurrentDay.TripDayPlaces.Count - 1; i++)
                    {
                        intermediates.Add(CurrentDay.TripDayPlaces[i].Place_id);
                    }
                    routesRequest.intermediates = intermediates;
                }
                RoutesResModel routesResModel = await GoogleAPIContext.Route.GetRoutes(routesRequest);

                WeakReferenceMessenger.Default.Send(routesResModel.routes[0].polyline.encodedPolyline.ToList());
            }

            var places = await Task.WhenAll(CurrentDay.TripDayPlaces.Select(x => GetPlaceDetail(x.Place_id, true)));
            foreach (var place in places)
            {
                WeakReferenceMessenger.Default.Send(place);
            }
        }

        public void OnTripDaysResponse(TripDaysDAO tripDays)
        {
            TripDaysContext tripDay = Utilities.Mapper.Map<TripDaysDAO, TripDaysContext>(tripDays);
            tripDaysContexts.Add(tripDay);
        }
    }
}
