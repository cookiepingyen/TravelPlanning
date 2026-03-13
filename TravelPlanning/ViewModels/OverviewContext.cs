using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Models;
using TravelPlanning.Utilities.Interfaces;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class OverviewContext : INavigationAware
    {
        public PlaceOverview placeOverview { get; set; }

        public OverviewContext()
        {

        }


        public void DataAware(object data)
        {
            this.placeOverview = (PlaceOverview)data;

        }
    }
}
