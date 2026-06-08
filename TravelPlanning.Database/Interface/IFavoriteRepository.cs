using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;

namespace TravelPlanning.Database.Interface
{
    public interface IFavoriteRepository
    {
        List<FavoriteDAO> GetFavorites();
        FavoriteDAO CreateFavorite(FavoriteDAO favoriteDAO);
        Task UpdateFavoriteAsync(FavoriteDAO favoriteDAO);
        Task DeleteFavoriteAsync(Guid favoriteID);


        void AddFavoriteItem(FavoriteDAO favoriteDAO, FavoriteItemDAO favoriteItemDAO);
        List<FavoriteItemDAO> GetFavoriteItems(Guid favoriteID);

        Task DeleteFavoriteItemAsync(Guid favoriteListID, string placeID);

        List<Guid> GetFavoriteIdsByPlaceId(string placeId);

    }
}
