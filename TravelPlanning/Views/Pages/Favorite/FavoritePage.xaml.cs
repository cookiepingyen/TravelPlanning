using GoogleMap.SDK.Contract.GoogleMapAPI;
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
using static TravelPlanning.Contracts.CreateFavoriteContract;

namespace TravelPlanning.Views.Pages.Favorite
{
    /// <summary>
    /// FavoritePage.xaml 的互動邏輯
    /// </summary>
    public partial class FavoritePage : Page, ICreateFavoriteView
    {
        public FavoritePage(FavoriteContext favoriteContext)
        {
            InitializeComponent();

            DataContext = favoriteContext;
        }
    }
}
