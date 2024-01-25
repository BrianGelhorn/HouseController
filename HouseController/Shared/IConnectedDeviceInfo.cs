using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HouseController.Models;

namespace HouseController.Shared
{
	public interface IConnectedDeviceInfo
	{
		public DeviceInformation? DeviceInformation { get; set; }
		public ObservableCollection<DeviceData>? DeviceDataList { get; set; }
		public void CreateDeviceInformation(IPEndPoint DeviceIp, string DeviceName, Socket EspSocket);
	}
}
