using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace TravelPlanning.ViewModels
{
    internal class CreateTripPageContext
    {
        public string name { get; set; }
        public string destination { get; set; }
        public DateTime date { get; set; }
        public int days { get; set; }

        public string cover_img { get; set; } = "https://png.pngtree.com/png-vector/20191129/ourmid/pngtree-image-upload-icon-photo-upload-icon-png-image_2047546.jpg";


    }
}
