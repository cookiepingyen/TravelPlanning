using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Interface;
using TravelPlanning.Database.Utility;

namespace TravelPlanning.Database.Repositories
{
    public class FavoriteItemRepository : IFavoriteItemRepository
    {

        public DatabaseContext db = new DatabaseContext();

        public async Task<FavoriteItemDAO> CreateFavoriteItemAsync(Guid favoriteID, string Name, string placeID)
        {

            FavoriteItem favoriteItem = db.FavoriteItem.FirstOrDefault(x => x.Favorite_id == favoriteID && x.Place_id == placeID);

            if (favoriteItem != null)
            {
                return Mapper.Map<FavoriteItem, FavoriteItemDAO>(favoriteItem);
            }
            favoriteItem = new FavoriteItem()
            {
                Id = Guid.NewGuid(),
                Favorite_id = favoriteID,
                Place_id = placeID,
                Name = Name,
            };

            db.FavoriteItem.Add(favoriteItem);
            await db.SaveChangesAsync();

            return Mapper.Map<FavoriteItem, FavoriteItemDAO>(favoriteItem);

        }

        public List<FavoriteItemDAO> GetFavoriteItems(Guid favoriteID)
        {
            List<FavoriteItemDAO> favoriteItemDAOs = db.FavoriteItem.Where(x => x.Favorite_id == favoriteID)
                                                                    .Select(x => new FavoriteItemDAO()
                                                                    {
                                                                        Id = x.Id,
                                                                        Name = x.Name,
                                                                        Favorite_id = x.Favorite_id
                                                                    }).OrderBy(f => f.Name).ToList();
            return favoriteItemDAOs;
        }

        public async Task RemoveFavoriteItemAsync(Guid favoriteItemID)
        {
            FavoriteItem favoriteItem = await db.FavoriteItem.FirstOrDefaultAsync(x => x.Id == favoriteItemID) ?? throw new KeyNotFoundException();

            db.FavoriteItem.Remove(favoriteItem);
            await db.SaveChangesAsync();

        }
    }
}
