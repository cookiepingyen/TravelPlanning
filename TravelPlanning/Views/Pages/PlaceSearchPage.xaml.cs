using GoogleMap.SDK.Contract.GoogleMap;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using GoogleMap.SDK.UI.WPF.Components.AutoComplete;
using IOCServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TravelPlanning.Views.Pages
{
    /// <summary>
    /// PlaceSearchPage.xaml 的互動邏輯
    /// </summary>
    public partial class PlaceSearchPage : Page
    {
        PlaceAutoCompleteView originAutoCompleteView;
        IMapControl mapControl;
        ServiceProvider serviceProvider;
        IGoogleAPIContext googleAPIContext;
        GoogleMapMarker selectedMarker;



        public PlaceSearchPage(ServiceProvider provider, IGoogleAPIContext googleAPIContext)
        {
            InitializeComponent();
            mapControl = serviceProvider.GetService<IMapControl>();

            Control control = (Control)mapControl;

            container.Children.Add(control);


        }
    }
}
