using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Models.DTO;

namespace TravelPlanning.ViewModels
{
    public class TripDetailContext
    {
        public Guid TripID { get; set; }

        public ObservableCollection<TripDaysContext> tripDaysContexts { get; set; }


        public TripDetailContext(Guid tripID)
        {
            TripID = tripID;

            tripDaysContexts = new ObservableCollection<TripDaysContext>()
            {
                new TripDaysContext(1, new DateTime(2026,6,29), true),
                new TripDaysContext(2, new DateTime(2026,6,30), false),
                new TripDaysContext(3, new DateTime(2026,7,1), false),
            };
        }




    }
}
