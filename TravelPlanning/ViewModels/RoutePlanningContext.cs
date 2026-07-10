using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Direction;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TravelPlanning.Utilities.Interfaces;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Enums;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Routes;

namespace TravelPlanning.ViewModels
{
    internal class RoutePlanningContext : INotifyPropertyChanged, INavigationAware
    {
        public string PlaceID = "";

        IGoogleAPIContext googleAPIContext;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AutoCompleteCommaned { get; set; }

        public string TravelMode { get; set; } = "DRIVE";
        public ICommand RoutePlanningCommaned { get; set; }
        public ICommand SelectTravelModeCommand { get; set; }
        private PlaceDetailResModel _start { get; set; }
        public PlaceDetailResModel Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                OnPropertyChanged(nameof(Start));
            }
        }

        private PlaceDetailResModel _end { get; set; }

        public PlaceDetailResModel End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                OnPropertyChanged(nameof(End));
            }
        }

        private List<RouteStep> _routeSteps { get; set; }
        public List<RouteStep> RouteSteps
        {
            get
            {
                return _routeSteps;
            }
            set
            {
                _routeSteps = value;
                OnPropertyChanged(nameof(RouteSteps));
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DataAware(object data)
        {
            End = (PlaceDetailResModel)data;
        }

        public RoutePlanningContext(IGoogleAPIContext googleAPIContext)
        {
            this.googleAPIContext = googleAPIContext;

            this.AutoCompleteCommaned = new RelayCommand<PlaceDetailResModel>(e =>
            {
                WeakReferenceMessenger.Default.Send(e);
            });

            this.SelectTravelModeCommand = new RelayCommand<string>(e =>
            {
                TravelMode = e;
                CallRoutePlanning();
            });


            this.RoutePlanningCommaned = new RelayCommand(async () =>
            {
                CallRoutePlanning();
            });

        }



        public async void CallRoutePlanning()
        {
            if (_start == null || _end == null)
                return;

            TravelMode travelMode = (TravelMode)Enum.Parse(typeof(TravelMode), TravelMode);

            RoutesRequest routesRequest = new RoutesRequest(Start.result.place_id, End.result.place_id, travelMode, AddressType.PlaceId);


            RoutesResModel routesResModel = await googleAPIContext.Route.GetRoutes(routesRequest);

            RouteSteps = routesResModel.routes[0].legs[0].steps.Select(step =>
            {
                if (step.navigationInstruction == null)
                    return null;
                return new RouteStep(step.navigationInstruction.instructions, step.localizedValues.distance.text, step.navigationInstruction.maneuver);
            }).Where(x => x != null).ToList();


            WeakReferenceMessenger.Default.Send(routesResModel.routes[0].polyline.encodedPolyline.ToList());
        }
    }
}
