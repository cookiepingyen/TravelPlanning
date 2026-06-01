using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.Contract.GoogleMapAPI;
using IOCServiceCollection;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Database.DAO;
using TravelPlanning.Models;
using TravelPlanning.Utilities.Interfaces;
using Wpf.Ui.Controls;
using static TravelPlanning.Contracts.FaviriteItemContract;
using TravelPlanning.Utilities;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class FavoriteListItemContext : INavigationAware, IFavoriteItemView
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int PlaceCount { get; set; }
        public SymbolRegular Icon { get; set; }
        public string EditingName { get; set; }
        public bool IsEditing { get; set; }


        // ==================================


        public ICommand AutoCompleteCommaned { get; set; }
        public ICommand AddPlaceCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        IGoogleAPIContext googleAPIContext;
        private INavigationService navigationService;

        public IFavoriteItemPresenter favoriteItemPresenter;

        public PlaceDetailResModel Place { get; set; }

        public ObservableCollection<FavoriteItemPlaceContext> FavoriteItemPlaceContexts { get; set; }

        public FavoriteListItemContext(IGoogleAPIContext googleAPIContext, PresenterFactory presenterFactory)
        {

            this.googleAPIContext = googleAPIContext;

            favoriteItemPresenter = presenterFactory.Create<IFavoriteItemPresenter, IFavoriteItemView>(this);


            this.AutoCompleteCommaned = new RelayCommand<PlaceDetailResModel>(e =>
            {
                WeakReferenceMessenger.Default.Send(e);
            });

            this.AddPlaceCommand = new RelayCommand<PlaceDetailResModel>(e =>
            {

            });

            this.GoBackCommand = new RelayCommand<PlaceDetailResModel>(item =>
            {
                navigationService.Navigate("FavoritePage", item);
            });
        }

        public FavoriteListItemContext()
        {

        }

        public void DataAware(object data)
        {
            FavoriteItemObject favoriteItemObject = data as FavoriteItemObject;
            navigationService = favoriteItemObject.navigationService;

            Id = favoriteItemObject.favoriteListItemContext.Id;
            Name = favoriteItemObject.favoriteListItemContext.Name;
            Name = favoriteItemObject.favoriteListItemContext.Name;
            Icon = favoriteItemObject.favoriteListItemContext.Icon;
            EditingName = favoriteItemObject.favoriteListItemContext.EditingName;
            IsEditing = favoriteItemObject.favoriteListItemContext.IsEditing;


        }

        public void OnFaviriteItemsResponse(List<FavoriteItemDAO> favoriteItemDAOs)
        {


            List<FavoriteItemPlaceContext> favoriteListItemContexts = favoriteItemDAOs.Select(x =>
            {
                return Mapper.Map<FavoriteItemDAO, FavoriteItemPlaceContext>(x);
            }).ToList();

            FavoriteItemPlaceContexts = new ObservableCollection<FavoriteItemPlaceContext>(favoriteListItemContexts);
        }

        public void AddCreatedFaviriteListItem(FavoriteItemDAO favoriteItemDAO)
        {
            throw new NotImplementedException();
        }





    }
}
