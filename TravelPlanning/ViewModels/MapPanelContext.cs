using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Models;
using TravelPlanning.Utilities.Interfaces;
using static GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail.PlaceDetailResModel;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class MapPanelContext : INavigationAware
    {
        public string PlaceID = "";

        private INavigationService navigationService;
        public ICommand PlaceSearchCommand { get; set; }
        public ICommand MyTripCommand { get; set; }


        public MapPanelContext(INavigationService navigationService)
        {
            navigationService.Navigate("PlaceSearchPage");
            this.navigationService = navigationService;

            this.PlaceSearchCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("PlaceSearchPage");

            });

            this.MyTripCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("MyTripPage");
            });

        }



        public void DataAware(object data)
        {
            PlaceID = (string)data;
        }
    }
}
