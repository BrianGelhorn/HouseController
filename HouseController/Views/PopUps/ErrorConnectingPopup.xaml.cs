using CommunityToolkit.Maui.Views;

namespace HouseController.Views.PopUps;

public partial class ErrorConnectingPopup : Popup
{
	public ErrorConnectingPopup(string ip)
	{
		InitializeComponent();
		ErrorIpLabel.Text = ip;
	}
}