using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.ViewModels
{
    public class FavoriteItemPlaceContext
    {
        public Guid Id { get; set; }

        public Guid FavoriteId { get; set; }

        public string Name { get; set; }


    }
}
