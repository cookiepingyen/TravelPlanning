using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TravelPlanning.Models
{
    [AddINotifyPropertyChangedInterface]
    internal class PlaceCard
    {
        public String PlaceName { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }

        public double Rating { get; set; }
        public string UserRatingsTotal { get; set; }
        public string BusinessStatus { get; set; }

        public BitmapImage Photo { get; set; }
    }
}
