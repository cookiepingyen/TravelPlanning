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
using TravelPlanning.Models.DTO;
using static TravelPlanning.Contracts.MyTripContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class TripDetailContext : IMyTripView
    {
        public Guid TripID { get; set; }

        public ObservableCollection<TripDaysContext> tripDaysContexts { get; set; }

        public ICommand AddDayBtnCommand { get; set; }
        public ICommand DeleteDayBtnCommand { get; set; }

        public TripDetailContext(Guid tripID, PresenterFactory presenterFactory)
        {
            TripID = tripID;

            IMyTripPresenter myTripPresenter = presenterFactory.Create<IMyTripPresenter, IMyTripView>(this);

            TripDAO tripDAO = myTripPresenter.GetTrip(tripID);


            tripDaysContexts = new ObservableCollection<TripDaysContext>()
            {
                new TripDaysContext(1, new DateTime(2026,6,29), true),
                new TripDaysContext(2, new DateTime(2026,6,30), false),
                new TripDaysContext(3, new DateTime(2026,7,1), false),
                new TripDaysContext(4, new DateTime(2026,7,2), false),
            };

            this.AddDayBtnCommand = new RelayCommand(() =>
            {
                TripDaysContext LastTripDay = tripDaysContexts.Last();
                TripDaysContext tripDay = new TripDaysContext(LastTripDay.Day + 1, LastTripDay.Date.AddDays(1), false);
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


        }

        public void OnTripsResponse(List<TripDAO> tripDAOs)
        {
            throw new NotImplementedException();
        }
    }
}
