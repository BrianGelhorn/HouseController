using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseController.Services
{
	public interface IPopupService
	{
		public void ShowPopup(Popup popup, bool canBeDismissedByTappingOutsideOfPopup);
		public void ClosePopup(Popup popup);
	}
}
