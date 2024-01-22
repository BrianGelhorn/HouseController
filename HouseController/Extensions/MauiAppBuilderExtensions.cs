using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using HouseController.Services;
using HouseController.ViewModels;
using HouseController.Views;
using HouseController.Views.PopUps;

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
			builder.Services.AddSingleton<INavigationService, NavigationService>();

			Routing.RegisterRoute(nameof(ControllerPage), typeof(ControllerPage));
		}
	}
}
