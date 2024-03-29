using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using HouseController.Extensions;
using HouseController.Shared;
using HouseController.ViewModels;
using Newtonsoft.Json;
using DeviceInfo = HouseController.Models.DeviceInfo;

namespace HouseController.Services
{
	public class CommunicationService : ICommunicationService
    {
        public NetworkStream? EspNetworkStream { get; set; }

		private readonly SemaphoreSlim _deviceSendingSemaphore = new(1, 1);

		private const int ESPPORT = 2500;

		public void SetNetworkStream(NetworkStream espNetworkStream)
		{
			EspNetworkStream = espNetworkStream;
		}

		public NetworkStream? GetNetworkStream()
		{
			return EspNetworkStream;
		}

        public void ClearNetworkStream()
        {
            if (EspNetworkStream == null)
            {
                return;
            }
			EspNetworkStream.Close();
            EspNetworkStream = null;
        }

		public async Task<bool> ConnectToDeviceAsync(
			string ip,
			int port,
			CancellationToken cancellationToken = new()
		)
		{
			var tcpClient = new TcpClient();
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				await tcpClient.ConnectAsync(ip, port, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
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
            if (EspNetworkStream == null)
                return [];
			await EspNetworkStream.WriteAsync("InDt".EncodeMessage());
			while (!EspNetworkStream.DataAvailable)
			{
				//Delay not to block the MainThread
				cancellationToken.ThrowIfCancellationRequested();
				await Task.Delay(50);
			}

			var buffer = await EspNetworkStream.ReadAsync(receivedData, cancellationToken);
			var receivedString = receivedData.DecodeMessage(buffer);
			Debug.WriteLine($"Received Initial Data: {receivedString}");
			var jsonObject = JsonConvert.DeserializeObject<ObservableCollection<DeviceInfo>>(
				receivedString
			);
			return jsonObject ?? [];
		}

		public async Task SendDeviceChangeAsync(int id, string dataType, params string[] dataValue)
		{
			await _deviceSendingSemaphore.WaitAsync();
			try
            {
                var stringToSend = $"Update;{id};{dataType};";
                if (dataValue.Length > 1)
                {
                    foreach (var value in dataValue)
                    {
                        stringToSend += ($"{value}-");
                    }
                    stringToSend = stringToSend.Remove(stringToSend.Length-1);
                }
                else
                {
                    stringToSend += dataValue.First();
                }
				var dataToSend = stringToSend.EncodeMessage();
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
			EspNetworkStream.Dispose();
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
				if(EspNetworkStream == null)
					break;
                if (EspNetworkStream.DataAvailable)
                {
                    var buffer = new byte[bufferSize];
                    var bytesRead = await EspNetworkStream.ReadAsync(buffer, cancellationToken);
                    var data = buffer.DecodeMessage(bytesRead);

                    if (data.StartsWith("Updated"))
                    {
                        var dataParsed = data.Split(";");
                        if (int.TryParse(dataParsed[1], out var updatedId) == false)
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
                await Task.Delay(50);
            }
		}

		//TODO: Apply good practices
        public async Task<string> CheckDevice(string ip, int timeout)
        {
            var checkerTcpClient = new UdpClient();
			var timeoutCancellationTokenSource = new CancellationTokenSource();
			timeoutCancellationTokenSource.CancelAfter(timeout);
			try
			{
				checkerTcpClient.Send("1".EncodeMessage(), ip, ESPPORT);
				var receivedData = await checkerTcpClient.ReceiveAsync(timeoutCancellationTokenSource.Token);
				if (receivedData.Buffer.DecodeMessage(1) == "1")
				{
					return ip;
				}
			}
			catch(Exception e) 
			{
				//Debug.WriteLine(e.Message);
			}
            return "";
        }
    }
}
