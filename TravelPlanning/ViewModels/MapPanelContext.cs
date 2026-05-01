using CommunityToolkit.Mvvm.Messaging;
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
        public ICommand RoutePlanningPageCommand { get; set; }

        public bool IsPlaceSearchSelected { get; set; } = true;
        public bool IsRoutePlanningSelected { get; set; } = false;

        public MapPanelContext(INavigationService navigationService)
        {
            navigationService.Navigate("PlaceSearchPage", navigationService);
            this.navigationService = navigationService;

            this.PlaceSearchCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("PlaceSearchPage", navigationService);
                IsPlaceSearchSelected = true;
                IsRoutePlanningSelected = false;
            });

            this.RoutePlanningPageCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("RoutePlanningPage");
                IsRoutePlanningSelected = true;
                IsPlaceSearchSelected = false;
            });

            WeakReferenceMessenger.Default.Register<NavigateToRoutePlanningMessage>(this, (recipient, message) =>
            {
                IsRoutePlanningSelected = true;
                IsPlaceSearchSelected = false;
            });
        }



        public void DataAware(object data)
        {
            PlaceID = (string)data;
        }
    }
}
