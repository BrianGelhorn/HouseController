using System.Collections.ObjectModel;
using System.Net.Sockets;
using HouseController.ViewModels;
using DeviceInfo = HouseController.Models.DeviceInfo;

namespace HouseController.Services
{
	public interface ICommunicationService
	{
		public NetworkStream EspNetworkStream { get; set; } 
		public Task<bool> SendDeviceChange(DeviceInfo deviceInfo);
		public Task<ObservableCollection<DeviceInfo>> GetInitialData(int recvBuffer);
		public void SetCurrentSocket(NetworkStream espNetworkStream);
		public DeviceInfo CreateDeviceInfo(DeviceViewModel deviceViewModel);
	}
}