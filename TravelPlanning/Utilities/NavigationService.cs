using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
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


            _frame.Navigate(page);
            DispatcherHelper.DoEvents();
            if (parm != null && page.DataContext is INavigationAware navigationAware)
            {
                navigationAware.DataAware(parm);
            }

        }



        public static class DispatcherHelper
        {
            [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
                try { Dispatcher.PushFrame(frame); }
                catch (InvalidOperationException) { }
            }
            private static object ExitFrames(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }
    }
}
