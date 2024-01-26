using HouseController.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using HouseController.ViewModels;

namespace HouseController.Shared
{
	public class ConnectedDeviceInfo : IConnectedDeviceInfo
	{
		public ServerInformation? DeviceInformation { get; set; }
		public ObservableCollection<DeviceViewModel>? DeviceDataList { get; set; }

		public void CreateDeviceInformation(IPEndPoint DeviceIp, string DeviceName, NetworkStream espNetworkStream)
		{
			DeviceInformation = new ServerInformation(DeviceIp, DeviceName, espNetworkStream);
		}
	}
}
