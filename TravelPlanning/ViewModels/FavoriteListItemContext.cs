using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelPlanning.Utilities.Interfaces;
using Wpf.Ui.Controls;

namespace TravelPlanning.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class FavoriteListItemContext
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int PlaceCount { get; set; }
        public SymbolRegular Icon { get; set; }
        public string EditingName { get; set; }
        public bool IsEditing { get; set; }
    }
}
