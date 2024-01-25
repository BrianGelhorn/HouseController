using CommunityToolkit.Mvvm.ComponentModel;
using HouseController.Models;
using HouseController.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Sockets;
using HouseController.Shared;

namespace HouseController.ViewModels
{
	public partial class ControllerPageViewModel : ObservableObject
	{
		private ObservableCollection<DeviceData>? _deviceList;

		public ObservableCollection<DeviceData>? DeviceList
		{
			get => _deviceList;
			set
			{
				_deviceList = value;
				OnPropertyChanged();
			}
		}

		private readonly ICommunicationService _communicationService;

		private readonly Socket? _socket;

		public ControllerPageViewModel(ICommunicationService communicationService,
			IConnectedDeviceInfo connectedDeviceInfo)
		{
			_communicationService = communicationService;
			if (connectedDeviceInfo.DeviceInformation == null)
			{
				return;
			}

			_socket = connectedDeviceInfo.DeviceInformation.EspSocket;
			GetDeviceList();
		}

		public void GetDeviceList()
		{
			Task.Run(async () =>
			{
				var deviceList = await _communicationService.GetInitialData(_socket, 4096);
				DeviceList = deviceList;
			});
		}
	}
}