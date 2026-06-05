using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;

namespace TravelPlanning.Database.Interface
{
    public interface IFavoriteItemRepository
    {
        Task<FavoriteItemDAO> CreateFavoriteItemAsync(Guid favoriteID, string Name, string placeID);

        List<FavoriteItemDAO> GetFavoriteItems(Guid favoriteID);

        Task RemoveFavoriteItemAsync(Guid favoriteItemID);



    }
}
