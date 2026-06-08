using IOCServiceCollection;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Database.DAO;
using TravelPlanning.Models;
using TravelPlanning.Utilities;
using TravelPlanning.Utilities.Interfaces;
using Wpf.Ui.Controls;
using static TravelPlanning.Contracts.CreateFavoriteContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class OverviewContext : INavigationAware, IFavoriteView
    {
        public PlaceOverview placeOverview { get; set; }
        public ICommand RoutePlanningPageCommand { get; set; }
        public ICommand ToggleFavoriteCommand { get; set; }
        //public ICommand RemoveFavoriteItemCommand { get; set; }


        public IFavoritePresenter createFavoritePresenter;

        public ObservableCollection<FavoriteListItemContext> FavoriteListItems { get; set; }
        public bool IsPlaceSaved { get; set; }

        public OverviewContext(PresenterFactory presenterFactory)
        {
            createFavoritePresenter = presenterFactory.Create<IFavoritePresenter, IFavoriteView>(this);
            createFavoritePresenter.GetFavoriteListItemsAsync();


            this.ToggleFavoriteCommand = new RelayCommand<FavoriteListItemContext>(async item =>
            {
                if (item.IsContainsCurrentPlace)
                    await createFavoritePresenter.RemoveFavoriteItemAsync(item.Id, placeOverview.PlaceID);
                else
                    await createFavoritePresenter.CreateFavoriteItemAsync(item.Id, placeOverview.PlaceName, placeOverview.PlaceID);

                item.IsContainsCurrentPlace = !item.IsContainsCurrentPlace;
                IsPlaceSaved = FavoriteListItems.Any(x => x.IsContainsCurrentPlace);
            });
        }

        public void LoadData(PlaceOverview placeOverview)
        {
            this.placeOverview = placeOverview;
            _ = UpdateContainsFlagsAsync(placeOverview.PlaceID);
        }

        private async Task UpdateContainsFlagsAsync(string placeId)
        {
            List<Guid> containedIds = await createFavoritePresenter.GetFavoriteIdsByPlaceIdAsync(placeId);
            if (FavoriteListItems == null) return;
            foreach (var item in FavoriteListItems)
                item.IsContainsCurrentPlace = containedIds.Contains(item.Id);
            IsPlaceSaved = FavoriteListItems.Any(x => x.IsContainsCurrentPlace);
        }


        public void DataAware(object data)
        {
            this.placeOverview = (PlaceOverview)data;

        }

        public void OnFaviriteItemsResponse(List<FavoriteDAO> favoriteDAOs)
        {
            List<FavoriteListItemContext> favoriteListItemContexts = Mapper.Map<FavoriteDAO, FavoriteListItemContext>(favoriteDAOs).ToList();

            FavoriteListItems = new ObservableCollection<FavoriteListItemContext>(favoriteListItemContexts);

        }

        public void AddCreatedFaviriteListItem(FavoriteDAO favoriteDAO)
        {
            this.FavoriteListItems.Add(Mapper.Map<FavoriteDAO, FavoriteListItemContext>(favoriteDAO));
        }
    }
}
