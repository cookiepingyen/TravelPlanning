using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Contracts
{
    public class CreateTripContract
    {


        public interface ICreateTripContext
        {


        }

        public interface ICreateTripPresenter
        {
            void CreateTrip();

        }
    }
}
