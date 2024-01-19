using HouseController.Models;
using System.Net;
using System.Net.Sockets;

namespace HouseController.Services
{
	public interface ICommunicationService
	{
		public Task<bool> SendDeviceChange(Socket espSocket, DeviceInformation device);
		public Task<List<DeviceData>> GetInitialData(Socket espSocket, int recvBuffer);
	}
}
