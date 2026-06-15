using IOCServiceCollection;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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

        public BitmapImage CoverImg { get; set; }

        public ICommand CreateTripCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }

        public CreateTripContext(PresenterFactory presenterFactory)
        {
            createTripPresenter = presenterFactory.Create<ICreateTripPresenter, ICreateTripView>(this);

            this.CreateTripCommand = new RelayCommand(CreateTrip);
            this.UploadImageCommand = new RelayCommand(Image_Click);

            this.ClearCommand = new RelayCommand(() =>
            {
                Name = "";
                Days = 0;
            });


            CoverImg = new BitmapImage(new Uri("/Resources/Images/upload.png", UriKind.Relative));
        }


        public async void CreateTrip()
        {
            TripDTO tripDTO = new TripDTO();

            tripDTO.Name = Name;
            tripDTO.Started_time = Date;
            tripDTO.Days = Days;
            tripDTO.Cover = CoverImg;

            createTripPresenter.CreateTrip(tripDTO);

            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Content = "新增成功"
            };

            _ = await uiMessageBox.ShowDialogAsync();
        }


        public void Image_Click()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.JPG;*.PNG;*.GIF)|*.JPG;*.PNG;*.GIF";

            if (openFileDialog.ShowDialog().Value)
            {
                CoverImg = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }

}
