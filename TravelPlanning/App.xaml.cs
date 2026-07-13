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
using TravelPlanning.Views.Pages.Favorite;
using System.Reflection;
using static TravelPlanning.Contracts.CreateFavoriteContract;
using TravelPlanning.Presenters;
using TravelPlanning.Database.Repositories;
using TravelPlanning.Database.Interface;
using static TravelPlanning.Contracts.FaviriteItemContract;
using static TravelPlanning.Contracts.CreateTripContract;
using static TravelPlanning.Contracts.MyTripContract;
using TravelPlanning.Views.Pages.Trip;
using static TravelPlanning.Contracts.TripDetailContract;

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
            collection.AutoRegisterMVP(Assembly.GetExecutingAssembly());
            collection.AddTravelPlanningDatabaseRegistration();

            collection.AddSingleton<INavigationService, NavigationService>();

            collection.AddSingleton<Window, MainTravelWindow>();

            #region Trip
            collection.AddTransient<CreateTripPage, CreateTripPage>();
            collection.AddTransient<ICreateTripView, CreateTripContext>();
            collection.AddTransient<ICreateTripPresenter, CreateTripPresenter>();

            collection.AddTransient<MyTripPage, MyTripPage>();
            collection.AddTransient<IMyTripPresenter, MyTripPresenter>();
            collection.AddTransient<IMyTripView, MyTripContext>();

            #endregion


            #region TripDetail
            collection.AddTransient<TripDetailPage, TripDetailPage>();
            collection.AddTransient<ITripDetailPresenter, TripDetailPresenter>();
            collection.AddTransient<ITripDetailView, TripDetailContext>();
            collection.AddTransient<ITripDayPlaceRepository, TripDayPlaceRepository>();
            #endregion


            collection.AddTransient<RoutePlanningPage, RoutePlanningPage>();
            collection.AddTransient<MapPanelPage, MapPanelPage>();
            collection.AddTransient<OverviewControl, OverviewControl>();
            collection.AddTransient<PlaceSearchPage, PlaceSearchPage>();
            collection.AddTransient<PlaceSearchContext, PlaceSearchContext>();
            collection.AddTransient<CommentControl, CommentControl>();
            collection.AddTransient<CommentContext, CommentContext>();
            collection.AddTransient<OverviewContext, OverviewContext>();


            #region Favorite
            collection.AddTransient<FavoriteContext, FavoriteContext>();
            collection.AddTransient<FavoritePage, FavoritePage>();
            collection.AddTransient<FavoriteListItemPage, FavoriteListItemPage>();
            collection.AddTransient<FavoriteListItemContext, FavoriteListItemContext>();
            collection.AddTransient<IFavoritePresenter, FavoritePresenter>();
            collection.AddTransient<IFavoriteRepository, FavoriteRepository>();
            collection.AddTransient<IFavoriteListItemPresenter, FavoriteListItemPresenter>();
            collection.AddTransient<IFavoriteItemRepository, FavoriteItemRepository>();
            #endregion


            collection.AddTransient<FavoriteListItemContext, FavoriteListItemContext>();



            provider = collection.BuildServiceProvider();

            Window window = provider.GetService<Window>();
            window.Show();
        }
    }
}
