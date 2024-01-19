using HouseController.Services;
using HouseController.ViewModels;
using HouseController.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Extensions
{
	public static class MauiAppBuilderExtensions
	{
		public static void ConfigureHouseController(this MauiAppBuilder builder)
		{
			builder.Services.AddTransient<ConnectPage>();
			builder.Services.AddTransient<ConnectPageViewModel>();
			builder.Services.AddSingleton<ICommunicationService, CommunicationService>();
			builder.Services.AddSingleton<IDeviceDiscoverService, DeviceDiscoverService>();
		}
	}
}
