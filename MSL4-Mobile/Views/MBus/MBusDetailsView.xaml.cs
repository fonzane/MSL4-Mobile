using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.MBus;

public partial class MBusDetailsView : ContentPage
{
	private MBusService mBusService;

	public List<ComDataResponse.ComData> comData { get; set; }
	public List<BaudDataResponse.BaudData> baudData { get; set; }
	public ComDataResponse.ComData selectedCom { get; set; }
	public BaudDataResponse.BaudData selectedBaud { get; set; }

	public MBusDeviceDetails deviceDetails { get; set; }

	public enum PortTypes
	{
		Serial = 1,
		TCP = 3
	};

	public MBusDetailsView(
		int deviceID,
		List<ComDataResponse.ComData> comData,
		List<BaudDataResponse.BaudData> baudData,
		ComDataResponse.ComData selectedCom,
		BaudDataResponse.BaudData selectedBaud)
	{
		this.comData = comData; this.baudData = baudData; this.selectedCom = selectedCom; this.selectedBaud = selectedBaud;
		Console.WriteLine("Selected Com: " + selectedCom.pLabel);
		InitializeComponent();
		mBusService = new MBusService();
		GetDeviceDetails(deviceID);
	}

	private async void GetDeviceDetails(int deviceID)
	{
		deviceDetails = await mBusService.GetMBusDeviceDetails(AuthService.ipaddress, AuthService.sessionid, deviceID);
		Console.WriteLine("PortType: " + deviceDetails.pPortType);
		Console.WriteLine("PortName: " + deviceDetails.pPortName);
        OnPropertyChanged(nameof(deviceDetails));
	}
}
