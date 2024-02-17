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

		public ControllerPageViewModel(ICommunicationService communicationService)
		{
			_communicationService = communicationService;
			if (ConnectedDeviceInfo.CurrentEspData == null)
			{
				return;
			}
			GetDeviceList();
		}

		public void GetDeviceList()
		{
			var listeningCancellationSource = new CancellationTokenSource();
			var listeningCancellationToken = listeningCancellationSource.Token;
			Task.Run(async () =>
			{
				var deviceInfoList = await _communicationService.GetInitialDataAsync(4096, listeningCancellationToken);
				var deviceViewModelList = new ObservableCollection<DeviceViewModel>();
				foreach (var device in deviceInfoList)
				{
					var deviceViewModel = new DeviceViewModel(_communicationService, device);
					deviceViewModelList.Add(deviceViewModel);
				}
				DeviceList = deviceViewModelList;
				ConnectedDeviceInfo.DeviceDataList = DeviceList;
				await _communicationService.StartListeningForUpdateAsync(4096, listeningCancellationToken);
			});
		}
	}
}