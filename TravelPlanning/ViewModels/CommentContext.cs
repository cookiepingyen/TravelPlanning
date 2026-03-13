using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Utilities.Interfaces;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class CommentContext : INavigationAware
    {
        public string PlaceID = "";


        public void DataAware(object data)
        {
            PlaceID = (string)data;
        }
    }
}
