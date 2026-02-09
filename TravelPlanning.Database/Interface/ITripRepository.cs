using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;

namespace TravelPlanning.Database.Interface
{
    public interface ITripRepository
    {

        void CreateTrip(TripDAO tripDAO);
    }
}
