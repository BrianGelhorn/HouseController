using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Services
{
	internal class NavigationService : INavigationService
	{
		public Task GoToAsync(string uri)
		{
			return MainThread.InvokeOnMainThreadAsync(async() =>
			{
				await Shell.Current.GoToAsync(uri);
			});
		}

		public Task GoToAsync(string uri, IDictionary<string, object> parameters)
		{
			return MainThread.InvokeOnMainThreadAsync(async() =>
			{
				await Shell.Current.GoToAsync(uri, parameters);
			});
		}
	}
}
