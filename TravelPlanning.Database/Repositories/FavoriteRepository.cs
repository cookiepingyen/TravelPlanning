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
    public class FavoriteRepository : IFavoriteRepository
    {
        public DatabaseContext db = new DatabaseContext();

        public void AddFavoriteItem(FavoriteDAO favoriteDAO, FavoriteItemDAO favoriteItemDAO)
        {
            Favorite favorite = db.Favorite.FirstOrDefault(x => x.Id == favoriteDAO.Id);

            if (favorite != null)
            {
                FavoriteItem favoriteItem = Mapper.Map<FavoriteItemDAO, FavoriteItem>(favoriteItemDAO);
                favoriteItem.Favorite_id = favoriteDAO.Id;
                favoriteItem.Id = Guid.NewGuid();
                db.FavoriteItem.Add(favoriteItem);
                db.SaveChanges();
            }

        }

        public FavoriteDAO CreateFavorite(FavoriteDAO favoriteDAO)
        {
            Favorite favorite = Mapper.Map<FavoriteDAO, Favorite>(favoriteDAO);

            favorite.Id = Guid.NewGuid();
            favorite.User_id = Guid.Parse("2CB96EE9-689F-44A6-A730-C14AE767E5C8");
            db.Favorite.Add(favorite);
            db.SaveChanges();

            return favoriteDAO;
        }

        public async Task UpdateFavoriteAsync(FavoriteDAO favoriteDAO)
        {
            Favorite favorite = db.Favorite.FirstOrDefault(x => x.Id == favoriteDAO.Id);
            favorite.Name = favoriteDAO.Name;
            favorite.Icon = favoriteDAO.Icon;
            int count = await db.SaveChangesAsync();
        }

        public List<FavoriteDAO> GetFavorites()
        {
            List<FavoriteDAO> favorites = db.Favorite.Select(x => new FavoriteDAO()
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                PlaceCount = x.FavoriteItem.Count

            }).OrderBy(f => f.Name).ToList();
            return favorites;
        }

        public async Task DeleteFavoriteAsync(Guid favoriteID)
        {
            Favorite favorite = await db.Favorite.FirstOrDefaultAsync(x => x.Id == favoriteID) ?? throw new KeyNotFoundException();
            List<FavoriteItem> favoriteItems = db.FavoriteItem.Where(x => x.Favorite_id == favoriteID).ToList();
            db.FavoriteItem.RemoveRange(favoriteItems);
            db.Favorite.Remove(favorite);
            await db.SaveChangesAsync();
        }

        public void DeleteFavoriteItem(Guid favoriteItemID)
        {
            FavoriteItem favoriteItem = db.FavoriteItem.FirstOrDefault(x => x.Id == favoriteItemID);
            db.FavoriteItem.Remove(favoriteItem);
            db.SaveChanges();
        }

        List<FavoriteItemDAO> IFavoriteRepository.GetFavoriteItems(Guid favoriteID)
        {
            List<FavoriteItem> favoriteItems = db.FavoriteItem.Where(x => x.Favorite_id == favoriteID).ToList();
            List<FavoriteItemDAO> favoriteItemDAOs = Mapper.Map<List<FavoriteItem>, List<FavoriteItemDAO>>(favoriteItems);
            return favoriteItemDAOs;
        }


    }
}
