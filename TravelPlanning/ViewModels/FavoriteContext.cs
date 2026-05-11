using CommunityToolkit.Mvvm.Messaging;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static TravelPlanning.Contracts.CreateFavoriteContract;
using static TravelPlanning.Contracts.CreateTripContract;

namespace TravelPlanning.ViewModels
{
    public class FavoriteContext : INotifyPropertyChanged, ICreateFavoriteView
    {
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



        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public FavoriteContext(IFavoritePresenter createFavoritePresenter)
        {
            this.CreateFavoriteCommand = new RelayCommand(() =>
            {
                createFavoritePresenter.CreateFavorite(_favoriteName);
                _favoriteName = null;
            });
        }

    }
}
