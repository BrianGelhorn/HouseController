using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Services
{
	public interface IDeviceDiscoverService
	{
		public void StopConnection(Socket socket);
	}
}
