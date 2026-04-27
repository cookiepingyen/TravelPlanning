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
    public class OverviewContext : INavigationAware
    {
        public PlaceOverview placeOverview { get; set; }

        public OverviewContext()
        {

        }

        public void LoadData(PlaceOverview placeOverview)
        {
            this.placeOverview = placeOverview;
        }


        public void DataAware(object data)
        {
            this.placeOverview = (PlaceOverview)data;

        }
    }
}
