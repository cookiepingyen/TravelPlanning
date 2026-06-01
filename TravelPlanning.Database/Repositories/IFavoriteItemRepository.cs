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

        public async Task CreateFavoriteItemAsync(Guid favoriteID, string Name)
        {

            FavoriteItem favoriteItem = new FavoriteItem()
            {
                Id = Guid.NewGuid(),
                Favorite_id = favoriteID,
                Name = Name,
            };

            db.FavoriteItem.Add(favoriteItem);
            await db.SaveChangesAsync();
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
