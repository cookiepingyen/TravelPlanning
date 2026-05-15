using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Interface;
using TravelPlanning.Database.Repositories;
using TravelPlanning.ViewModels;
using Wpf.Ui.Controls;
using static TravelPlanning.Contracts.CreateFavoriteContract;

namespace TravelPlanning.Presenters
{
    public class FavoritePresenter : IFavoritePresenter
    {
        public IFavoriteView FavoriteView { get; set; }
        public IFavoriteRepository FavoriteRepository { get; set; }
        public FavoritePresenter(IFavoriteView favoriteView, IFavoriteRepository favoriteRepository)
        {
            this.FavoriteView = favoriteView;
            this.FavoriteRepository = favoriteRepository;
        }

        public void CreateFavorite(string name, string selectIcon)
        {
            FavoriteDAO favoriteDAO = new FavoriteDAO
            {
                Name = name,
                Icon = selectIcon,
            };
            favoriteDAO = FavoriteRepository.CreateFavorite(favoriteDAO);
            FavoriteView.AddCreatedFaviriteListItem(favoriteDAO);

        }

        public Task GetFavoriteListItems()
        {
            return Task.Run(() => this.FavoriteView.OnFaviriteItemsResponse(FavoriteRepository.GetFavorites()));
        }
    }
}
