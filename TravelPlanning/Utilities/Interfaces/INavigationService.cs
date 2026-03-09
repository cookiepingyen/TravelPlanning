using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanning.Utilities.Interfaces
{
    public interface INavigationService
    {
        void Navigate(string page, object parm);
    }
}
