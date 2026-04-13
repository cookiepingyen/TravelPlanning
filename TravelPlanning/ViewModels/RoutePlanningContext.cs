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

namespace TravelPlanning.ViewModels
{
    internal class RoutePlanningContext : INotifyPropertyChanged
    {
        public string PlaceID = "";

        IGoogleAPIContext googleAPIContext;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AutoCompleteCommaned { get; set; }
        public ICommand RoutePlanningCommaned { get; set; }

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


        public RoutePlanningContext(IGoogleAPIContext googleAPIContext)
        {
            this.googleAPIContext = googleAPIContext;

            this.AutoCompleteCommaned = new RelayCommand<PlaceDetailResModel>(e =>
            {
                WeakReferenceMessenger.Default.Send(e);
            });

            this.RoutePlanningCommaned = new RelayCommand(async () =>
            {
                DirectionRequest directionRequest = new DirectionRequest(End.result.place_id, Start.result.place_id, true);
                DirectionResModel directionResModel = await googleAPIContext.Direction.GetDirections(directionRequest);

                RouteSteps = directionResModel.routes[0].legs[0].steps.Select(step => new RouteStep(step.html_instructions, step.distance.text, step.maneuver)).ToList();


                WeakReferenceMessenger.Default.Send(directionResModel.routes[0].overview_polyline.points.ToList());
            });

        }
    }
}
