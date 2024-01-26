using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Models;
using HouseController.Services;
using HouseController.Shared;
using HouseController.Views;
using HouseController.Views.PopUps;

namespace HouseController.ViewModels
{
	public partial class ConnectPageViewModel(
		IDeviceDiscoverService deviceDiscoverService,
		ICommunicationService communicationService,
		INavigationService navigationService,
		IPopupService popupService,
		IConnectedDeviceInfo connectedDeviceInfo
	) : ObservableObject
	{
		[ObservableProperty] ObservableCollection<ServerInformation>? deviceInformationList;

		[RelayCommand]
		private async Task ConnectToDevice(string ip)
		{
			Debug.WriteLine(ip);
			var tcpClient = new TcpClient();
			var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 2500);
			var connectingPopup = new ConnectingPopup(ip);
			try
			{
				await tcpClient.ConnectAsync(ipEndPoint);
				var networkStream = tcpClient.GetStream();
				popupService.ShowPopup(connectingPopup);
				if (tcpClient.Connected)
				{
					connectedDeviceInfo.CreateDeviceInformation(ipEndPoint, ip, networkStream);
					communicationService.SetCurrentSocket(networkStream);
					await navigationService.GoToAsync(nameof(ControllerPage));
				}

				popupService.ClosePopup(connectingPopup);
			}
			catch (Exception e)
			{
				popupService.ClosePopup(connectingPopup);
				var errorPopup = new ErrorConnectingPopup(ip);
				popupService.ShowPopup(errorPopup);
			}
		}
	}
}