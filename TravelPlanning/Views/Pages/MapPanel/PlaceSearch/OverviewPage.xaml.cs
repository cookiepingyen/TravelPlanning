using System;
using System.Collections.Generic;
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
using TravelPlanning.Utilities;
using TravelPlanning.ViewModels;

namespace TravelPlanning.Views.Pages.MapPanel.PlaceSearch
{
    /// <summary>
    /// Overview.xaml 的互動邏輯
    /// </summary>
    public partial class OverviewPage : Page
    {

        public OverviewPage()
        {
            InitializeComponent();


            DataContext = new OverviewContext();
        }
    }
}
