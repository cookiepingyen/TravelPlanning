using GMap.NET;
using GoogleMap.SDK.Contract.GoogleMap;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using GoogleMap.SDK.UI.WPF.Components.AutoComplete;
using IOCServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using TravelPlanning.Models;
using TravelPlanning.Utilities;
using TravelPlanning.Utilities.Interfaces;
using TravelPlanning.ViewModels;
using static GoogleMap.SDK.Contract.Components.AutoComplete.AutoCompleteContract;

namespace TravelPlanning.Views.Pages
{
    /// <summary>
    /// PlaceSearchPage.xaml 的互動邏輯
    /// </summary>

    public partial class PlaceSearchPage : Page
    {
        PlaceAutoCompleteView AutoCompleteView;
        IMapControl mapControl;
        ServiceProvider serviceProvider;
        IGoogleAPIContext googleAPIContext;
        GoogleMapMarker selectedMarker;
        PlaceSearchContext placeSearchContext;

        public PlaceSearchPage(ServiceProvider provider, IGoogleAPIContext googleAPIContext)
        {
            InitializeComponent();
            this.serviceProvider = provider;
            this.googleAPIContext = googleAPIContext;

            AutoCompleteView = (PlaceAutoCompleteView)provider.GetService<IAutoCompleteView>();
            AutoCompleteView.selectChange += PlaceAutoCompleteView_selectChange;

            AutoCompleteView.Padding = new Thickness(5);
            AutoCompleteView.VerticalContentAlignment = VerticalAlignment.Center;
            AutoCompleteView.Background = new SolidColorBrush(Colors.Transparent);
            AutoCompleteView.Foreground = new SolidColorBrush(Colors.White);
            AutoCompleteView.BorderThickness = new Thickness(0);

            autoCompletePanel.Children.Add(AutoCompleteView);

            mapControl = serviceProvider.GetService<IMapControl>();
            mapControl.MarkerClick += MapControl_MarkerClick;
            Control control = (Control)mapControl;

            container.Children.Add(control);

            this.placeSearchContext = new PlaceSearchContext(new NavService(frame));
            DataContext = this.placeSearchContext;
        }

        private void MapControl_MarkerClick(GoogleMapMarker marker)
        {
            //DataContext = marker.Tag;
        }

        private async void PlaceAutoCompleteView_selectChange(object sender, PlaceDetailResModel e)
        {
            Console.WriteLine($"name: {e.result.name}, place_id: {e.result.place_id}");

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
            toolTip.DataContext = new PlaceCard()
            {
                PlaceName = e.result.name,
                Phone = e.result.formatted_phone_number,
                Address = e.result.formatted_address,
                Rating = e.result.rating,
                UserRatingsTotal = $"({e.result.user_ratings_total.ToString()})",
                BusinessStatus = BusinessStatusText,
                Photo = CreateImage(photobytes)
            };

            placeSearchContext.IsVisible = true;
            placeSearchContext.PlaceName = e.result.name;
            placeSearchContext.Phone = e.result.formatted_phone_number;
            placeSearchContext.Address = e.result.formatted_address;
            placeSearchContext.Rating = e.result.rating;
            placeSearchContext.UserRatingsTotal = $"({e.result.user_ratings_total.ToString()})";
            placeSearchContext.BusinessStatus = BusinessStatusText;
            placeSearchContext.Photo = CreateImage(photobytes);

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
