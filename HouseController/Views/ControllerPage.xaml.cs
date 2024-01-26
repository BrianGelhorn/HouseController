using HouseController.ViewModels;

namespace HouseController.Views;
public partial class ControllerPage : ContentPage
{
	public ControllerPage(ControllerPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}