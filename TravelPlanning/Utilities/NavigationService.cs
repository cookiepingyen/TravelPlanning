using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelPlanning.Utilities.Interfaces;

namespace TravelPlanning.Utilities
{
    internal class NavService : INavigationService
    {
        private Frame _frame;
        public NavService(Frame frame)
        {
            this._frame = frame;
        }

        public void Navigate(string pageName, object parm)
        {
            Type pageType = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.Name == pageName);
            Page page = (Page)Activator.CreateInstance(pageType);

            if (page.DataContext is INavigationAware navigationAware)
            {
                navigationAware.DataAware(parm);
            }
            _frame.Navigate(page);

        }
    }
}
