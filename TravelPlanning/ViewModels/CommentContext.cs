using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml.Linq;
using TravelPlanning.Utilities.Interfaces;
using static GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail.PlaceDetailResModel;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CommentContext : INavigationAware
    {
        private const int PageSize = 3;

        private List<Review> _allComments = new List<Review>();

        public ObservableCollection<Review> DisplayedComments { get; set; } = new ObservableCollection<Review>();

        public bool HasMoreComments { get; set; } = false;

        public ICommand ShowMoreCommand { get; }

        public CommentContext()
        {
            ShowMoreCommand = new RelayCommand(ShowMore);
        }

        private void ShowMore()
        {
            int currentCount = DisplayedComments.Count;
            var next = _allComments.Skip(currentCount).Take(PageSize);
            foreach (var review in next)
                DisplayedComments.Add(review);

            HasMoreComments = DisplayedComments.Count < _allComments.Count;
        }

        public void DataAware(object data)
        {
            _allComments = new List<Review>((Review[])data);
            DisplayedComments = new ObservableCollection<Review>(_allComments.Take(PageSize));
            HasMoreComments = _allComments.Count > PageSize;
        }

        public void LoadComments(IEnumerable<Review> reviews)
        {
            DisplayedComments = new ObservableCollection<Review>(reviews.Take(PageSize));
            HasMoreComments = _allComments.Count > PageSize;
        }
    }
}
