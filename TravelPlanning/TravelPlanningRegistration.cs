using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Presenters;
using TravelPlanning.ViewModels;
using static TravelPlanning.Contracts.CreateTripContract;

namespace TravelPlanning
{
    public static class TravelPlanningRegistration
    {

        public static void AddTravelPlanningRegistration(this IServiceCollection collection)
        {
            collection.AddTransient<ICreateTripView, CreateTripContext>();
            collection.AddTransient<ICreateTripPresenter, CreateTripPresenter>();
        }
    }
}
