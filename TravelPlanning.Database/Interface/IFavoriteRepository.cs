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
        void CreateFavorite(FavoriteDAO favoriteDAO);
        void UpdateFavorite(FavoriteDAO favoriteDAO);
        void DeleteFavorite(Guid favoriteID);


        void AddFavoriteItem(FavoriteDAO favoriteDAO, FavoriteItemDAO favoriteItemDAO);
        List<FavoriteItemDAO> GetFavoriteItems(Guid favoriteID);

        void DeleteFavoriteItem(Guid favoriteItemID);


    }
}
