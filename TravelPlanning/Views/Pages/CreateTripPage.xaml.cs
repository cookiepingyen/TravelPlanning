using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelPlanning.ViewModels;

namespace TravelPlanning.Views.Pages
{
    /// <summary>
    /// CreateTripPage.xaml 的互動邏輯
    /// </summary>
    public partial class CreateTripPage : Page
    {
        CreateTripContext createTripPageContext;
        public CreateTripPage(CreateTripContext createTripPageContext)
        {
            InitializeComponent();
            this.createTripPageContext = createTripPageContext;
        }
    }
}
