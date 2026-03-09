using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using TravelPlanning.Utilities.Interfaces;
using TravelPlanning.Views.Pages;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class PlaceSearchContext : INavigationAware
    {
        public string PlaceID = "";

        public bool IsVisible { get; set; } = false;

        // 地圖物件資訊
        public String PlaceName { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public double Rating { get; set; }
        public string UserRatingsTotal { get; set; }
        public string BusinessStatus { get; set; }
        public BitmapImage Photo { get; set; }

        public String QueryData { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CommentClickCommand { get; set; }
        public ICommand OverviewClickCommand { get; set; }

        public PlaceSearchContext(INavigationService navigationService)
        {
            navigationService.Navigate("OverviewPage", QueryData);

            this.SearchCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("OverviewPage", QueryData);
            });

            this.OverviewClickCommand = new RelayCommand(() => navigationService.Navigate("OverviewPage", QueryData));
            this.CommentClickCommand = new RelayCommand(() => navigationService.Navigate("CommentPage", QueryData));
        }



        public void DataAware(object data)
        {
            PlaceID = (string)data;
        }

    }
}
