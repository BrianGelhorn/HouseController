using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Extensions;
using HouseController.Services;
using HouseController.Views;
using HouseController.Views.PopUps;
using Debug = System.Diagnostics.Debug;
using Zeroconf;

namespace HouseController.ViewModels
{
    public partial class ConnectPageViewModel : ObservableObject
    {
        public ObservableCollection<string> IpList { get; set; }

        private ICommunicationService communicationService;
        private INavigationService navigationService;

        private List<string> ipListToCheck = [];
        private List<string> ipListToIgnore = [];
        private bool scanDevices = true;
        public bool ScanDevices { get => scanDevices; set { scanDevices = value;
            Task.Run(StartDeviceScanning).ConfigureAwait(false);
        } }
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
            while (GetScanDevices())
            {
                const string protocol = "_esp._tcp.local.";
                const string deviceDisplayName = "HouseControllerESP";
                var newIpList = new List<string>();
                try
                {
                    var resolvedHost = await ZeroconfResolver.ResolveAsync(protocol);
                    foreach (var service in resolvedHost)
                    {
                        if (service.DisplayName == deviceDisplayName)
                        {
                            var serviceIp = service.IPAddress;
                            newIpList.Add(serviceIp);
                        }
                    }

                    for (var i = 0; i < IpList.Count; i++)
                    {
                        var ip = IpList[i];
                        if (!newIpList.Contains(ip))
                        {
                            Application.Current?.Dispatcher.Dispatch(() => { IpList.Remove(ip); });
                        }
                    }

                    foreach (var ip in newIpList.Where(ip => !IpList.Contains(ip)))
                    {
                        Application.Current?.Dispatcher.Dispatch(() =>
                        {
                            IpList.Add(ip);
                        });
                    }
                }
                catch (Exception e)
                {
                    //Check if the exception is when "No such host is known" so we can clear the list when there are no HouseControllers
                    if(e.GetType() == typeof(SocketException)) Application.Current?.Dispatcher.Dispatch(() => { IpList.Clear(); });
                }
                await Task.Delay(100);
            }
        }

        [RelayCommand]
        private async Task ConnectToDevice(string ip)
        {
            var connectingPopup = new ConnectingPopup(ip);
            await connectingPopup.ShowAsPopup(false);
            try
            {
                //If successfully connected, Go to the ControllerPage to control the ESP
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