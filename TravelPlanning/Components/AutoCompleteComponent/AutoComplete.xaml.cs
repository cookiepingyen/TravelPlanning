using GoogleMap.SDK.Contract.GoogleMapAPI.Models;
using GoogleMap.SDK.Contract.GoogleMapAPI.Models.Place.PlaceDetail;
using GoogleMap.SDK.UI.WPF.Components.AutoComplete;
using IOCServiceCollection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelPlanning.Views.Pages;
using static GoogleMap.SDK.Contract.Components.AutoComplete.AutoCompleteContract;

namespace TravelPlanning.Components.AutoCompleteComponent
{
    /// <summary>
    /// AutoComplete.xaml 的互動邏輯
    /// </summary>
    public partial class AutoComplete : UserControl
    {
        PlaceAutoCompleteView AutoCompleteView;

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
            nameof(SelectedCommand),
            typeof(ICommand),
            typeof(AutoComplete),
            new PropertyMetadata());

        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }


        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(PlaceDetailResModel),
            typeof(AutoComplete),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PlaceDetailResModel SelectedItem
        {
            get => (PlaceDetailResModel)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public AutoComplete()
        {

            InitializeComponent();
        }

        private void AutoComplete_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompleteView = (PlaceAutoCompleteView)App.provider.GetService<IAutoCompleteView>();
            AutoCompleteView.selectChange += PlaceAutoCompleteView_selectChange;

            AutoCompleteView.Padding = new Thickness(5);
            AutoCompleteView.VerticalContentAlignment = VerticalAlignment.Center;
            AutoCompleteView.Background = new SolidColorBrush(Colors.Transparent);
            AutoCompleteView.Foreground = new SolidColorBrush(Colors.White);
            AutoCompleteView.BorderThickness = new Thickness(0);

            autoCompletePanel.Children.Add(AutoCompleteView);

        }

        private void PlaceAutoCompleteView_selectChange(object sender, PlaceDetailResModel e)
        {
            SelectedItem = e;
            SelectedCommand.Execute(e);
        }
    }
}
