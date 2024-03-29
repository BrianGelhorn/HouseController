using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Extensions;
using HouseController.Services;
using HouseController.Views.PopUps;
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

        [ObservableProperty]
        private string name = "";

        [ObservableProperty]
        private int status = -1;
        private ObservableCollection<TimeInfo> _timeInfoList = [];
        public ObservableCollection<TimeInfo> TimeInfoList
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

        [RelayCommand]
        private void TimeTapped(object parameter)
        {
            Debug.WriteLine(parameter);
        }

        [RelayCommand(CanExecute = nameof(CanStatusButtonClicked))]
        private async Task StatusButtonClicked()
        {
            SetCanStatusButton(false);
            var changedStatus = Status == 0 ? 1 : 0;
            await _communicationService.SendDeviceChangeAsync(
                Id,
                nameof(Status),
                changedStatus.ToString()
            );
        }

        private bool CanStatusButtonClicked() => CanStatusButton;

        public void SetCanStatusButton(bool status)
        {
            CanStatusButton = status;
        }

        const string NAME_TYPE = "Name";
        const string STATUS_TYPE = "Status";
        const string TIMEINFOLIST_TYPE = "TimeInfoList";

        public void UpdateDeviceView(string value, string propertyName)
        {
            var property = GetType().GetProperty(propertyName);
            if (property.Name == STATUS_TYPE)
            {
                property.SetValue(this, int.Parse((string)value), null);
                SetCanStatusButton(true);
            }
            else if (property.Name == TIMEINFOLIST_TYPE)
            {
                //ESP32 Sends data in the following format: TimeListIndex-TimeListItem-TimeListStatus
                var values = value.Split("-");
                var wasTimeListIndexParsed = int.TryParse(values[0], out var timeListIndex);
                var timeListItem = values[1];
                var wasTimeListStatusParsed = int.TryParse(values[2], out var timeListStatus);
                if (!(wasTimeListIndexParsed | wasTimeListStatusParsed))
                {
                    //Data Error
                }
                var newTimeInfo = new TimeInfo(timeListItem, timeListStatus);
                TimeInfoList[timeListIndex] = newTimeInfo;
            }
            else
            {
                property.SetValue(this, value, null);
            }
            //Id = deviceInfo.Id;
            //Name = deviceInfo.Name;
            //Status = deviceInfo.Status;
            //TimeInfoList = deviceInfo.TimeInfoList.ToObservableCollection(
        }

        [RelayCommand]
        private async Task OpenChangeTimePopup(object parameter)
        {
            var timeInfo = (TimeInfo)parameter;
            var timePopup = new TimePopup(timeInfo);
            timePopup.BindingContext = timePopup;
            await timePopup.ShowAsPopupAndWaitAsync(); //Wait until the window closes
            var timeInfoIndex = TimeInfoList.IndexOf(timeInfo);
            await _communicationService.SendDeviceChangeAsync(
                Id,
                TIMEINFOLIST_TYPE,
                timeInfoIndex.ToString(),
                timePopup.TimeInfo.Time,
                timePopup.TimeInfo.TimeStatus.ToString()
            );
            TimeInfoList[timeInfoIndex] = timePopup.TimeInfo;
        }
    }

    public partial class TimeInfo(string time, int timeStatus) : ObservableObject
    {
        [ObservableProperty]
        private string _time = time;

        [ObservableProperty]
        private int _timeStatus = timeStatus;

        //[RelayCommand]
        //public async Task TimeTapped(object parameter)
        //{
        //    var timePopup = new TimePopup(TimeStatus, Time);
        //    timePopup.BindingContext = timePopup;
        //    timePopup.CanBeDismissedByTappingOutsideOfPopup = true;
        //    Application.Current?.MainPage?.ShowPopup(timePopup);
        //    while (!timePopup.IsDismissed)
        //    {
        //        await Task.Delay(50);
        //    }

        //    Time = timePopup.TimeStatus.ToString("hh\\:mm");
        //    TimeStatus = timePopup.ButtonStatus;
        //}
    }
}
