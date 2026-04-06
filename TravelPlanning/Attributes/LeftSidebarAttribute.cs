using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace TravelPlanning.Attributes
{
    internal class LeftSidebarAttribute : Attribute
    {
        public string PageName { get; set; }
        public SymbolRegular Icon { get; set; }

        public LeftSidebarAttribute(string pageName, SymbolRegular icon = SymbolRegular.Building24)
        {
            this.PageName = pageName;
            this.Icon = icon;
        }

    }
}
