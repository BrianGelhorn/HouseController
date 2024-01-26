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
using CommunityToolkit.Maui.Core.Extensions;
using HouseController.ViewModels;
using DeviceInfo = HouseController.Models.DeviceInfo;

namespace HouseController.Services
{
	public class CommunicationService : ICommunicationService
	{
		public NetworkStream EspNetworkStream { get; set; }

		public async Task<ObservableCollection<DeviceInfo>> GetInitialData(int recvBuffer)
		{
			var receivedData = new byte[recvBuffer];
			await EspNetworkStream.WriteAsync("InDt".EncodeMessage());
			while (!EspNetworkStream.DataAvailable)
			{
				//Delay not to block the MainThread
				await Task.Delay(50);
			}
			var buffer = await EspNetworkStream.ReadAsync(receivedData);
			Debug.WriteLine(receivedData.DecodeMessage(buffer));
			var jsonObject =
				JsonConvert.DeserializeObject<ObservableCollection<DeviceInfo>>(
					receivedData.DecodeMessage(buffer));
			return jsonObject ?? [];
		}

		public async Task<bool> SendDeviceChange(DeviceInfo deviceInfo)
		{
			//DeviceViewModel deviceInfo = new DeviceViewModel(this);
			var data = (JsonConvert.SerializeObject(deviceInfo));
			await EspNetworkStream.WriteAsync(("Update" + data).EncodeMessage());
			byte[] buffer = new byte[16];
			var byteSize = await EspNetworkStream.ReadAsync(buffer);
			while (!EspNetworkStream.DataAvailable)
			{
				//Delay not to block the MainThread
				await Task.Delay(50);
			}
			byteSize = await EspNetworkStream.ReadAsync(buffer);
			var receivedData = buffer.DecodeMessage(byteSize);
			Debug.WriteLine(deviceInfo.Id);
			if (receivedData == ("Updated " + deviceInfo.Id))
			{
				return true;
			}
			return false;
		}

		public void SetCurrentSocket(NetworkStream espNetworkStream) => EspNetworkStream = espNetworkStream;

		public DeviceInfo CreateDeviceInfo(DeviceViewModel deviceViewModel)
		{
			return new DeviceInfo(deviceViewModel.Id, deviceViewModel.Name, deviceViewModel.Status,
				deviceViewModel.TimeInfoList.ToList());
		}
	}
}