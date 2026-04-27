using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMap;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using IOCServiceCollection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelPlanning.Models;
using TravelPlanning.Utilities;
using TravelPlanning.Utilities.Interfaces;
using TravelPlanning.ViewModels;

namespace TravelPlanning.Views.Pages.MapPanel.PlaceSearch
{
    /// <summary>
    /// PlaceSearchPage.xaml 的互動邏輯
    /// </summary>
    public partial class PlaceSearchPage : Page
    {

        public PlaceSearchPage(IGoogleAPIContext googleAPIContext, OverviewContext overviewContext, CommentContext commentContext)
        {
            InitializeComponent();

            DataContext = new PlaceSearchContext(googleAPIContext, overviewContext, commentContext);
        }

    }
}
