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
        public SymbolRegular icon { get; set; }
        public string instructions { get; set; }
        public string distance { get; set; }


        public RouteStep(string instructions, string distance, string maneuver)
        {
            this.instructions = Regex.Replace(instructions, "<[^>]*>", string.Empty);
            this.distance = distance;

            switch (maneuver)
            {
                case "turn-left":
                    this.icon = SymbolRegular.ArrowLeft24;
                    break;
                case "turn-right":
                    this.icon = SymbolRegular.ArrowRight24;
                    break;
                default:
                    this.icon = SymbolRegular.ArrowUp24;
                    break;
            }

        }
    }
}
