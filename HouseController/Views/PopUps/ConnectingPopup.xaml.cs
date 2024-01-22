using CommunityToolkit.Maui.Views;
using HouseController.ViewModels;

namespace HouseController.Views.PopUps;

public partial class ConnectingPopup : Popup
{
	public ConnectingPopup(ConnectingPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}