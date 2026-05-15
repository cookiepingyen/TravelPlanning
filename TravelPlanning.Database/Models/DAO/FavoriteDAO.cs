using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Database.DAO
{
    public class FavoriteDAO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int PlaceCount { get; set; }

        public string Icon { get; set; }
    }
}
