using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMap;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using IOCServiceCollection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelPlanning.Utilities;
using TravelPlanning.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using TravelPlanning.Models;
using System.IO;

namespace TravelPlanning.Views.Pages.Trip
{
    /// <summary>
    /// TripDetailPage.xaml 的互動邏輯
    /// </summary>
    public partial class TripDetailPage : Page
    {
        IMapControl mapControl;
        ServiceProvider serviceProvider;
        IGoogleAPIContext googleAPIContext;

        private const double ScrollStep = 80;
        public TripDetailPage(ServiceProvider provider, IGoogleAPIContext googleAPIContext)
        {
            InitializeComponent();
            this.serviceProvider = provider;
            this.googleAPIContext = googleAPIContext;

            mapControl = serviceProvider.GetService<IMapControl>();
            mapControl.MarkerClick += MapControl_MarkerClick;
            Control control = (Control)mapControl;
            container.Children.Add(control);

            //DataContext = new TripDetailContext();

            WeakReferenceMessenger.Default.Register<PlaceDetailResModel>(this, AddMarkerandToolTip);

            WeakReferenceMessenger.Default.Register<List<Location>>(this, CreateRoute);


            BtnScrollLeft.Click += (sender, e) => DayTabScroller.ScrollToHorizontalOffset(
                DayTabScroller.HorizontalOffset - ScrollStep);

            BtnScrollRight.Click += (sender, e) => DayTabScroller.ScrollToHorizontalOffset(
                DayTabScroller.HorizontalOffset + ScrollStep);
        }



        private void MapControl_MarkerClick(GoogleMapMarker marker)
        {
        }

        private void CreateRoute(object recipient, List<Location> locations)
        {
            this.mapControl.RemoveRoute("Route");
            this.mapControl.AddRoute("Route", locations);
        }

        public async void AddMarkerandToolTip(object sender, PlaceDetailResModel e)
        {

            Action<object, PlaceDetailResModel> func = AddMarkerandToolTip;

            MapToolTip toolTip = new MapToolTip();

            string BusinessStatusText = "未提供";
            if (e.result.current_opening_hours != null)
            {
                BusinessStatusText = e.result.current_opening_hours.open_now ? "營業中" : "已打烊";
            }

            byte[] photobytes = await googleAPIContext.Place.PlacePhoto(new PlacePhotoRequest()
            {
                photo_reference = e.result.photos[0].photo_reference,
                photoSpec = new PhotoSpec()
                {
                    maxwidth = 600,
                    maxheight = 300
                }
            });


            toolTip.DataContext = new PlaceModel()
            {
                PlaceID = e.result.place_id,
                PlaceName = e.result.name,
                Phone = e.result.formatted_phone_number,
                Address = e.result.formatted_address,
                Rating = e.result.rating,
                UserRatingsTotal = $"({e.result.user_ratings_total})",
                BusinessStatus = BusinessStatusText,
                Photo = CreateImage(photobytes),
                Reviews = e.result.reviews,
                IsOpening = e.result.current_opening_hours?.open_now
            };

            var location = e.result.geometry.location;
            mapControl.AddMarker("選擇的地點", new Location(location.lat, location.lng), toolTip);
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
    }
}
