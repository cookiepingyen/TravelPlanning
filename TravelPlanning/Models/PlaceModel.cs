using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail.PlaceDetailResModel;

namespace TravelPlanning.Models
{
    [AddINotifyPropertyChangedInterface]
    internal class PlaceModel
    {
        public string PlaceID { get; set; }
        public String PlaceName { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }

        public double Rating { get; set; }
        public string UserRatingsTotal { get; set; }
        public string BusinessStatus { get; set; }
        public BitmapImage Photo { get; set; }
        public Review[] Reviews { get; set; }

        public bool isvisible { get; set; }

        public bool? IsOpening { get; set; }
        public string BusinessText => GetBusinessText(IsOpening);


        private string GetBusinessText(bool? isOpening)
        {
            string businessStatusText = "未提供";
            if (isOpening != null)
            {
                businessStatusText = isOpening.Value ? "營業中" : "已打烊";
            }
            return businessStatusText;
        }
    }
}
