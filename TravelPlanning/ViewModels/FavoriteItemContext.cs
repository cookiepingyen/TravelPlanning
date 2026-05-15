using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace TravelPlanning.ViewModels
{
    public class FavoriteListItemContext
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int PlaceCount { get; set; }
        public SymbolRegular Icon { get; set; }


    }
}
