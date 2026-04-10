using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using GoogleMap.SDK.Contract.GoogleMap;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlacePhoto;
using GoogleMap.SDK.UI.WPF.Components.AutoComplete;
using IOCServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
using TravelPlanning.Attributes;
using TravelPlanning.Models;
using TravelPlanning.ViewModels;
using Wpf.Ui;
using static GoogleMap.SDK.Contract.Components.AutoComplete.AutoCompleteContract;

namespace TravelPlanning.Views.Pages.RoutePlanning
{
    /// <summary>
    /// RoutePlanningPage.xaml 的互動邏輯
    /// </summary>
    /// 
    public partial class RoutePlanningPage : Page
    {
        IGoogleAPIContext googleAPIContext;

        public RoutePlanningPage(IGoogleAPIContext googleAPIContext)
        {
            InitializeComponent();
            DataContext = new RoutePlanningContext(googleAPIContext);
        }
    }
}
