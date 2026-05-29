using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Database.Entities;
using TravelPlanning.Models;
using TravelPlanning.Utilities.Interfaces;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class FavoriteItemPlaceContext : INavigationAware
    {
        public ICommand AutoCompleteCommaned { get; set; }
        public ICommand AddPlaceCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        IGoogleAPIContext googleAPIContext;
        private INavigationService navigationService;

        public PlaceDetailResModel Place { get; set; }

        public int PlaceCount { get; set; }

        public FavoriteListItemContext FavoriteListItemContext { get; set; }

        public FavoriteItemPlaceContext(IGoogleAPIContext googleAPIContext)
        {
            this.googleAPIContext = googleAPIContext;

            this.AutoCompleteCommaned = new RelayCommand<PlaceDetailResModel>(e =>
            {
                WeakReferenceMessenger.Default.Send(e);
            });

            this.AddPlaceCommand = new RelayCommand<PlaceDetailResModel>(e =>
            {

            });

            this.GoBackCommand = new RelayCommand<PlaceDetailResModel>(item =>
            {
                navigationService.Navigate("FavoritePage", item);
            });
        }



        public void DataAware(object data)
        {
            FavoriteItemObject favoriteItemObject = data as FavoriteItemObject;

            navigationService = favoriteItemObject.navigationService;
            FavoriteListItemContext = favoriteItemObject.favoriteListItemContext;
        }
    }
}
