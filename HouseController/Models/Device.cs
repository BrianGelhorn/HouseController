using System;
using System.Collections.Generic;
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
		public int Status { get; set; }
		public List<string>? Times { get; set; }
		public List<string>? TimesStatus { get; set; }
	}
	public record DeviceInformation(IPEndPoint DeviceIp, string DeviceName, Socket EspSocket);
}
