using GoogleMap.SDK.Contract.GoogleMap;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using IOCServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TravelPlanning.Models;
using TravelPlanning.Utilities;
using TravelPlanning.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using TravelPlanning.Attributes;

namespace TravelPlanning.Views.Pages
{
    /// <summary>
    /// MapPanelPage.xaml 的互動邏輯
    /// </summary>

    [LeftSidebarAttribute("地點搜尋", Wpf.Ui.Controls.SymbolRegular.Search16)]
    public partial class MapPanelPage : Page
    {
        IMapControl mapControl;
        ServiceProvider serviceProvider;
        IGoogleAPIContext googleAPIContext;


        public MapPanelPage(ServiceProvider provider, IGoogleAPIContext googleAPIContext)
        {
            InitializeComponent();
            this.serviceProvider = provider;
            this.googleAPIContext = googleAPIContext;

            mapControl = serviceProvider.GetService<IMapControl>();
            mapControl.MarkerClick += MapControl_MarkerClick;
            Control control = (Control)mapControl;

            container.Children.Add(control);

            DataContext = new MapPanelContext(new NavService(frame));

            WeakReferenceMessenger.Default.Register<PlaceDetailResModel>(this, AddMarkerandToolTip);
        }

        private void MapControl_MarkerClick(GoogleMapMarker marker)
        {
            //this.MapPanelContext.RenderData((PlaceModel)marker.Tag);
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
    }
}
