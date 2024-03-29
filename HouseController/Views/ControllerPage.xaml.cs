using System.Diagnostics;
using HouseController.ViewModels;

namespace HouseController.Views;
public partial class ControllerPage : ContentPage
{
	public ControllerPage(ControllerPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override bool OnBackButtonPressed()
    {
        var vm = (ControllerPageViewModel)BindingContext;
        vm.DisconnectDevice();
        return base.OnBackButtonPressed();
    }
}