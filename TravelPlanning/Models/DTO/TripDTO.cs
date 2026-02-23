using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TravelPlanning.Models.DTO
{
    public class TripDTO
    {
        public string Name { get; set; }

        public DateTime Started_time { get; set; }
        public int Days { get; set; }

        public BitmapImage Cover { get; set; }
    }
}
