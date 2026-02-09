using IOCServiceCollection;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Models.DTO;
using Wpf.Ui.Input;
using static System.Net.WebRequestMethods;
using static TravelPlanning.Contracts.CreateTripContract;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CreateTripContext : ICreateTripView
    {
        public ICreateTripPresenter createTripPresenter;

        public string Name { get; set; } = "北海道";
        public DateTime Date { get; set; } = DateTime.Now;
        public int Days { get; set; } = 3;

        public string CoverImg { get; set; } = "https://png.pngtree.com/png-vector/20191129/ourmid/pngtree-image-upload-icon-photo-upload-icon-png-image_2047546.jpg";

        public ICommand CreateTripCommand { get; set; }

        public CreateTripContext(PresenterFactory presenterFactory)
        {
            createTripPresenter = presenterFactory.Create<ICreateTripPresenter, ICreateTripView>(this);

            this.CreateTripCommand = new RelayCommand(CreateTrip);


        }


        public void CreateTrip()
        {
            TripDTO tripDTO = new TripDTO();

            tripDTO.Name = Name;
            tripDTO.Started_time = Date;
            tripDTO.Days = Days;
            tripDTO.Cover = CoverImg;

            createTripPresenter.CreateTrip(tripDTO);
        }

    }
}
