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

                WeakReferenceMessenger.Default.Send(directionResModel.routes[0].overview_polyline.points.ToList());
            });

        }
    }
}
