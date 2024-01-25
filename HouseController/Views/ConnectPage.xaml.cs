using HouseController.ViewModels;

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
