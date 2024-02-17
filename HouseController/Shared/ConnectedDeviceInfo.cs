using HouseController.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using HouseController.ViewModels;

namespace HouseController.Shared
{
	public static class ConnectedDeviceInfo
	{
		public static EspData? CurrentEspData { get; set; }
		public static ObservableCollection<DeviceViewModel>? DeviceDataList { get; set; }

		public static void SetCurrentEspData(string deviceIp, string deviceName, NetworkStream espNetworkStream)
		{
			CurrentEspData = new EspData(deviceIp, deviceName, espNetworkStream);
		}
	}
}
