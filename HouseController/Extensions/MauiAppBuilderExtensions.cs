using HouseController.Services;
using HouseController.Shared;
using HouseController.ViewModels;
using HouseController.Views;

namespace HouseController.Extensions
{
	public static class MauiAppBuilderExtensions
	{
		public static void ConfigureHouseController(this MauiAppBuilder builder)
		{
			builder.Services.AddTransient<ConnectPage>();
			builder.Services.AddTransient<ControllerPage>();
			builder.Services.AddTransient<ConnectPageViewModel>();
			builder.Services.AddTransient<ControllerPageViewModel>();
			builder.Services.AddTransient<DeviceViewModel>();
			builder.Services.AddTransient<CardView>();
			builder.Services.AddSingleton<ICommunicationService, CommunicationService>();
			builder.Services.AddSingleton<INavigationService, NavigationService>();
			Routing.RegisterRoute(nameof(ControllerPage), typeof(ControllerPage));
		}
	}
}