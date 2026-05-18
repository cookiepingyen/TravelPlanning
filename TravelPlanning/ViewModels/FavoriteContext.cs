using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using IOCServiceCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Models.DTO;
using TravelPlanning.Utilities;
using Wpf.Ui.Controls;
using static TravelPlanning.Contracts.CreateFavoriteContract;
using static TravelPlanning.Contracts.CreateTripContract;

namespace TravelPlanning.ViewModels
{
    public class FavoriteContext : INotifyPropertyChanged, IFavoriteView
    {
        public SymbolRegular[] Icons { get; set; }
        private SymbolRegular _selectedIcon = SymbolRegular.Star24;
        public SymbolRegular SelectedIcon
        {
            get => _selectedIcon;
            set
            {
                _selectedIcon = value;
                OnPropertyChanged(nameof(SelectedIcon));
            }
        }

        public IFavoritePresenter createFavoritePresenter;
        public string _favoriteName { get; set; }
        public string FavoriteName
        {
            get
            {
                return _favoriteName;
            }
            set
            {
                _favoriteName = value;
                OnPropertyChanged(nameof(FavoriteName));

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CreateFavoriteCommand { get; set; }
        public ICommand EditFavoriteCommand { get; set; }
        public ICommand DeleteFavoriteCommand { get; set; }

        private ObservableCollection<FavoriteListItemContext> favoriteContexts;
        public ObservableCollection<FavoriteListItemContext> FavoriteListItems
        {
            get
            {
                return favoriteContexts;
            }
            set
            {
                favoriteContexts = value;
                OnPropertyChanged(nameof(FavoriteListItems));
            }

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public FavoriteContext(PresenterFactory presenterFactory)
        {
            Icons = (SymbolRegular[])Enum.GetValues(typeof(SymbolRegular));

            createFavoritePresenter = presenterFactory.Create<IFavoritePresenter, IFavoriteView>(this);


            createFavoritePresenter.GetFavoriteListItemsAsync();

            this.CreateFavoriteCommand = new RelayCommand(() =>
            {
                createFavoritePresenter.CreateFavorite(_favoriteName, _selectedIcon.ToString());
                _favoriteName = null;
            });

            //this.EditFavoriteCommand = new RelayCommand(() => { });
            this.DeleteFavoriteCommand = new RelayCommand<FavoriteListItemContext>((item) =>
            {
                createFavoritePresenter.RemoveFavoriteAsync(item.Id);
                FavoriteListItems.Remove(item);
            });


        }

        public void OnFaviriteItemsResponse(List<FavoriteDAO> favoriteDAOs)
        {
            List<FavoriteListItemContext> favoriteListItemContexts = favoriteDAOs.Select(x =>
            {
                return Mapper.Map<FavoriteDAO, FavoriteListItemContext>(x);
            }).ToList();

            FavoriteListItems = new ObservableCollection<FavoriteListItemContext>(favoriteListItemContexts);
        }


        public void AddCreatedFaviriteListItem(FavoriteDAO favoriteDAO)
        {
            this.FavoriteListItems.Add(Mapper.Map<FavoriteDAO, FavoriteListItemContext>(favoriteDAO));
        }

    }
}
