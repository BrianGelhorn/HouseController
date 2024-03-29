using CommunityToolkit.Maui.Views;

namespace HouseController.Extensions
{
    public static class PopupExtensions
    {
        private static Popup? _currentPopup;

        private static void SetCurrentPopup(Popup popup)
        {
            _currentPopup = popup;
        }

        private static void ClearCurrentPopup()
        {
            _currentPopup = null;
        }

        public static async Task ShowAsPopup(
            this Popup popup,
            bool canBeDismissedByTappingOutsideOfPopup = true
        )
        {
            if (_currentPopup != null)
            {
                await ClosePopupAsync(_currentPopup);
            }
            popup.CanBeDismissedByTappingOutsideOfPopup = canBeDismissedByTappingOutsideOfPopup;
            Application.Current?.MainPage?.ShowPopup(popup);
            SetCurrentPopup(popup);
        }

        public static async Task ShowAsPopupAndWaitAsync(
            this Popup popup,
            bool canBeDismissedByTappingOutsideOfPopup = true
        )
        {
            if (_currentPopup != null)
            {
                await ClosePopupAsync(_currentPopup);
            }
            popup.CanBeDismissedByTappingOutsideOfPopup = canBeDismissedByTappingOutsideOfPopup;
            SetCurrentPopup(popup);
            await Application.Current?.MainPage?.ShowPopupAsync(popup)!;
            ClearCurrentPopup();
        }

        public static void ClosePopup(this Popup popup)
        {
            ClearCurrentPopup();
            popup.Close();
        }

        public static Task ClosePopupAsync(this Popup popup)
        {
            if (_currentPopup != popup) return Task.CompletedTask; //Popup not active. Don't close anything
            ClearCurrentPopup();
            return popup.CloseAsync();
        }
    }
}
