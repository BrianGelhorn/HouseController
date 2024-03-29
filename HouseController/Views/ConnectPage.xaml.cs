using HouseController.ViewModels;
using System.Diagnostics;

namespace HouseController.Views
{
    public partial class ConnectPage : ContentPage
    {
        public ConnectPage(ConnectPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            var vm = (ConnectPageViewModel)BindingContext;
            //Start scanning devices
            vm.SetScanDevices(true);
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            var vm = (ConnectPageViewModel)BindingContext;
            //Stop scanning devices
            vm.SetScanDevices(false);
        }
    }

}
