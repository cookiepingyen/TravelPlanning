using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.Interface;
using TravelPlanning.Database.Repositories;

namespace TravelPlanning.Database
{
    public static class TravelPlanningDatabaseRegistration
    {
        public static void AddTravelPlanningDatabaseRegistration(this IServiceCollection collection)
        {
            collection.AddTransient<ITripRepository, TripRepository>();

        }

    }
}
