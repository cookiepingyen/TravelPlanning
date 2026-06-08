using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Models.DTO;
using TravelPlanning.ViewModels;

namespace TravelPlanning.Contracts
{
    public class CreateFavoriteContract
    {

        public interface IFavoriteView
        {
            void OnFaviriteItemsResponse(List<FavoriteDAO> favoriteDAOs);

            void AddCreatedFaviriteListItem(FavoriteDAO favoriteDAO);
        }

        public interface IFavoritePresenter
        {
            void CreateFavorite(string name, string selectIcon);

            Task GetFavoriteListItemsAsync();

            Task RemoveFavoriteAsync(Guid favoriteId);
            Task RemoveFavoriteItemAsync(Guid favoriteListId, String placeID);

            Task UpdateFavorite(FavoriteDTO favoriteDTO);
            Task CreateFavoriteItemAsync(Guid favoriteID, string Name, string placeID);

            Task<List<Guid>> GetFavoriteIdsByPlaceIdAsync(string placeId);

        }


    }
}
