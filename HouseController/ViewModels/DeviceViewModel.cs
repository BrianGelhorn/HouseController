using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Models;
using HouseController.Services;
using DeviceInfo = HouseController.Models.DeviceInfo;

namespace HouseController.ViewModels
{
	public partial class DeviceViewModel : ObservableObject
	{
		private readonly ICommunicationService _communicationService;

		public DeviceViewModel(ICommunicationService communicationService, DeviceInfo deviceInfo)
		{
			_communicationService = communicationService;
			Id = deviceInfo.Id;
			Name = deviceInfo.Name;
			Status = deviceInfo.Status;
			TimeInfoList = deviceInfo.TimeInfoList.ToObservableCollection();
		}
		[ObservableProperty] private string? name;
		[ObservableProperty] private bool status;
		public ObservableCollection<TimeInfo>? TimeInfoList { get; set; }
		public int Id { get; set; }

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(StatusButtonClickedCommand))]
		private bool canStatusButton = true;

		[RelayCommand(CanExecute= nameof(CanStatusButtonClicked))]
		private async Task StatusButtonClicked()
		{
			RaiseCanStatusButtonChanged();
			Status = !Status;
			var deviceViewModelChanged = this;
			deviceViewModelChanged.Status = Status;
			var deviceInfoChanged = _communicationService.CreateDeviceInfo(deviceViewModelChanged);
			var deviceChangeCompleted = await _communicationService.SendDeviceChange(deviceInfoChanged);
			if (deviceChangeCompleted)
			{
				RaiseCanStatusButtonChanged();
			}
		}

		private bool CanStatusButtonClicked() => canStatusButton;

		public void RaiseCanStatusButtonChanged()
		{
			CanStatusButton = !CanStatusButton;
		}
	}
}
