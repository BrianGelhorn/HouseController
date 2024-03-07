using HouseController.ViewModels;
using System.Net.Sockets;

namespace HouseController.Models
{
	public record EspData(string DeviceIp, string DeviceName, NetworkStream EspNetworkStream);

	public record DeviceInfo(int Id, string Name, int Status, List<TimeInfo> TimeInfoList);
}