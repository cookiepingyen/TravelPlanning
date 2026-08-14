using Aspose.Words;
using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Enums;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Routes;
using IOCServiceCollection;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Models.DAO;
using TravelPlanning.Extension;
using TravelPlanning.Models.DTO;
using TravelPlanning.Presenters;
using TravelPlanning.Utilities;
using TravelPlanning.Views.Pages;
using static TravelPlanning.Contracts.MyTripContract;
using static TravelPlanning.Contracts.TripDetailContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class TripDetailContext : ITripDetailView
    {
        [DoNotNotify]
        public Func<int, Task<byte[]>> CaptureMapImage { get; set; }

        public TripDTO CurrentTrip { get; set; }
        public Guid TripID { get; set; }
        public TripDaysContext CurrentDay { get; set; }

        public ObservableCollection<TripDaysContext> tripDaysContexts { get; set; }

        #region commands
        public ICommand AddDayBtnCommand { get; set; }
        public ICommand DeleteDayBtnCommand { get; set; }
        public ICommand SelectDayCommand { get; set; }
        public ICommand OpenAddPlaceCommand { get; set; }
        public ICommand CancelAddPlaceCommand { get; set; }
        public ICommand ConfirmAddPlaceCommand { get; set; }
        public ICommand SelectAddPlaceCommand { get; set; }
        public ICommand TogglePlaceMenuCommand { get; set; }
        public ICommand DeletePlaceCommand { get; set; }
        public ICommand EditPlaceCommand { get; set; }
        public ICommand CancelEditPlaceCommand { get; set; }
        public ICommand ConfirmEditPlaceCommand { get; set; }
        public ICommand ChangeTravelModeCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public bool IsAddPlacePopupOpen { get; set; }
        #endregion

        public PlaceDetailResModel PendingPlace { get; set; }

        public string PendingTimeText { get; set; }

        public string PendingTransitTimeText { get; set; }

        public bool IsCustom { get; set; }

        public string PendingCustomHourText { get; set; }

        public string PendingCustomMinuteText { get; set; }

        public bool IsEditPlacePopupOpen { get; set; }

        public TripDayPlaceContext EditingPlace { get; set; }

        public string EditPendingStayTimeText { get; set; }

        public string EditPendingTransitTimeText { get; set; }

        public bool EditIsCustom { get; set; }

        public string EditPendingCustomHourText { get; set; }

        public string EditPendingCustomMinuteText { get; set; }

        public ITripDetailPresenter tripDetailPresenter { get; set; }


        public IGoogleAPIContext GoogleAPIContext { get; set; }

        public int MapVersion { get; set; }

        public TravelMode TravelMode { get; set; } = TravelMode.DRIVE;


        public TripDetailContext(TripDTO tripDTO, PresenterFactory presenterFactory, IGoogleAPIContext googleAPIContext)
        {
            CurrentTrip = tripDTO;
            TripID = tripDTO.Id;

            GoogleAPIContext = googleAPIContext;

            this.tripDetailPresenter = presenterFactory.Create<ITripDetailPresenter, ITripDetailView>(this);

            tripDetailPresenter.GetTripRequest(TripID);



            this.AddDayBtnCommand = new RelayCommand<Guid>(guid =>
            {
                tripDetailPresenter.CreateTripDay(guid);
            });

            this.DeleteDayBtnCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                tripDetailPresenter.DeleteTripDay(tripDay.Id);
                tripDaysContexts.Remove(tripDay);
                var firstDay = tripDaysContexts.OrderBy(x => x.Day).First();
                firstDay.IsChecked = true;

                for (int i = 0; i < tripDaysContexts.Count; i++)
                {
                    TripDaysContext x = tripDaysContexts[i];
                    x.Day = i + 1;
                    x.Date = firstDay.Date.AddDays(i);
                    x.IsChecked = false;
                }

            });

            this.DeletePlaceCommand = new RelayCommand<TripDayPlaceContext>(async (tripDayPlace) =>
            {
                tripDetailPresenter.DeleteTripDayPlace(tripDayPlace.Id);
                CurrentDay.TripDayPlaces.Remove(tripDayPlace);
                await RefreshMapAsync();
            });



            this.SelectDayCommand = new RelayCommand<TripDaysContext>(async tripDay =>
            {
                CurrentDay = tripDay;
                await LoadCurrentDayMapAsync();
            });

            this.SelectAddPlaceCommand = new RelayCommand<PlaceDetailResModel>(place =>
            {
                PendingPlace = place;
            });

            this.TogglePlaceMenuCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                if (CurrentDay?.TripDayPlaces == null) return;

                bool isOpening = !place.IsMenuOpen;

                foreach (TripDayPlaceContext item in CurrentDay.TripDayPlaces)
                {
                    item.IsMenuOpen = false;
                }

                place.IsMenuOpen = isOpening;
            });

            this.EditPlaceCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                place.IsMenuOpen = false;

                EditingPlace = place;
                EditPendingStayTimeText = place.Stay_time.ToString();
                EditPendingTransitTimeText = place.Transit_time.ToString();
                EditIsCustom = place.Is_custom;
                EditPendingCustomHourText = place.Is_custom ? place.Start_time.Hour.ToString("00") : string.Empty;
                EditPendingCustomMinuteText = place.Is_custom ? place.Start_time.Minute.ToString("00") : string.Empty;
                IsEditPlacePopupOpen = true;
            });

            this.CancelEditPlaceCommand = new RelayCommand(() =>
            {
                EditingPlace = null;
                EditPendingStayTimeText = string.Empty;
                EditPendingTransitTimeText = string.Empty;
                EditIsCustom = false;
                EditPendingCustomHourText = string.Empty;
                EditPendingCustomMinuteText = string.Empty;
                IsEditPlacePopupOpen = false;
            });

            this.ConfirmEditPlaceCommand = new RelayCommand<TripDayPlaceContext>(place =>
            {
                place.Is_custom = EditIsCustom;
                place.Stay_time = int.Parse(EditPendingStayTimeText);
                place.Transit_time = int.Parse(EditPendingTransitTimeText);
                if (EditIsCustom)
                {
                    DateTime customTime = new DateTime(place.Start_time.Year, place.Start_time.Month, place.Start_time.Day, int.Parse(EditPendingCustomHourText), int.Parse(EditPendingCustomMinuteText), 0);
                    place.Start_time = customTime;
                }

                TripDayPlaceDAO tripDayPlaceDAO = Mapper.Map<TripDayPlaceContext, TripDayPlaceDAO>(place);
                tripDetailPresenter.UpdateTripDayPlace(tripDayPlaceDAO);

                IsEditPlacePopupOpen = false;
            });

            this.ChangeTravelModeCommand = new RelayCommand<string>(async mode =>
            {
                TravelMode = (TravelMode)Enum.Parse(typeof(TravelMode), mode);

                await LoadCurrentDayMapAsync();
            });

            this.OpenAddPlaceCommand = new RelayCommand(() =>
            {
                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = true;
            });

            this.CancelAddPlaceCommand = new RelayCommand(() =>
            {
                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = false;
            });

            this.ConfirmAddPlaceCommand = new RelayCommand(() =>
            {
                if (PendingPlace == null) return;

                int stayTime = int.TryParse(PendingTimeText, out int parsedStayTime) ? parsedStayTime : 30;
                int transitTime = int.TryParse(PendingTransitTimeText, out int parsedTransitTime) ? parsedTransitTime : 0;

                bool hasCustomTime = IsCustom
                    && int.TryParse(PendingCustomHourText, out int customHour) && customHour >= 0 && customHour <= 23
                    && int.TryParse(PendingCustomMinuteText, out int customMinute) && customMinute >= 0 && customMinute <= 59;

                DateTime startTime;
                if (hasCustomTime)
                {
                    startTime = new DateTime(CurrentDay.Date.Year, CurrentDay.Date.Month, CurrentDay.Date.Day, int.Parse(PendingCustomHourText), int.Parse(PendingCustomMinuteText), 0);
                }
                else if (CurrentDay.TripDayPlaces == null || CurrentDay.TripDayPlaces.Count == 0)
                {
                    startTime = CurrentDay.StartTime;
                }
                else
                {
                    TripDayPlaceContext lastPlace = CurrentDay.TripDayPlaces.OrderBy(x => x.Start_time).Last();
                    startTime = lastPlace.Start_time.AddMinutes(lastPlace.Transit_time + lastPlace.Stay_time);
                }

                TripDayPlaceDAO tripDayPlace = new TripDayPlaceDAO()
                {
                    TripDays_id = CurrentDay.Id,
                    Place_id = PendingPlace.result.place_id,
                    Place_name = PendingPlace.result.name,
                    Start_time = startTime,
                    Transit_time = transitTime,
                    Stay_time = stayTime,
                    Is_custom = IsCustom,
                };

                tripDetailPresenter.AddTripDayPlace(tripDayPlace);

                PendingPlace = null;
                PendingTimeText = string.Empty;
                PendingTransitTimeText = string.Empty;
                PendingCustomHourText = string.Empty;
                PendingCustomMinuteText = string.Empty;
                IsCustom = false;
                IsAddPlacePopupOpen = false;
            });

            this.ExportCommand = new RelayCommand(() =>
            {
                ExportDataToWord();
            });
        }

        public async Task<PlaceDetailResModel> GetPlaceDetail(string selectedItem, bool with_all_field)
        {
            PlaceDetailRequest placeDetailRequest = new PlaceDetailRequest();
            placeDetailRequest.placeId = selectedItem;

            if (!with_all_field)
            {
                placeDetailRequest.fields = new PlaceDetailInputFields[] { PlaceDetailInputFields.name, PlaceDetailInputFields.formatted_address, PlaceDetailInputFields.type };
            }

            PlaceDetailResModel placeDetailResModel = await GoogleAPIContext.Place.PlaceDetail(placeDetailRequest);

            return placeDetailResModel;
        }

        public async void OnTripsResponse(List<TripDaysDAO> tripDays)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TripDaysDAO, TripDaysContext>();
                cfg.CreateMap<TripDayPlaceDAO, TripDayPlaceContext>();
            });
            var mapper = config.CreateMapper();

            List<TripDaysContext> days = mapper.Map<List<TripDaysContext>>(tripDays);

            foreach (TripDaysContext day in days)
            {
                if (day.TripDayPlaces != null)
                {
                    day.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>(day.TripDayPlaces.OrderBy(x => x.Start_time));
                }
            }

            tripDaysContexts = new ObservableCollection<TripDaysContext>(days);
            CurrentDay = tripDaysContexts[1];

            await LoadCurrentDayMapAsync();
        }

        public async Task LoadCurrentDayMapAsync()
        {
            if (CurrentDay.TripDayPlaces.Count >= 2)
            {
                string StartPlaceId = CurrentDay.TripDayPlaces.First().Place_id;
                string EndPlaceId = CurrentDay.TripDayPlaces.Last().Place_id;

                RoutesRequest routesRequest = new RoutesRequest(StartPlaceId, EndPlaceId, mode: TravelMode, addressType: AddressType.PlaceId);
                if (CurrentDay.TripDayPlaces.Count > 2)
                {
                    List<string> intermediates = new List<string>();
                    for (int i = 1; i < CurrentDay.TripDayPlaces.Count - 1; i++)
                    {
                        intermediates.Add(CurrentDay.TripDayPlaces[i].Place_id);
                    }
                    routesRequest.intermediates = intermediates;
                }
                RoutesResModel routesResModel = await GoogleAPIContext.Route.GetRoutes(routesRequest);

                WeakReferenceMessenger.Default.Send(routesResModel.routes[0].polyline.encodedPolyline.ToList());
            }

            var places = await Task.WhenAll(CurrentDay.TripDayPlaces.Select(x => GetPlaceDetail(x.Place_id, true)));
            foreach (var place in places)
            {
                WeakReferenceMessenger.Default.Send(place);
            }
        }

        public async Task RefreshMapAsync()
        {
            MapVersion++;
            await LoadCurrentDayMapAsync();
        }

        public void OnCreateTripDaysResponse(TripDaysDAO tripDays)
        {
            TripDaysContext tripDay = Utilities.Mapper.Map<TripDaysDAO, TripDaysContext>(tripDays);
            tripDaysContexts.Add(tripDay);
        }

        public async void OnCreateTripDayPlaceResponse(TripDayPlaceDAO tripDayPlace)
        {
            TripDayPlaceContext tripDayPlaceContext = Utilities.Mapper.Map<TripDayPlaceDAO, TripDayPlaceContext>(tripDayPlace);

            if (CurrentDay.TripDayPlaces == null)
            {
                CurrentDay.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>();
            }

            CurrentDay.TripDayPlaces.Add(tripDayPlaceContext);
            await RefreshMapAsync();
        }


        public void OnUpdateTripDayPlaceResponse(List<TripDayPlaceDAO> tripDayPlaceDAOs)
        {

            List<TripDayPlaceContext> tripDayPlaceContexts = Utilities.Mapper.Map<TripDayPlaceDAO, TripDayPlaceContext>(tripDayPlaceDAOs).ToList();

            CurrentDay.TripDayPlaces = new ObservableCollection<TripDayPlaceContext>(tripDayPlaceContexts);

        }



        public async Task ExportDataToWord()
        {
            Document doc = new Document("C:\\Users\\user\\source\\repos\\C#基礎專案\\TravelPlanning\\TravelPlanning\\Templates\\旅遊行程套版_v3.docx");

            // Trip 資料
            string TripName = CurrentTrip.Name;
            string TotalDays = tripDaysContexts.Count().ToString();
            string StartDate = tripDaysContexts.First().DateText;
            string EndDate = tripDaysContexts.Last().DateText;
            byte[] imgData = ConvertBitmapImageToByteArray(CurrentTrip.Cover);


            doc.MailMerge.Execute(
                new[] { "TripName", "TotalDays", "StartDate", "EndDate" },
                new object[] { $"{TripName}", $"{TotalDays}", $"{StartDate}", $"{EndDate}" }
            );


            //tripDaysContexts.ToList().ForEach(async (x) =>
            //{
            //    CurrentDay = x;
            //    await LoadCurrentDayMapAsync();
            //    x.GmapImage = CaptureMapImage?.Invoke();
            //});

            for (int i = 0; i < tripDaysContexts.Count; i++)
            {
                CurrentDay = tripDaysContexts[i];
                await LoadCurrentDayMapAsync();

                if (CaptureMapImage != null)
                {
                    CurrentDay.GmapImage = await CaptureMapImage.Invoke(390);
                }
            }

            // TripDay 資料

            doc.MailMerge.ExecuteWithRegions(new DayDataSource(tripDaysContexts));



            //DataTable productsTable = products.ToDataTable();
            //TripDaysContext Day = CurrentDay;

            //doc.MailMerge.Execute(
            //    new[] { "DayIndex", "DayDate", "DayWeekday", "DepartureTime" },

            //    new object[] { $"{1}", $"{Day.DateText}", $"{Day.Date.DayOfWeek}", $"{Day.StartTimeText}" }
            //);


            // 封面圖
            DocumentBuilder builder = new DocumentBuilder(doc);
            bool moved = builder.MoveToBookmark("TripCover");
            builder.InsertImage(imgData);

            doc.Save("output1.pdf");



            MessageBox.Show("檔案匯出完成!");








        }


        /// <summary>
        /// 將圖片 byte[] 等比例縮放到指定寬度(px)後再轉回 byte[]
        /// </summary>
        //private byte[] ResizeImageByteArray(byte[] imageData, int pixelWidth)
        //{
        //    if (imageData == null || imageData.Length == 0) return imageData;

        //    BitmapImage bitmapImage = new BitmapImage();
        //    using (MemoryStream input = new MemoryStream(imageData))
        //    {
        //        bitmapImage.BeginInit();
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;   // 讀完就關掉 stream
        //        bitmapImage.StreamSource = input;
        //        bitmapImage.DecodePixelWidth = pixelWidth;            // 只設寬度，高度會自動等比例
        //        bitmapImage.EndInit();
        //        bitmapImage.Freeze();
        //    }

        //    using (MemoryStream output = new MemoryStream())
        //    {
        //        BitmapEncoder encoder = new PngBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        //        encoder.Save(output);
        //        return output.ToArray();
        //    }
        //}

        private byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 依你的圖片格式選擇對應的 Encoder，PNG 可保留透明度
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
