using HouseController.Extensions;
using HouseController.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Services
{
	public class CommunicationService : ICommunicationService
	{
		public async Task<ObservableCollection<DeviceData>> GetInitialData(Socket espSocket, int recvBuffer)
		{
			var receivedData = new byte[recvBuffer];
			await espSocket.SendAsync("InDt".EncodeMessage());
			Task.Delay(500).Wait();
			var buffer = await espSocket.ReceiveAsync(receivedData);
			var jsonObject =
				JsonConvert.DeserializeObject<ObservableCollection<DeviceData>>(receivedData.DecodeMessage(buffer));
			return jsonObject ?? [];
		}

		public async Task<bool> SendDeviceChange(Socket espSocket, DeviceInformation device)
		{
			throw new NotImplementedException();
		}
	}
}