using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Models
{
    [AddINotifyPropertyChangedInterface]
    public class PlaceOverview
    {
        public String Address { get; set; }
        public String Phone { get; set; }
        public string BusinessStatus { get; set; }


        public PlaceOverview(string address, string phone, string businessStatus)
        {
            this.Address = address;
            this.Phone = phone;
            this.BusinessStatus = businessStatus;
        }
    }
}
