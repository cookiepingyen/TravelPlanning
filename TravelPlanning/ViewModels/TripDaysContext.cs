using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class TripDaysContext
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public string DateText => Date.ToString("M月dd日");
        public bool IsChecked { get; set; }

        public TripDaysContext() { }
        public TripDaysContext(int day, DateTime date, bool ischecked)
        {
            Day = day;
            Date = date;
            IsChecked = ischecked;
        }
    }
}
