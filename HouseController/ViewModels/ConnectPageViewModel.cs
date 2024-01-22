using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Models;
using HouseController.Services;
using HouseController.Views;
using HouseController.Views.PopUps;

namespace HouseController.ViewModels
{
    public partial class ConnectPageViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<DeviceInformation>? deviceInformationList;

        IDeviceDiscoverService deviceDiscoverService;
        ICommunicationService communicationService;
        INavigationService navigationService;

        public ConnectPageViewModel(
            IDeviceDiscoverService deviceDiscoverService,
            ICommunicationService communicationService,
            INavigationService navigationService
        )
        {
            this.deviceDiscoverService = deviceDiscoverService;
            this.communicationService = communicationService;
            this.navigationService = navigationService;
        }

        [RelayCommand]
        private async Task ConnectToDevice(string ip)
        {
            Debug.WriteLine(ip);
            var socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 2500);
            var connectingPopup = new ConnectingPopup(new ConnectingPopupViewModel());

            try
            {
                Application.Current?.MainPage?.ShowPopup(connectingPopup);
                await socket.ConnectAsync(ipEndPoint);
                if (socket.Connected)
                {
                    await navigationService.GoToAsync(nameof(ControllerPage));
                }
            }
            catch
            {
                var errorPopup = new ErrorConnectingPopup(ip);
                Application.Current?.MainPage?.ShowPopup(errorPopup);
            }
            finally
            {
                connectingPopup.Close();
            }
        }
    }
}
