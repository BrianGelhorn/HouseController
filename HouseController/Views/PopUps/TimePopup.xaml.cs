using System.Globalization;
using CommunityToolkit.Maui.Views;
using HouseController.ViewModels;

namespace HouseController.Views.PopUps;

public partial class TimePopup : Popup
{
	public int ButtonStatus { get; set; }
	public TimeSpan TimeStatus { get; set; }
    public TimeInfo TimeInfo { get; set; }
	public TimePopup(TimeInfo timeInfo)
	{
        ButtonStatus = timeInfo.TimeStatus;

        //TODO: Manage exception when couldn't parse
        TimeSpan.TryParse(timeInfo.Time, out var timeStatus);
        TimeStatus = timeStatus;
        InitializeComponent();
    }

    protected override Task OnDismissedByTappingOutsideOfPopup(CancellationToken token = new CancellationToken())
    {
        TimeInfo = new TimeInfo(TimeStatus.ToString("hh\\:mm"), ButtonStatus);
        return base.OnDismissedByTappingOutsideOfPopup(token);
    }

    private void OnTimeStatusClicked(object? sender, EventArgs e)
    {
        ButtonStatus = ButtonStatus == 0? 1 : 0;
        OnPropertyChanged(nameof(ButtonStatus));
    }
}