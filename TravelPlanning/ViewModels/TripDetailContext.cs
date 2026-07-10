using AutoMapper;
using IOCServiceCollection;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Models.DAO;
using TravelPlanning.Models.DTO;
using static TravelPlanning.Contracts.MyTripContract;
using static TravelPlanning.Contracts.TripDetailContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class TripDetailContext : ITripDetailView
    {
        public Guid TripID { get; set; }

        public ObservableCollection<TripDaysContext> tripDaysContexts { get; set; }

        public ICommand AddDayBtnCommand { get; set; }
        public ICommand DeleteDayBtnCommand { get; set; }
        public ICommand SelectDayCommand { get; set; }

        public ITripDetailPresenter tripDetailPresenter { get; set; }

        public TripDaysContext CurrentDay { get; set; }



        public TripDetailContext(Guid tripID, PresenterFactory presenterFactory)
        {
            TripID = tripID;

            this.tripDetailPresenter = presenterFactory.Create<ITripDetailPresenter, ITripDetailView>(this);

            tripDetailPresenter.GetTripRequest(tripID);



            this.AddDayBtnCommand = new RelayCommand(() =>
            {
                TripDaysContext LastTripDay = tripDaysContexts.Last();
                TripDaysContext tripDay = new TripDaysContext(LastTripDay.Day + 1, LastTripDay.Date.AddDays(1), LastTripDay.StartTime, false);
                tripDaysContexts.Add(tripDay);
            });

            this.DeleteDayBtnCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                tripDaysContexts.Remove(tripDay);
                var firstDay = tripDaysContexts.OrderBy(x => x.Day).First();
                firstDay.IsChecked = true;

                for (int i = 1; i < tripDaysContexts.Count; i++)
                {
                    TripDaysContext x = tripDaysContexts[i];
                    x.Day = i + 1;
                    x.Date = firstDay.Date.AddDays(i);
                    x.IsChecked = false;
                }
            });


            this.SelectDayCommand = new RelayCommand<TripDaysContext>(tripDay =>
            {
                CurrentDay = tripDay;
            });



        }

        public void OnTripsResponse(List<TripDaysDAO> tripDays)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TripDaysDAO, TripDaysContext>();
                cfg.CreateMap<TripDayPlaceDAO, TripDayPlaceContext>();
            });
            var mapper = config.CreateMapper();

            List<TripDaysContext> days = mapper.Map<List<TripDaysContext>>(tripDays);

            tripDaysContexts = new ObservableCollection<TripDaysContext>(days);
            CurrentDay = tripDaysContexts.FirstOrDefault();
        }
    }
}
