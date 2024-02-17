using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Models;
using HouseController.Services;
using System.Collections.ObjectModel;
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
		[ObservableProperty] private int status;
		private ObservableCollection<TimeInfo>? _timeInfoList;
		public ObservableCollection<TimeInfo>? TimeInfoList
		{
			get => _timeInfoList;
			set
			{
				_timeInfoList = value;
				OnPropertyChanged();
			}
		}

		public int Id { get; set; }

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(StatusButtonClickedCommand))]
		private bool canStatusButton = true;

		[RelayCommand(CanExecute = nameof(CanStatusButtonClicked))]
		private async Task StatusButtonClicked()
		{
			RaiseCanStatusButtonChanged();
			var changedStatus = Status == 0 ? 1 : 0;
			await _communicationService.SendDeviceChangeAsync(Id, nameof(Status), changedStatus.ToString());
		}

		private bool CanStatusButtonClicked() => CanStatusButton;

		public void RaiseCanStatusButtonChanged()
		{
			CanStatusButton = !CanStatusButton;
		}
		const string NAME_TYPE = "Name";
		const string STATUS_TYPE = "Status";
		const string TIMEINFOLIST_TYPE = "TimeInfoList";
		const string TIMEINFOLISTTIME_TYPE = "Time";
		const string TIMEINFOLISTTIMESTATUS_TYPE = "TimeStatus";

        public void UpdateDeviceView(object value, string propertyName)
        {
            var property = GetType().GetProperty(propertyName);
            if (property.Name == STATUS_TYPE)
            {
                property.SetValue(this, int.Parse((string)value), null);
			}
			else
			{ 
                property.SetValue(this, value, null);
			}
			//Id = deviceInfo.Id;	
			//Name = deviceInfo.Name;
			//Status = deviceInfo.Status;
			//TimeInfoList = deviceInfo.TimeInfoList.ToObservableCollection();
			RaiseCanStatusButtonChanged();
		}
	}
}
