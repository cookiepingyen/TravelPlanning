using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Database.DAO
{
    public class FavoriteItemDAO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid Favorite_id { get; set; }

        public string Place_id { get; set; }
    }
}
