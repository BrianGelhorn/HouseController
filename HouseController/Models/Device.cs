using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace HouseController.Models
{
	public record EspData(string DeviceIp, string DeviceName, NetworkStream EspNetworkStream);

	public record DeviceInfo(int Id, string Name, int Status, List<TimeInfo> TimeInfoList);
	public class TimeInfo
	{
		public string? Time { get; set; }

		public int TimeStatus { get; set; }
	}
}