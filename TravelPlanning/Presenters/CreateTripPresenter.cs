using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Interface;
using TravelPlanning.Models.DTO;
using TravelPlanning.Utilities;
using static TravelPlanning.Contracts.CreateTripContract;

namespace TravelPlanning.Presenters
{
    public class CreateTripPresenter : ICreateTripPresenter
    {
        ITripRepository tripRepository;

        public CreateTripPresenter(ITripRepository tripRepository)
        {
            this.tripRepository = tripRepository;
        }

        public void CreateTrip(TripDTO tripDTO)
        {
            TripDAO tripDAO = Mapper.Map<TripDTO, TripDAO>(tripDTO);
            tripRepository.CreateTrip(tripDAO);
        }
    }
}
