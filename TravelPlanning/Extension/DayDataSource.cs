using Aspose.Words.MailMerging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanning.ViewModels;

namespace TravelPlanning.Extension
{
    public class DayDataSource : IMailMergeDataSource
    {
        private readonly ObservableCollection<TripDaysContext> days;
        private int index = -1;

        internal DayDataSource(ObservableCollection<TripDaysContext> days) => this.days = days;

        public string TableName => "Days";

        public bool MoveNext() => ++index < days.Count;

        public bool GetValue(string fieldName, out object fieldValue)
        {
            var day = days[index];
            switch (fieldName)
            {
                case "DayIndex": fieldValue = index + 1; return true;
                case "DayDate": fieldValue = day.DateText; return true;
                case "DayWeekday": fieldValue = day.Date.DayOfWeek; return true;
                case "DepartureTime": fieldValue = day.StartTimeText; return true;
                case "DayRouteMap": fieldValue = day.GmapImage; return true;
                default: fieldValue = null; return false;
            }
        }

        public IMailMergeDataSource GetChildDataSource(string tableName)
        {
            if (tableName == "Stops")
                return new StopDataSource(days[index].TripDayPlaces);
            return null;
        }
    }
}
