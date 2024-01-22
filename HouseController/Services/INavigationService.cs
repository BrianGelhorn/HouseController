using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Services
{
	public interface INavigationService
	{
		Task GoToAsync(string uri);
		Task GoToAsync(string uri, IDictionary<string, object> parameters);
	}
}
