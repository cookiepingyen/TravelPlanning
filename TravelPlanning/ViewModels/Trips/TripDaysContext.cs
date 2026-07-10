using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class TripDaysContext
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public string DateText => Date.ToString("M月dd日");
        public string StartTimeText => StartTime.ToString("HH:mm");

        public bool IsChecked { get; set; }
        public List<TripDayPlaceContext> TripDayPlaces { get; set; }

        public TripDaysContext() { }
        public TripDaysContext(int day, DateTime date, DateTime startTime, bool ischecked)
        {
            Day = day;
            Date = date;
            StartTime = startTime;
            IsChecked = ischecked;
        }
    }
}
