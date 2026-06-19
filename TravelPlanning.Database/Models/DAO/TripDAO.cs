using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Database.DAO
{
    public class TripDAO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Started_time { get; set; }
        public DateTime End_time { get; set; }

        public int Days { get; set; }

        public string Cover { get; set; }
    }
}
