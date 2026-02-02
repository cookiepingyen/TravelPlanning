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
using TravelPlanning.ViewModels;
using Wpf.Ui;

namespace TravelPlanning.Views
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainTravelWindow
    {
        public static IServiceProvider provider = null;

        MainWindowContext viewModel { get; set; } = new MainWindowContext();

        public MainTravelWindow(INavigationService navigationService)
        {
            InitializeComponent();

            DataContext = viewModel;
            navigationService.SetNavigationControl(NavigationView);


        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
