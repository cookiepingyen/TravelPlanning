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
using System.Windows.Threading;
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

        private readonly List<Location> currentMarkerLocations = new List<Location>();

        /// <summary>正在進行中的 marker 建立流程(含 PlacePhoto 的網路請求)</summary>
        private readonly List<Task> pendingMarkerTasks = new List<Task>();

        /// <summary>截圖前等圖磚下載 / 地圖重新定位的緩衝時間</summary>
        private const int MapSettleDelayMs = 1500;

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

            DataContextChanged += TripDetailPage_DataContextChanged;

            BtnScrollLeft.Click += (sender, e) => DayTabScroller.ScrollToHorizontalOffset(
                DayTabScroller.HorizontalOffset - ScrollStep);

            BtnScrollRight.Click += (sender, e) => DayTabScroller.ScrollToHorizontalOffset(
                DayTabScroller.HorizontalOffset + ScrollStep);
        }

        private void TripDetailPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyPropertyChanged oldViewModel)
            {
                oldViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            if (e.OldValue is TripDetailContext oldContext)
            {
                oldContext.CaptureMapImage = null;
            }


            if (e.NewValue is INotifyPropertyChanged newViewModel)
            {
                newViewModel.PropertyChanged += ViewModel_PropertyChanged;
            }


            if (e.NewValue is TripDetailContext newContext)
            {
                newContext.CaptureMapImage = CaptureMapImageAsync;
            }

        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TripDetailContext.CurrentDay) || e.PropertyName == nameof(TripDetailContext.MapVersion))
            {
                ClearMap();
            }
        }

        private void ClearMap()
        {
            foreach (Location location in currentMarkerLocations)
            {
                mapControl.RemoveMarker("選擇的地點", location);
            }

            currentMarkerLocations.Clear();
            pendingMarkerTasks.Clear();

            mapControl.RemoveRoute("Route");
        }

        /// <summary>
        /// 截圖前先確定「當日的 marker 都已經加到地圖上、而且畫面真的畫完了」,
        /// 否則 ToImage 抓到的會是上一幀(沒有 marker、中心點停在前一天)。
        /// </summary>
        private async Task<byte[]> CaptureMapImageAsync(int pixelWidth)
        {
            // 1. 等所有 marker 的非同步流程跑完(AddMarkerandToolTip 會先 await PlacePhoto)
            while (pendingMarkerTasks.Count > 0)
            {
                Task[] tasks = pendingMarkerTasks.ToArray();
                pendingMarkerTasks.Clear();
                await Task.WhenAll(tasks);
            }

            Control mapVisual = (Control)mapControl;
            mapVisual.UpdateLayout();

            // 2. 等圖磚下載完、地圖 zoom-to-fit 到當日路線
            await Task.Delay(MapSettleDelayMs);

            // 3. 讓 WPF 完成一次 render pass,RenderTargetBitmap 才抓得到最新畫面
            await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Background);

            return mapControl.ToImage(pixelWidth);
        }



        private void MapControl_MarkerClick(GoogleMapMarker marker)
        {
        }

        private void CreateRoute(object recipient, List<Location> locations)
        {
            this.mapControl.RemoveRoute("Route");
            this.mapControl.AddRoute("Route", locations);
        }

        public void AddMarkerandToolTip(object sender, PlaceDetailResModel e)
        {
            // 記下這個 task,匯出截圖時才知道 marker 有沒有真的加完
            pendingMarkerTasks.Add(AddMarkerandToolTipAsync(e));
        }

        private async Task AddMarkerandToolTipAsync(PlaceDetailResModel e)
        {
            MapToolTip toolTip = new MapToolTip();

            string BusinessStatusText = "未提供";
            if (e.result.current_opening_hours != null)
            {
                BusinessStatusText = e.result.current_opening_hours.open_now ? "營業中" : "已打烊";
            }

            byte[] photobytes = null;
            if (e.result.photos != null && e.result.photos.Count() > 0)
            {
                photobytes = await googleAPIContext.Place.PlacePhoto(new PlacePhotoRequest()
                {
                    photo_reference = e.result.photos[0].photo_reference,
                    photoSpec = new PhotoSpec()
                    {
                        maxwidth = 600,
                        maxheight = 300
                    }
                });
            }


            toolTip.DataContext = new PlaceModel()
            {
                PlaceID = e.result.place_id,
                PlaceName = e.result.name,
                Phone = e.result.formatted_phone_number,
                Address = e.result.formatted_address,
                Rating = e.result.rating,
                UserRatingsTotal = $"({e.result.user_ratings_total})",
                BusinessStatus = BusinessStatusText,
                Photo = photobytes == null ? null : CreateImage(photobytes),
                Reviews = e.result.reviews,
                IsOpening = e.result.current_opening_hours?.open_now
            };

            var location = e.result.geometry.location;
            Location markerLocation = new Location(location.lat, location.lng);
            mapControl.AddMarker("選擇的地點", markerLocation, toolTip);
            currentMarkerLocations.Add(markerLocation);
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
