using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Extensions;
using HouseController.Services;
using HouseController.Views;
using HouseController.Views.PopUps;
using Zeroconf;

namespace HouseController.ViewModels
{
    public partial class ConnectPageViewModel(
        ICommunicationService communicationService,
        INavigationService navigationService)
        : ObservableObject
    {
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")] 
        public ObservableCollection<string> IpList { get; set; } = [];

        private bool _scanDevices = true;
        private bool ScanDevices { get => _scanDevices; set { _scanDevices = value;
            Task.Run(StartDeviceScanning).ConfigureAwait(false);
        } }
        public void SetScanDevices(bool status) { ScanDevices = status; }
        private bool GetScanDevices() { return ScanDevices; }

        private async Task StartDeviceScanning()
        {
            while (GetScanDevices())
            {
                const string protocol = "_esp._tcp.local.";
                const string deviceDisplayName = "HouseControllerESP";
                
                //Temporal list to save the IPs of the new scan
                var newIpList = new List<string>();
                try
                {
                    var resolvedHost = await ZeroconfResolver.ResolveAsync(protocol);
                    newIpList.AddRange(resolvedHost.Where(service => service.DisplayName == deviceDisplayName).Select(service => service.IPAddress));
                    
                    //If an ip from IpList doesn't appear on the new scan, remove it from IpList
                    for (var i = 0; i < IpList.Count; i++)
                    {
                        var ip = IpList[i];
                        if (newIpList.Contains(ip)) continue;
                        Application.Current?.Dispatcher.Dispatch(() => IpList.Remove(ip));
                    }
                    
                    //If the new scan has an ip that is not on the IpList, add it to the IpList
                    foreach (var ip in newIpList.Where(ip => !IpList.Contains(ip)))
                    {
                        Application.Current?.Dispatcher.Dispatch(() => IpList.Add(ip));
                    }
                }
                catch (Exception e)
                {
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
            catch
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