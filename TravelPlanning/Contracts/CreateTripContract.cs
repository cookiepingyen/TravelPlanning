using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Models.DTO;

namespace TravelPlanning.Contracts
{
    public class CreateTripContract
    {
        public interface ICreateTripView
        {


        }

        public interface ICreateTripPresenter
        {
            void CreateTrip(TripDTO tripDTO);

        }
    }
}
