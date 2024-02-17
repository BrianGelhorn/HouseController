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
		IPopupService popupService
	) : ObservableObject

	{
		[RelayCommand]
		private void ConnectToDevice(string ip)
		{
			var connectCancellationTokenSource = new CancellationTokenSource();
			CancellationToken connectCancellationToken = connectCancellationTokenSource.Token;
			var connectingPopup = new ConnectingPopup(ip);
			var errorConnectingPopup = new ErrorConnectingPopup(ip);

			//connectingPopup.Closed += ((sender, args) =>
			//{
			//	connectCancellationTokenSource.Cancel();
			//});
			popupService.ShowPopup(connectingPopup, true);
			var isConnected = false;
			Task.Run(async () =>
				{
					try
					{
						isConnected = await communicationService.ConnectToDeviceAsync(ip, 2500, connectCancellationToken);
						if (isConnected)
						{
							await navigationService.GoToAsync(nameof(ControllerPage));
						}
						else
						{
							throw new Exception("Connecting Failed");
						}
					}
					catch(Exception e)
					{
						//Debug.WriteLine(e.Message);
						//popupService.ClosePopup(connectingPopup);
						//popupService.ShowPopup(errorConnectingPopup, true);
					}
				},
				connectCancellationToken);
		}
	}
}