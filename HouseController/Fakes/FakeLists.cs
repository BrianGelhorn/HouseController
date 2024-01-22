using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Fakes
{
	class FakeLists
	{
		public List<string> IpList { get; } = ["192.168.0.91", "192.168.0.93", "184.12.23.12", "192.168.0.3"];

		public class TimesStatus
		{
			public string? Time {get; set;}
			public bool? Status { get; set;}
		}
		public List<TimesStatus> TimesList { get; } = [new TimesStatus(){ Time="21:19", Status=true}, new TimesStatus() { Time = "21:19", Status = true }, new TimesStatus() { Time = "21:19", Status = true }, new TimesStatus() { Time = "21:19", Status = true }, new TimesStatus() { Time = "21:19", Status = true }, new TimesStatus() { Time = "21:19", Status = true }, new TimesStatus() { Time = "21:19", Status = true }, new TimesStatus() { Time = "12:31", Status = true }, new TimesStatus() { Time = "12:31", Status = true }, new TimesStatus() { Time = "12:31", Status = true }, new TimesStatus() { Time = "12:31", Status = true }, new TimesStatus() { Time = "12:31", Status = false }, new TimesStatus() { Time = "12:31", Status = false }, new TimesStatus() { Time = "14:50", Status = true }];
	}
}
