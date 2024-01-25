using CommunityToolkit.Maui.Views;
using HouseController.ViewModels;

namespace HouseController.Views.PopUps;

public partial class ConnectingPopup : Popup
{
	public string Ip { get; set; }
	public ConnectingPopup(string Ip)
	{
		InitializeComponent();
		BindingContext = this;
		this.Ip = Ip;
	}
}