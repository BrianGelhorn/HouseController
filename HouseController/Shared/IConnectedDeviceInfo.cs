using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net;
using HouseController.Models;
using HouseController.ViewModels;

namespace HouseController.Shared
{
	public interface IConnectedDeviceInfo
	{
		public ServerInformation? DeviceInformation { get; set; }
		public ObservableCollection<DeviceViewModel>? DeviceDataList { get; set; }
		public void CreateDeviceInformation(IPEndPoint DeviceIp, string DeviceName, NetworkStream espNetworkStream);
	}
}
