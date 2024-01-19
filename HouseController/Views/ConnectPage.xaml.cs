using HouseController.ViewModels;
using System.Net;
using System.Net.Sockets;

namespace HouseController.Views
{
    public partial class ConnectPage : ContentPage
    {

        public ConnectPage(ConnectPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}
