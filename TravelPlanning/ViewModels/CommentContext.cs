using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelPlanning.Utilities.Interfaces;
using static GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail.PlaceDetailResModel;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class CommentContext : INavigationAware
    {

        public ObservableCollection<Review> Comments { get; set; } = new ObservableCollection<Review>();


        public void DataAware(object data)
        {
            Comments = new ObservableCollection<Review>((Review[])data);
        }
    }
}
