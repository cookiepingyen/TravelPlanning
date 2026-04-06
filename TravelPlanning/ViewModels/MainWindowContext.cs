using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelPlanning.Attributes;
using Wpf.Ui.Controls;

namespace TravelPlanning.ViewModels
{
    internal class MainWindowContext
    {

        private string _applicationTitle = "WPF UI Gallery";

        public ObservableCollection<NavigationViewItem> MenuItems { get; set; } = new ObservableCollection<NavigationViewItem>()
        {

        };


        public MainWindowContext()
        {
            Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(x => x.BaseType == typeof(Page) && x.GetCustomAttribute<LeftSidebarAttribute>() != null)
                .Select(x =>
                {
                    LeftSidebarAttribute attr = x.GetCustomAttribute<LeftSidebarAttribute>();
                    return new NavigationViewItem(attr.PageName, attr.Icon, x);
                })
                .ToList()
                .ForEach(x => MenuItems.Add(x));

        }



        public ObservableCollection<NavigationViewItem> FooterItems { get; set; } = new ObservableCollection<NavigationViewItem>()
        {

        };


    }
}
