using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelPlanning.Utilities.Interfaces;
using TravelPlanning.Views.Pages.MapPanel.PlaceSearch;

namespace TravelPlanning.Utilities
{
    internal class NavService : INavigationService
    {
        private Frame _frame;
        private string currentPage = "";

        private Dictionary<string, Page> pages = new Dictionary<string, Page>();
        public NavService(Frame frame)
        {
            this._frame = frame;
        }

        public void Navigate(string pageName, object parm = null)
        {
            if (currentPage == pageName)
                return;

            Page page = null;

            Type pageType = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.Name == pageName);

            if (pages.ContainsKey(pageName))
            {
                page = pages[pageName];
            }
            else
            {
                page = (Page)App.provider.GetService(pageType);
                if (page == null)
                {
                    page = (Page)Activator.CreateInstance(pageType);
                }

                pages.Add(pageName, page);
            }


            if (parm != null && page.DataContext is INavigationAware navigationAware)
            {
                navigationAware.DataAware(parm);
            }
            _frame.Navigate(page);

        }
    }
}
