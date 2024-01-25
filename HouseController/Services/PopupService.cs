using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseController.Views.PopUps;

namespace HouseController.Services
{
	public class PopupService : IPopupService
	{
		public void ClosePopup(Popup popup)
		{
			popup.Close();
		}

		public void ShowPopup(Popup popup)
		{
			Application.Current?.MainPage?.ShowPopup(popup);
		}
	}
}
