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

namespace TravelPlanning.Presenters
{
    public class FavoriteItemPresenter : IFavoriteItemPresenter
    {
        public IFavoriteItemView FavoriteItemView { get; set; }
        public IFavoriteItemRepository FavoriteItemRepository { get; set; }


        public FavoriteItemPresenter(IFavoriteItemView favoriteItemView, IFavoriteItemRepository favoriteItemRepository)
        {
            FavoriteItemView = favoriteItemView;
            FavoriteItemRepository = favoriteItemRepository;
        }

        public Task CreateFavoriteItemAsync(Guid favoriteID, string Name)
        {
            return FavoriteItemRepository.CreateFavoriteItemAsync(favoriteID, Name);
        }

        public void GetFavoriteItems(Guid favoriteID)
        {
            this.FavoriteItemView.OnFaviriteItemsResponse(FavoriteItemRepository.GetFavoriteItems(favoriteID));
        }

        public Task RemoveFavoriteItemAsync(Guid favoriteItemID)
        {
            return FavoriteItemRepository.RemoveFavoriteItemAsync(favoriteItemID);
        }
    }
}
