using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Models
{
	public record DeviceData
	{
		public string? Name { get; set; }
		public bool Status { get; set; }
		public ObservableCollection<TimeInfo>? TimeInfoList { get; set; }
	}

	public class TimeInfo
	{
		public string? Time { get; set; }

		public bool TimeStatus { get; set; }
	}

	public record DeviceInformation(IPEndPoint DeviceIp, string DeviceName, Socket EspSocket);
}