using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.DAO;
using TravelPlanning.Models.DTO;

namespace TravelPlanning.Contracts
{
    public class CreateFavoriteContract
    {

        public interface ICreateFavoriteView
        {


        }

        public interface IFavoritePresenter
        {
            void CreateFavorite(string name);


        }


    }
}
