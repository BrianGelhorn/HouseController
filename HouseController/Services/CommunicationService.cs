using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using HouseController.Extensions;
using HouseController.Models;
using HouseController.Shared;
using HouseController.ViewModels;
using HouseController.Views;
using HouseController.Views.PopUps;
using Newtonsoft.Json;
using DeviceInfo = HouseController.Models.DeviceInfo;

namespace HouseController.Services
{
	public class CommunicationService : ICommunicationService
	{
		public NetworkStream EspNetworkStream { get; set; }

		public SemaphoreSlim _deviceSendingSemaphore = new SemaphoreSlim(1, 1);

		public void SetNetworkStream(NetworkStream espNetworkStream)
		{
			EspNetworkStream = espNetworkStream;
		}

		public NetworkStream GetNetworkStream()
		{
			return EspNetworkStream;
		}

		public async Task<bool> ConnectToDeviceAsync(
			string ip,
			int port,
			CancellationToken cancellationToken
		)
		{
			var tcpClient = new TcpClient();
			try
			{
				await tcpClient.ConnectAsync(ip, port, cancellationToken);
				if (tcpClient.Connected)
				{
					var networkStream = tcpClient.GetStream();
					ConnectedDeviceInfo.SetCurrentEspData(ip, ip, networkStream);
					SetNetworkStream(networkStream);
					return true;
				}
				throw new Exception("Connecting Failed");
			}
			catch
			{
				return false;
			}
		}

		public async Task<ObservableCollection<DeviceInfo>> GetInitialDataAsync(
			int bufferSize,
			CancellationToken cancellationToken
		)
		{
			var receivedData = new byte[bufferSize];
			await EspNetworkStream.WriteAsync("InDt".EncodeMessage());
			while (!EspNetworkStream.DataAvailable)
			{
				//Delay not to block the MainThread
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Delay(50);
			}

			var buffer = await EspNetworkStream.ReadAsync(receivedData);
			var receivedString = receivedData.DecodeMessage(buffer);
			Debug.WriteLine(string.Format("Received Initial Data: {0}", receivedString));
			var jsonObject = JsonConvert.DeserializeObject<ObservableCollection<DeviceInfo>>(
				receivedString
			);
			return jsonObject ?? [];
		}

		public async Task SendDeviceChangeAsync(int id, string dataType, string dataValue)
		{
			await _deviceSendingSemaphore.WaitAsync();
			try
			{
				var dataToSend = (
					string.Format("Update;{0};{1};{2}", id.ToString(), dataType, dataValue)
				).EncodeMessage();
				await EspNetworkStream.WriteAsync(dataToSend);
			}
			finally
			{
				_deviceSendingSemaphore.Release();
			}
		}

		public DeviceInfo CreateDeviceInfo(DeviceViewModel deviceViewModel)
		{
			return new DeviceInfo(
				deviceViewModel.Id,
				deviceViewModel.Name,
				deviceViewModel.Status,
				deviceViewModel.TimeInfoList.ToList()
			);
		}

		public void DisconnectFromDevice()
		{
			EspNetworkStream.Close();
			_deviceSendingSemaphore.Release();
		}

		public async Task StartListeningForUpdateAsync(
			int bufferSize,
			CancellationToken cancellationToken
		)
		{
			var deviceDataList = ConnectedDeviceInfo.DeviceDataList;
			//Endless Loop to keep checking all the time if new information was received
			while (true)
			{
				if (EspNetworkStream.DataAvailable)
				{
					var buffer = new byte[bufferSize];
					var bytesRead = await EspNetworkStream.ReadAsync(buffer, cancellationToken);
					var data = buffer.DecodeMessage(bytesRead);

					if (data.StartsWith("Updated"))
					{
						var dataParsed = data.Split(";");
						int updatedId;
						if (int.TryParse(dataParsed[1], out updatedId) == false)
						{
							//Data error
						}
						var updatedDataType = dataParsed[2];
						var updatedDataValue = dataParsed[3];

						foreach (var device in deviceDataList)
						{
							if (device.Id != updatedId)
								continue;
							MainThread.BeginInvokeOnMainThread(() =>
							{
                                device.UpdateDeviceView(updatedDataValue, updatedDataType);
                            });	
						}
					}
				}
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Delay(50);
			}
		}
	}
}
