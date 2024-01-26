using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace HouseController.Models
{
	public record ServerInformation(IPEndPoint DeviceIp, string DeviceName, NetworkStream EspNetworkStream);

	public record DeviceInfo(int Id, string Name, bool Status, List<TimeInfo> TimeInfoList);
	public class TimeInfo
	{
		public string? Time { get; set; }

		public bool TimeStatus { get; set; }
	}
}