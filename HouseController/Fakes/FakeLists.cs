﻿using System.Collections.ObjectModel;
using HouseController.ViewModels;
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
			new TimeInfo("21:19", 1),
            new TimeInfo("21:13", 0),
            new TimeInfo("12:19", 0),
            new TimeInfo("11:19", 1),
            new TimeInfo("23:19", 1),
            new TimeInfo("00:00", 1),
            new TimeInfo("21:19", 0),
            new TimeInfo("21:15", 1),
            new TimeInfo("21:19", 1)
		];

		//public ObservableCollection<DeviceInfo> DeviceDataList { get; } =
		//	[new DeviceInfo { Name = "ASDASD", Status = false, TimeInfoList = TimesList.ToList(), Id = 0}];
	}
}