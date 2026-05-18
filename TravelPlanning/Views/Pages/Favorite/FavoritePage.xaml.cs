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
using TravelPlanning.Database.DAO;
using TravelPlanning.ViewModels;
using static TravelPlanning.Contracts.CreateFavoriteContract;

namespace TravelPlanning.Views.Pages.Favorite
{
    /// <summary>
    /// FavoritePage.xaml 的互動邏輯
    /// </summary>
    public partial class FavoritePage : Page
    {
        public FavoritePage(FavoriteContext favoriteContext)
        {
            InitializeComponent();
            DataContext = favoriteContext;
        }


        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }

    }
}
