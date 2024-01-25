using CommunityToolkit.Maui.Views;
using HouseController.ViewModels;

namespace HouseController.Views;

public partial class LoadingView : ContentView
{
	public bool IsLoading { get; set; } = true;
	public bool IsEmptyList { get; set; } = false;

	public LoadingView()
	{
		InitializeComponent();
		BindingContext = this;
		Task.Run(() =>
		{
			Task.Delay(8000).Wait();
			RaiseIsLoadingChanged();
		});
	}

	public void RaiseIsLoadingChanged()
	{
		IsLoading = !IsLoading;
		IsEmptyList = !IsEmptyList;
		OnPropertyChanged(nameof(IsLoading));
		OnPropertyChanged(nameof(IsEmptyList));
	}
}