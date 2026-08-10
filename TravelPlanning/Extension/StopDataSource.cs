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
    public class StopDataSource : IMailMergeDataSource
    {
        private readonly ObservableCollection<TripDayPlaceContext> stops;
        private int index = -1;

        internal StopDataSource(ObservableCollection<TripDayPlaceContext> stops) => this.stops = stops;

        public string TableName => "Stops";
        public bool MoveNext() => ++index < stops.Count;

        public bool GetValue(string fieldName, out object fieldValue)
        {
            var stop = stops[index];
            switch (fieldName)
            {
                case "StopName": fieldValue = stop.Place_name; return true;
                case "StopDuration": fieldValue = stop.Stay_time; return true;
                case "TravelTimeToNext": fieldValue = stop.Transit_time; return true;
                case "StopTime": fieldValue = stop.StartTimeText; return true;
                default: fieldValue = null; return false;
            }
        }

        public IMailMergeDataSource GetChildDataSource(string tableName) => null; // 沒有再下一層
    }
}
