using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseController.Models;
using HouseController.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.ViewModels
{
	public partial class ConnectPageViewModel : ObservableObject
	{
		[ObservableProperty]
		ObservableCollection<DeviceInformation>? deviceInformationList;

		IDeviceDiscoverService deviceDiscoverService;
		ICommunicationService communicationService;

		public ConnectPageViewModel(IDeviceDiscoverService deviceDiscoverService, ICommunicationService communicationService)
		{
			this.deviceDiscoverService = deviceDiscoverService;
			this.communicationService = communicationService;
		}

		[RelayCommand]
		private async Task ConnectToDevice(string ip)
		{
			Debug.WriteLine("Conectando");
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 5000);
			try
			{
				await socket.ConnectAsync(ipEndPoint);
			}
			catch
			{

			}
			finally
			{
				Debug.WriteLine(socket.Connected);
			}
		}
	}
}
