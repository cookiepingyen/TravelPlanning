using IOCServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TravelPlanning.Views;
using TravelPlanning.Views.Pages;
using TravelPlanning.Views.Pages.MapPanel.PlaceSearch;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;
using ServiceCollection = IOCServiceCollection.ServiceCollection;
using TravelPlanning.Database;
using GoogleMap.SDK.Core;
using GoogleMap.SDK.UI.WPF;
using TravelPlanning.Views.Pages.RoutePlanning;
using TravelPlanning.ViewModels;

namespace TravelPlanning
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider provider = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var collection = new ServiceCollection();
            collection.AddGoogleMapCoreRegistration();
            collection.AddGoogleMapWPFRegistration();
            collection.AddNavigationViewPageProvider();
            collection.AddTravelPlanningRegistration();
            collection.AddTravelPlanningDatabaseRegistration();

            collection.AddSingleton<INavigationService, NavigationService>();

            collection.AddSingleton<Window, MainTravelWindow>();
            collection.AddTransient<CreateTripPage, CreateTripPage>();
            collection.AddTransient<MyTripPage, MyTripPage>();
            collection.AddTransient<RoutePlanningPage, RoutePlanningPage>();
            collection.AddTransient<MapPanelPage, MapPanelPage>();
            collection.AddTransient<OverviewControl, OverviewControl>();
            collection.AddTransient<PlaceSearchPage, PlaceSearchPage>();
            collection.AddTransient<CommentControl, CommentControl>();
            collection.AddTransient<CommentContext, CommentContext>();
            collection.AddTransient<OverviewContext, OverviewContext>();


            provider = collection.BuildServiceProvider();

            Window window = provider.GetService<Window>();
            window.Show();
        }
    }
}
