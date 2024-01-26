using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseController.Models;
namespace HouseController.Fakes
{
	class FakeLists
	{
		public List<string> IpList { get; } =
			["192.168.0.91", "192.168.0.93", "184.12.23.12", "192.168.0.3"];

		public class TimesStatus
		{
			public string? Time { get; set; }
			public bool? Status { get; set; }
		}

		public static ObservableCollection<TimeInfo> TimesList { get; } =
		[
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "21:19", TimeStatus = true },
			new TimeInfo() { Time = "12:31", TimeStatus = true },
			new TimeInfo() { Time = "12:31", TimeStatus = true },
			new TimeInfo() { Time = "12:31", TimeStatus = true },
			new TimeInfo() { Time = "12:31", TimeStatus = true },
			new TimeInfo() { Time = "12:31", TimeStatus = false },
			new TimeInfo() { Time = "12:31", TimeStatus = false },
			new TimeInfo() { Time = "14:50", TimeStatus = true }
		];

		//public ObservableCollection<DeviceInfo> DeviceDataList { get; } =
		//	[new DeviceInfo { Name = "ASDASD", Status = false, TimeInfoList = TimesList.ToList(), Id = 0}];
	}
}