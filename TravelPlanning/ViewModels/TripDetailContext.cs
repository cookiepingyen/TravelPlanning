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
using TravelPlanning.Presenters;
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
        public ICommand OpenAddPlaceCommand { get; set; }
        public ICommand CancelAddPlaceCommand { get; set; }
        public ICommand ConfirmAddPlaceCommand { get; set; }
        public ICommand SelectAddPlaceCommand { get; set; }
        public ICommand TogglePlaceMenuCommand { get; set; }
        public ICommand DeletePlaceCommand { get; set; }

        public bool IsAddPlacePopupOpen { get; set; }

        public PlaceDetailResModel PendingPlace { get; set; }

        public string PendingTimeText { get; set; }

        public string PendingTransitTimeText { get; set; }

        public bool IsCustom { get; set; }

        public string PendingCustomHourText { get; set; }

        public string PendingCustomMinuteText { get; set; }

        public ITripDetailPresenter tripDetailPresenter { get; set; }

        public TripDaysContext CurrentDay { get; set; }

        public IGoogleAPIContext GoogleAPIContext { get; set; }

        public int MapVersion { get; set; }


        public TripDetailContext(Guid tripID, PresenterFactory presenterFactory, IGoogleAPIContext googleAPIContext)
        {
            TripID = tripID;

            GoogleAPIContext = googleAPIContext;

            this.tripDetailPresenter = presenterFactory.Create<ITripDetailPresenter, ITripDetailView>(this);

            tripDetailPresenter.GetTripRequest(tripID);



            this.AddDayBtnCommand = new RelayCommand<Guid>(guid =>
            {
                tripDetailPresenter.CreateTripDay(guid);
            });

            this.DeleteDayBtnCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                tripDetailPresenter.DeleteTripDay(tripDay.Id);
                tripDaysContexts.Remove(tripDay);
                var firstDay = tripDaysContexts.OrderBy(x => x.Day).First();
                firstDay.IsChecked = true;

                for (int i = 0; i < tripDaysContexts.Count; i++)
                {
                    TripDaysContext x = tripDaysContexts[i];
                    x.Day = i + 1;
                    x.Date = firstDay.Date.AddDays(i);
                    x.IsChecked = false;
                }

            });

            this.DeletePlaceCommand = new RelayCommand<TripDayPlaceContext>(async (tripDayPlace) =>
            {
                tripDetailPresenter.DeleteTripDayPlace(tripDayPlace.Id);
                CurrentDay.TripDayPlaces.Remove(tripDayPlace);
                await RefreshMapAsync();
            });



            this.SelectDayCommand = new RelayCommand<TripDaysContext>(async tripDay =>
            {
                CurrentDay = tripDay;
                await LoadCurrentDayMapAsync();
            });

            this.SelectAddPlaceCommand = new RelayCommand<PlaceDetailResModel>(place =>
            {
                PendingPlace = place;
            });

            this.TogglePlaceMenuCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                if (CurrentDay?.TripDayPlaces == null) return;

                bool isOpening = !place.IsMenuOpen;

                foreach (TripDayPlaceContext item in CurrentDay.TripDayPlaces)
                {
                    item.IsMenuOpen = false;
                }

                place.IsMenuOpen = isOpening;
            });

            this.OpenAddPlaceCommand = new RelayCommand(() =>
            {
                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = true;
            });

            this.CancelAddPlaceCommand = new RelayCommand(() =>
            {
                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = false;
            });

            this.ConfirmAddPlaceCommand = new RelayCommand(() =>
            {
                if (PendingPlace == null) return;

                int stayTime = int.TryParse(PendingTimeText, out int parsedStayTime) ? parsedStayTime : 30;
                int transitTime = int.TryParse(PendingTransitTimeText, out int parsedTransitTime) ? parsedTransitTime : 0;

                bool hasCustomTime = IsCustom
                    && int.TryParse(PendingCustomHourText, out int customHour) && customHour >= 0 && customHour <= 23
                    && int.TryParse(PendingCustomMinuteText, out int customMinute) && customMinute >= 0 && customMinute <= 59;

                DateTime startTime;
                if (hasCustomTime)
                {
                    startTime = new DateTime(CurrentDay.Date.Year, CurrentDay.Date.Month, CurrentDay.Date.Day, int.Parse(PendingCustomHourText), int.Parse(PendingCustomMinuteText), 0);
                }
                else if (CurrentDay.TripDayPlaces == null || CurrentDay.TripDayPlaces.Count == 0)
                {
                    startTime = CurrentDay.StartTime;
                }
                else
                {
                    TripDayPlaceContext lastPlace = CurrentDay.TripDayPlaces.OrderBy(x => x.Start_time).Last();
                    startTime = lastPlace.Start_time.AddMinutes(lastPlace.Transit_time + lastPlace.Stay_time);
                }

                TripDayPlaceDAO tripDayPlace = new TripDayPlaceDAO()
                {
                    TripDays_id = CurrentDay.Id,
                    Place_id = PendingPlace.result.place_id,
                    Place_name = PendingPlace.result.name,
                    Start_time = startTime,
                    Transit_time = transitTime,
                    Stay_time = stayTime,
                    Is_custom = IsCustom,
                };

                tripDetailPresenter.AddTripDayPlace(tripDayPlace);

                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = false;
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

            foreach (TripDaysContext day in days)
            {
                if (day.TripDayPlaces != null)
                {
                    day.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>(day.TripDayPlaces.OrderBy(x => x.Start_time));
                }
            }

            tripDaysContexts = new ObservableCollection<TripDaysContext>(days);
            CurrentDay = tripDaysContexts[1];

            await LoadCurrentDayMapAsync();
        }

        public async Task LoadCurrentDayMapAsync()
        {
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

        public async Task RefreshMapAsync()
        {
            MapVersion++;
            await LoadCurrentDayMapAsync();
        }

        public void OnCreateTripDaysResponse(TripDaysDAO tripDays)
        {
            TripDaysContext tripDay = Utilities.Mapper.Map<TripDaysDAO, TripDaysContext>(tripDays);
            tripDaysContexts.Add(tripDay);
        }

        public async void OnCreateTripDayPlaceResponse(TripDayPlaceDAO tripDayPlace)
        {
            TripDayPlaceContext tripDayPlaceContext = Utilities.Mapper.Map<TripDayPlaceDAO, TripDayPlaceContext>(tripDayPlace);

            if (CurrentDay.TripDayPlaces == null)
            {
                CurrentDay.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>();
            }

            CurrentDay.TripDayPlaces.Add(tripDayPlaceContext);
            await RefreshMapAsync();
        }



    }
}
