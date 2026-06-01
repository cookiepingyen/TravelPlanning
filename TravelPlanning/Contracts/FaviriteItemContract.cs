using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Models.DTO;

namespace TravelPlanning.Contracts
{
    public class FaviriteItemContract
    {

        public interface IFavoriteItemView
        {
            void OnFaviriteItemsResponse(List<FavoriteItemDAO> favoriteItemDAOs);

            void AddCreatedFaviriteListItem(FavoriteItemDAO favoriteItemDAO);
        }

        public interface IFavoriteItemPresenter
        {
            Task CreateFavoriteItemAsync(Guid favoriteID, string Name);

            void GetFavoriteItems(Guid favoriteID);

            Task RemoveFavoriteItemAsync(Guid favoriteItemID);
        }

    }
}
