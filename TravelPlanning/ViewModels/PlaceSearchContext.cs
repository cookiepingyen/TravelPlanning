using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using CommunityToolkit.Mvvm.Messaging;

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
        public ICommand AutoCompleteCommaned { get; set; }

        IGoogleAPIContext googleAPIContext;


        private INavigationService navigationService;
        public PlaceSearchContext(INavigationService navigationService, IGoogleAPIContext googleAPIContext)
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

            this.AutoCompleteCommaned = new RelayCommand<PlaceDetailResModel>(async e =>
            {
                byte[] photobytes = await googleAPIContext.Place.PlacePhoto(new PlacePhotoRequest()
                {
                    photo_reference = e.result.photos[0].photo_reference,
                    photoSpec = new PhotoSpec()
                    {
                        maxwidth = 600,
                        maxheight = 300
                    }
                });

                RenderData(e.result.place_id, true, e.result.name, e.result.formatted_phone_number, e.result.formatted_address,
                                           e.result.rating, $"({e.result.user_ratings_total})", GetBusinessText(e.result.current_opening_hours), e.result.reviews, CreateImage(photobytes));

                WeakReferenceMessenger.Default.Send(e);
            });
            this.googleAPIContext = googleAPIContext;
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

        private BitmapImage CreateImage(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            memoryStream.Position = 0;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        public string GetBusinessText(Current_Opening_Hours current_opening_hours)
        {
            string businessStatusText = "未提供";
            if (current_opening_hours != null)
            {
                businessStatusText = current_opening_hours.open_now ? "營業中" : "已打烊";
            }
            return businessStatusText;
        }

    }
}
