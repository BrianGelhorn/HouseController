using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HouseController.Models;
using HouseController.Services;
using System.Net.Sockets;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using HouseController.Shared;

namespace HouseController.ViewModels
{
	public partial class ControllerPageViewModel : ObservableObject
	{
		private ObservableCollection<DeviceViewModel>? _deviceList;

		public ObservableCollection<DeviceViewModel>? DeviceList
		{
			get => _deviceList;
			set
			{
				_deviceList = value;
				OnPropertyChanged();
			}
		}

		private readonly ICommunicationService _communicationService;

		public ControllerPageViewModel(ICommunicationService communicationService,
			IConnectedDeviceInfo connectedDeviceInfo)
		{
			_communicationService = communicationService;
			if (connectedDeviceInfo.DeviceInformation == null)
			{
				return;
			}

			GetDeviceList();
		}

		public void GetDeviceList()
		{
			Task.Run(async () =>
			{
				var deviceInfoList = await _communicationService.GetInitialData(4096);
				var deviceViewModelList = new ObservableCollection<DeviceViewModel>();
				foreach (var device in deviceInfoList)
				{
					var deviceViewModel = new DeviceViewModel(_communicationService, device);
					deviceViewModelList.Add(deviceViewModel);
				}
				DeviceList = deviceViewModelList;
			});
		}
	}
}