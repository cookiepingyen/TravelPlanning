using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.Entities;

namespace TravelPlanning.Database.Repositories
{
    public class UserRepository
    {

        public void GetUsers()
        {
            DatabaseContext databaseContext = new DatabaseContext();
            var users = databaseContext.User.ToList();

            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }
        }
    }
}
