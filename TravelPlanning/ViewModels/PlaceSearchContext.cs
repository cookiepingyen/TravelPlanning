using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using TravelPlanning.Models;
using TravelPlanning.Utilities.Interfaces;
using TravelPlanning.Views.Pages;
using static GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail.PlaceDetailResModel;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class PlaceSearchContext : INavigationAware
    {
        public string PlaceID = "";

        public bool IsVisible { get; set; } = false;

        // 地圖物件資訊
        public String PlaceName { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public double Rating { get; set; }
        public string UserRatingsTotal { get; set; }
        public string BusinessStatus { get; set; }
        public Review[] Reviews { get; set; }
        public BitmapImage Photo { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CommentClickCommand { get; set; }
        public ICommand OverviewClickCommand { get; set; }

        private INavigationService navigationService;
        public PlaceSearchContext(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.SearchCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("OverviewPage", new PlaceOverview(Address, Phone, BusinessStatus));
            });

            this.OverviewClickCommand = new RelayCommand(() => navigationService.Navigate("OverviewPage", new PlaceOverview(Address, Phone, BusinessStatus)));
            this.CommentClickCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("CommentPage", Reviews);
            });
        }

        public void RenderData(string placeID, bool isvisible, string placeName, string phone, string address, double rating, string userRatingsTotal, string businessStatusText, Review[] reviews, BitmapImage photo)
        {
            PlaceID = placeID;
            IsVisible = isvisible;
            PlaceName = placeName;
            Phone = phone;
            Address = address;
            Rating = rating;
            UserRatingsTotal = userRatingsTotal;
            BusinessStatus = businessStatusText;
            Reviews = reviews;
            Photo = photo;

            navigationService.Navigate("OverviewPage", new PlaceOverview(Address, Phone, BusinessStatus));
        }


        public void DataAware(object data)
        {
            PlaceID = (string)data;
        }

    }
}
