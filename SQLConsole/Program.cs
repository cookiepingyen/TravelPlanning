using SQLConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.Database.Repositories;

namespace SQLConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DatabaseContext Db = new DatabaseContext();


            #region Create User
            //User usr = new User();
            //usr.Id = Guid.NewGuid();
            //usr.Name = "Test2";
            //Db.User.Add(usr);
            //Db.SaveChanges();
            #endregion



            #region Update User
            //User user = Db.User.First(x => x.Name == "Test2");
            //user.Name = "Test";
            //Db.SaveChanges();
            #endregion

            #region Delete User
            //User user = Db.User.First(x => x.Name == "Test");
            //Db.User.Remove(user);

            //Db.SaveChanges();
            #endregion


            #region Select Users
            //foreach (User user in users)
            //{
            //    Console.WriteLine(user.Name);
            //}


            #endregion


            #region Create User_Trip
            //User user = Db.User.First(x => x.Name == "Yen");

            //Trip trip = new Trip();
            //trip.Name = "My Trip";
            //trip.User_id = user.Id;
            //trip.Days = 3;
            //trip.Started_time = DateTime.Now;
            //trip.Ended_time = DateTime.Now.AddDays(3);

            //Db.Trip.Add(trip);
            //Db.SaveChanges();

            #endregion


            Console.WriteLine("Done");

            Console.ReadKey();



        }
    }
}
