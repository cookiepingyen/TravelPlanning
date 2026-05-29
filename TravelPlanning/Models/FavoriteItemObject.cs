using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Utilities.Interfaces;
using TravelPlanning.ViewModels;

namespace TravelPlanning.Models
{
    public class FavoriteItemObject
    {

        public FavoriteListItemContext favoriteListItemContext { get; set; }

        public INavigationService navigationService { get; set; }


        public FavoriteItemObject(FavoriteListItemContext favoriteListItemContext, INavigationService navigationService)
        {
            this.favoriteListItemContext = favoriteListItemContext;
            this.navigationService = navigationService;
        }

    }
}
