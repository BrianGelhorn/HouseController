using HouseController.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace HouseController.Services
{
	public interface ICommunicationService
	{
		public Task<bool> SendDeviceChange(Socket espSocket, DeviceInformation device);
		public Task<ObservableCollection<DeviceData>> GetInitialData(Socket espSocket, int recvBuffer);
	}
}