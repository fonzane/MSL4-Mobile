using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.GPIO;

public partial class GPIOView : ContentPage
{
	private GPIOService gPIOService;

	public List<string> digitalModes;
	public StartTypeResponse startTypes;

	public List<MSL4_Mobile.Services.GPIO> gPIOData { get; set; }

	public GPIOView()
	{
		gPIOService = new GPIOService();
		GetDigitalModes();
		GetGPIOData();
		InitializeComponent();
	}

	private async void GetDigitalModes()
	{
		digitalModes = await gPIOService.GetDigitalModes(AuthService.ipaddress, AuthService.sessionid);
		startTypes = await gPIOService.GetStartTypes(AuthService.ipaddress, AuthService.sessionid);
	}

	private async void GetGPIOData()
	{
		gPIOData = await gPIOService.GetGPIOData(AuthService.ipaddress, AuthService.sessionid);
		OnPropertyChanged(nameof(gPIOData));
	}
}
