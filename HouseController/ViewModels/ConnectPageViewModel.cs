using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Extensions;
using HouseController.Services;
using HouseController.Views;
using HouseController.Views.PopUps;

namespace HouseController.ViewModels
{
    public partial class ConnectPageViewModel : ObservableObject
    {
        public ObservableCollection<string> IpList { get; set; }

        private ICommunicationService communicationService;
        private INavigationService navigationService;

        List<string> ipListToCheck = [];
        private bool scanDevices = true;
        public bool ScanDevices { get => scanDevices; set { scanDevices = value; Task.Run(StartDeviceScanning); } }
        public void SetScanDevices(bool status) { ScanDevices = status; }
        public bool GetScanDevices() { return ScanDevices; }

        public ConnectPageViewModel(
            ICommunicationService communicationService,
            INavigationService navigationService
        )
        {
            this.communicationService = communicationService;
            this.navigationService = navigationService;
            //Add all the possible private IPs to check
            for (int i = 1; i < 255; i++)
            {
                ipListToCheck.Add($"192.168.0.{i}");
            }
            IpList = [];
        }

        private async Task StartDeviceScanning()
        {
            while(GetScanDevices())
            {
                List<Task<string>> tasksList = new List<Task<string>>();
                //Create a list with all the tasks to check if the IP it's from a device
                foreach (var ip in ipListToCheck)
                {
                    tasksList.Add(communicationService.CheckDevice(ip, 2000));
                }
                await Task.WhenAll(tasksList);
                //Temporal list to filter IPs
                var newIpList = new ObservableCollection<string>();
                foreach (var deviceChecked in tasksList)
                {
                    var ip = deviceChecked.Result;
                    if (ip == "") continue;
                    newIpList.Add(ip);
                }
                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    foreach (var ip in ipListToCheck)
                    {
                        //Check if the IP is already in the IpList, if it's not remove it, otherwise add it
                        if (IpList.Contains(ip))
                        {
                            if (newIpList.Contains(ip)) continue;
                            IpList.Remove(ip);
                        }
                        else if (newIpList.Contains(ip))
                        {
                            IpList.Add(ip);
                        }
                    }
                });
                await Task.Delay(1000);
            }
        }

        [RelayCommand]
        private async Task ConnectToDevice(string ip)
        {
            var connectingPopup = new ConnectingPopup(ip);
            await connectingPopup.ShowAsPopup(false);
            try
            {
                //If succesfully connected, Go to the ControllerPage to control the ESP
                var isConnected = await communicationService.ConnectToDeviceAsync(ip, 2500);
                if (isConnected)
                {
                    await navigationService.GoToAsync(nameof(ControllerPage));
                }
                else
                {
                    throw new Exception("Connecting Failed");
                }
            }
            catch (Exception e)
            {
                var errorConnectingPopup = new ErrorConnectingPopup(ip);
                await errorConnectingPopup.ShowAsPopupAndWaitAsync();
            }
            finally
            {
                await connectingPopup.ClosePopupAsync();
            }
        }
    }
}
