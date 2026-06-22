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

        public TripDetailContext(Guid tripID)
        {
            TripID = tripID;
        }
    }
}
