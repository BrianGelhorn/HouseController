using System.Diagnostics;
using HouseController.ViewModels;
using System.Net.Sockets;

namespace HouseController.Views;
public partial class ControllerPage : ContentPage
{
	public ControllerPage(ControllerPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}