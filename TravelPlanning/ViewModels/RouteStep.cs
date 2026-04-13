using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Wpf.Ui.Controls;

namespace TravelPlanning.ViewModels
{
    public class RouteStep
    {
        public SymbolRegular Icon { get; set; }
        public string Instructions { get; set; }
        public string Distance { get; set; }


        public RouteStep(string instructions, string distance, string maneuver)
        {
            this.Instructions = Regex.Replace(instructions, "<[^>]*>", string.Empty);
            this.Distance = distance;

            switch (maneuver)
            {
                case "turn-left":
                    this.Icon = SymbolRegular.ArrowLeft24;
                    break;
                case "turn-right":
                    this.Icon = SymbolRegular.ArrowRight24;
                    break;
                default:
                    this.Icon = SymbolRegular.ArrowUp24;
                    break;
            }

        }
    }
}
