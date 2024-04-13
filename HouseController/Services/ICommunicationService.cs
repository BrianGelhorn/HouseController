using System.Collections.ObjectModel;
using System.Net.Sockets;
using HouseController.Shared;
using HouseController.ViewModels;
using DeviceInfo = HouseController.Models.DeviceInfo;

namespace HouseController.Services
{
	public interface ICommunicationService
	{
		public void SetNetworkStream(NetworkStream espNetworkStream);
        public void ClearNetworkStream();
        public NetworkStream? GetNetworkStream();
        public DeviceInfo CreateDeviceInfo(DeviceViewModel deviceViewModel);
        public Task<bool> ConnectToDeviceAsync(string ip, int port, CancellationToken cancellationToken = new());
        public void DisconnectFromDevice();
        public Task<ObservableCollection<DeviceInfo>> GetInitialDataAsync(int bufferSize, CancellationToken cancellationToken);
		public Task SendDeviceChangeAsync(int id, string dataType, params string[] dataValue);
		public Task StartListeningForUpdateAsync(int bufferSize, CancellationToken cancellationToken);
	}
}