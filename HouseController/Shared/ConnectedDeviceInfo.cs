using HouseController.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Shared
{
	public class ConnectedDeviceInfo : IConnectedDeviceInfo
	{
		public DeviceInformation? DeviceInformation { get; set; }
		public ObservableCollection<DeviceData>? DeviceDataList { get; set; }

		public void CreateDeviceInformation(IPEndPoint DeviceIp, string DeviceName, Socket EspSocket)
		{
			DeviceInformation = new DeviceInformation(DeviceIp, DeviceName, EspSocket);
		}
	}
}
