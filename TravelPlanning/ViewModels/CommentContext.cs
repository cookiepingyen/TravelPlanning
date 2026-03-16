using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Utilities.Interfaces;
using static GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail.PlaceDetailResModel;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class CommentContext : INavigationAware
    {
        public Review[] Reviews { get; set; }


        public void DataAware(object data)
        {
            Reviews = (Review[])data;
        }
    }
}
