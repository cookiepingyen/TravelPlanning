using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TravelPlanning.Database.DAO;
using TravelPlanning.Database.Entities;
using TravelPlanning.Database.Interface;
using TravelPlanning.Models.DTO;
using TravelPlanning.Utilities;
using static TravelPlanning.Contracts.CreateTripContract;

namespace TravelPlanning.Presenters
{
    public class CreateTripPresenter : ICreateTripPresenter
    {
        ITripRepository tripRepository;

        public CreateTripPresenter(ITripRepository tripRepository)
        {
            this.tripRepository = tripRepository;
        }

        public void CreateTrip(TripDTO tripDTO)
        {
            TripDAO tripDAO = Mapper.Map<TripDTO, TripDAO>(tripDTO);

            // 儲存圖片
            string coverPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\TravelPlanning\\data\\{tripDAO.Started_time.Date.ToString("yyyy-MM-dd")}\\{Guid.NewGuid()}.jpg";
            BitmapImage comPressImg = ImageCompress.Compress(tripDTO.Cover);
            ImageCompress.Save(comPressImg, coverPath);

            tripDAO.Cover = coverPath;

            tripRepository.CreateTrip(tripDAO);

        }



    }
}
