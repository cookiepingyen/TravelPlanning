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
        public int PlaceCount => FavoriteItemPlaceContexts.Count;
        public SymbolRegular Icon { get; set; }
        public string EditingName { get; set; }
        public bool IsEditing { get; set; }
        public bool IsContainsCurrentPlace { get; set; }


        // ==================================


        public ICommand AutoCompleteCommaned { get; set; }
        public ICommand AddFavoriteItemPlaceCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand DeleteFavoriteItemPlaceCommand { get; set; }

        IGoogleAPIContext googleAPIContext;
        private INavigationService navigationService;

        public IFavoriteListItemPresenter favoriteItemPresenter;

        public PlaceDetailResModel Place { get; set; }

        public ObservableCollection<FavoriteItemPlaceContext> FavoriteItemPlaceContexts { get; set; } = new ObservableCollection<FavoriteItemPlaceContext>();

        public FavoriteListItemContext(IGoogleAPIContext googleAPIContext, PresenterFactory presenterFactory)
        {

            this.googleAPIContext = googleAPIContext;

            favoriteItemPresenter = presenterFactory.Create<IFavoriteListItemPresenter, IFavoriteItemView>(this);


            this.AutoCompleteCommaned = new RelayCommand<PlaceDetailResModel>(e =>
            {
                WeakReferenceMessenger.Default.Send(e);
            });

            this.AddFavoriteItemPlaceCommand = new RelayCommand(async () =>
            {
                await favoriteItemPresenter.CreateFavoriteItemAsync(Id, Place.result.name, Place.result.place_id);

            });

            this.GoBackCommand = new RelayCommand<PlaceDetailResModel>(item =>
            {
                navigationService.Navigate("FavoritePage", item);
            });

            this.DeleteFavoriteItemPlaceCommand = new RelayCommand<FavoriteItemPlaceContext>(async (item) =>
            {
                await favoriteItemPresenter.RemoveFavoriteItemAsync(item.Id);
                FavoriteItemPlaceContexts.Remove(item);
                FavoriteItemPlaceContexts = new ObservableCollection<FavoriteItemPlaceContext>(FavoriteItemPlaceContexts);

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
            Icon = favoriteItemObject.favoriteListItemContext.Icon;
            EditingName = favoriteItemObject.favoriteListItemContext.EditingName;
            IsEditing = favoriteItemObject.favoriteListItemContext.IsEditing;
            favoriteItemPresenter.GetFavoriteItems(Id);

        }

        public void OnFaviriteItemsResponse(List<FavoriteItemDAO> favoriteItemDAOs)
        {
            List<FavoriteItemPlaceContext> favoriteListItemContexts = favoriteItemDAOs.Select(x =>
            {
                return Mapper.Map<FavoriteItemDAO, FavoriteItemPlaceContext>(x);
            }).ToList();

            FavoriteItemPlaceContexts = new ObservableCollection<FavoriteItemPlaceContext>(favoriteListItemContexts);
        }

        public void OnCreatedFaviriteListItem(FavoriteItemDAO favoriteItemDAO)
        {
            FavoriteItemPlaceContext placeItem = Mapper.Map<FavoriteItemDAO, FavoriteItemPlaceContext>(favoriteItemDAO);
            FavoriteItemPlaceContexts.Add(placeItem);
            FavoriteItemPlaceContexts = new ObservableCollection<FavoriteItemPlaceContext>(FavoriteItemPlaceContexts);
        }





    }
}
