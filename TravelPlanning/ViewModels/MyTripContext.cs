using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using IOCServiceCollection;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TravelPlanning.Database.DAO;
using TravelPlanning.Models;
using TravelPlanning.Models.DTO;
using TravelPlanning.Presenters;
using TravelPlanning.Utilities;
using TravelPlanning.Views.Pages;
using TravelPlanning.Views.Pages.Trip;
using Wpf.Ui;
using static TravelPlanning.Contracts.CreateFavoriteContract;
using static TravelPlanning.Contracts.MyTripContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MyTripContext : IMyTripView
    {
        public ObservableCollection<TripDTO> Trips { get; set; }

        public PresenterFactory presenterFactory { get; set; }

        public ICommand CreateTripPageCommand { get; set; }

        public ICommand TripDetailPageCommand { get; set; }

        public ICommand DeleteTripCommand { get; set; }
        private INavigationService navigationService;

        public MyTripContext(PresenterFactory presenterFactory, INavigationService navigationService, IGoogleAPIContext googleAPIContext)
        {

            IMyTripPresenter myTripPresenter = presenterFactory.Create<IMyTripPresenter, IMyTripView>(this);
            myTripPresenter.GetTrips();
            this.navigationService = navigationService;

            this.CreateTripPageCommand = new RelayCommand(() =>
            {
                this.navigationService.Navigate(typeof(CreateTripPage));
            });

            this.TripDetailPageCommand = new RelayCommand<Guid>(ID =>
            {
                this.navigationService.Navigate(typeof(TripDetailPage), new TripDetailContext(ID, presenterFactory, googleAPIContext));
            });

            this.DeleteTripCommand = new RelayCommand<TripDTO>(x =>
            {
                myTripPresenter.DeleteTrip(x.Id);
                this.Trips.Remove(x);
            });

        }

        public void OnTripsResponse(List<TripDAO> tripDAOs)
        {

            List<TripDTO> trips = Mapper.Map<TripDAO, TripDTO>(tripDAOs, config =>
            {
                config.ForMember(dest => dest.Cover, source => source.MapFrom(x => new BitmapImage(new Uri(x.Cover))));
            }).ToList();

            this.Trips = new ObservableCollection<TripDTO>(trips);




            //this.Trips = Mapper.Map<TripDAO, TripDTO>(tripDAOs, config =>
            //{
            //    config.ForMember(dest => dest.Cover, source => source.MapFrom(x => new BitmapImage(new Uri(x.Cover))));
            //}).Aggregate(new ObservableCollection<TripDTO>(), (datas, trip) =>
            //{
            //    trip.End_time = trip.Started_time.AddDays(trip.Days);
            //    datas.Add(trip);
            //    return datas;
            //});

        }
    }
}
