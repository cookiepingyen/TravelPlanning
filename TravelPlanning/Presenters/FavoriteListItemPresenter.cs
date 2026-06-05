using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TravelPlanning.Contracts.CreateFavoriteContract;
using TravelPlanning.Database.Interface;
using static TravelPlanning.Contracts.FaviriteItemContract;
using TravelPlanning.Database.Repositories;
using System.Xml.Linq;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.DAO;

namespace TravelPlanning.Presenters
{
    public class FavoriteListItemPresenter : IFavoriteListItemPresenter
    {
        public IFavoriteItemView FavoriteItemView { get; set; }
        public IFavoriteItemRepository FavoriteItemRepository { get; set; }


        public FavoriteListItemPresenter(IFavoriteItemView favoriteItemView, IFavoriteItemRepository favoriteItemRepository)
        {
            FavoriteItemView = favoriteItemView;
            FavoriteItemRepository = favoriteItemRepository;
        }

        public async Task CreateFavoriteItemAsync(Guid favoriteID, string Name, string placeID)
        {
            FavoriteItemDAO favoriteItemDAO = await FavoriteItemRepository.CreateFavoriteItemAsync(favoriteID, Name, placeID);

            this.FavoriteItemView.OnCreatedFaviriteListItem(favoriteItemDAO);

        }

        public void GetFavoriteItems(Guid favoriteID)
        {
            var temps = FavoriteItemRepository.GetFavoriteItems(favoriteID);
            this.FavoriteItemView.OnFaviriteItemsResponse(temps);
        }

        public Task RemoveFavoriteItemAsync(Guid favoriteItemID)
        {
            return FavoriteItemRepository.RemoveFavoriteItemAsync(favoriteItemID);
        }


    }
}
