using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.MBus;

public partial class MBusDetailsView : ContentPage
{
	private MBusService mBusService;

	public MBusDeviceDetails deviceDetails { get; set; }

	public MBusDetailsView(int deviceID)
	{
		InitializeComponent();
		mBusService = new MBusService();
		GetDeviceDetails(deviceID);
	}

	private async void GetDeviceDetails(int deviceID)
	{
		deviceDetails = await mBusService.GetMBusDeviceDetails(AuthService.ipaddress, AuthService.sessionid, deviceID);
		OnPropertyChanged(nameof(deviceDetails));
	}
}
