using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Interface;
using TravelPlanning.Database.Repositories;
using static TravelPlanning.Contracts.CreateFavoriteContract;

namespace TravelPlanning.Presenters
{
    public class FavoritePresenter : IFavoritePresenter
    {
        public IFavoriteRepository FavoriteRepository { get; set; }
        public FavoritePresenter(IFavoriteRepository favoriteRepository)
        {
            this.FavoriteRepository = favoriteRepository;
        }

        public void CreateFavorite(string name)
        {
            FavoriteDAO favoriteDAO = new FavoriteDAO { Name = name };
            FavoriteRepository.CreateFavorite(favoriteDAO);
        }
    }
}
