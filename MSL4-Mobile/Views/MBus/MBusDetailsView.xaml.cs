using MSL4_Mobile.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace MSL4_Mobile.Views.MBus;

public partial class MBusDetailsView : ContentPage
{
	public int deviceID { get; set; }
	public MBusService mBusService { get; }

	public List<ComDataResponse.ComData> comData { get; set; }
	public List<DataStruct> baudData { get; set; }
	public List<DataStruct> indexData { get; set; }

	public ComDataResponse.ComData selectedCom { get; set; }
	public DataStruct selectedBaud { get; set; }
	public DataStruct selectedIndex { get; set; }
	public DataStruct selectedPeriodData { get; set; }

	public MBusDeviceDetails deviceDetails { get; set; }
	public List<MBusChannelData> channelData { get; set; }
	public UnitData unitData { get; set; }

	public ICommand OnSetMBusChannelData { get; private set; }

	public enum PortTypes
	{
		Serial = 1,
		TCP = 3
	};

	public MBusDetailsView(
		int deviceID,
		List<ComDataResponse.ComData> comData,
		List<DataStruct> baudData,
		ComDataResponse.ComData selectedCom,
		DataStruct selectedBaud)
	{
		this.comData = comData; this.baudData = baudData; this.selectedCom = selectedCom; this.selectedBaud = selectedBaud; this.deviceID = deviceID;
		OnSetMBusChannelData = new Command<int>((int channelID) => SetMBusChannelData(channelID));

		InitializeComponent();
		mBusService = new MBusService();
		OnPropertyChanged(nameof(mBusService));

		GetDeviceDetails(deviceID);
		GetMBusIndexData(deviceID);
		GetChannelData(deviceID);
	}

	private async void GetDeviceDetails(int deviceID)
	{
		deviceDetails = await mBusService.GetMBusDeviceDetails(AuthService.ipaddress, AuthService.sessionid, deviceID);
		Console.WriteLine("PortType: " + deviceDetails.pPortType);
		Console.WriteLine("PortName: " + deviceDetails.pPortName);
        OnPropertyChanged(nameof(deviceDetails));
	}

	private async void GetMBusIndexData(int deviceID)
	{
		MBusIndexDataResponse response = await mBusService.GetMBusIndexData(AuthService.ipaddress, AuthService.sessionid, deviceID);
		if (response != null)
		{
			indexData = response.items;
			OnPropertyChanged(nameof(indexData));
			Console.WriteLine("Fetched DeviceIndexData...");
		}
		else
		{
			Console.WriteLine("Error fetching MBusIndexData (response is null)...");
		}
	}

	private async void GetChannelData(int deviceID)
	{
		channelData = await mBusService.GetMBusChannelData(AuthService.ipaddress, AuthService.sessionid, deviceID);
		OnPropertyChanged(nameof(channelData));
        GetUnitData();
    }

    private async void GetUnitData()
	{
		unitData = await mBusService.GetUnitData(AuthService.ipaddress, AuthService.sessionid);
		foreach(MBusChannelData channel in channelData)
		{
			channel.unitData = unitData;
		}
		OnPropertyChanged(nameof(channelData));
	}

    async void OnSetMBusDeviceDetails(System.Object sender, System.EventArgs e)
	{
		deviceDetails.pDBAction = "TYPE_UPDATE";
		deviceDetails.pSessionID = AuthService.sessionid;
		deviceDetails.pBaud = selectedBaud.id;
		deviceDetails.pPeriod = selectedPeriodData.id;
		deviceDetails.pPortName = selectedCom.id;
		deviceDetails.pPosition = selectedIndex.id;
		Console.WriteLine("BaudRate " + selectedBaud.id + " - " + deviceDetails.pBaud);
		Console.WriteLine("PeriodData " + selectedPeriodData.id + " - " + deviceDetails.pPeriod);
        Console.WriteLine("Com " + selectedCom.id + " - " + deviceDetails.pPortName);
		Console.WriteLine("Index " + selectedIndex.id + " - " + deviceDetails.pPosition);
		Console.WriteLine(deviceDetails.pDBDeviceID);
		MBusDeviceDetails response = await mBusService.SetMBusDeviceDetails(AuthService.ipaddress, AuthService.sessionid, deviceDetails);
		if (response != null)
		{
			Console.WriteLine("DeviceDetails Set...");
			foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(response))
			{
				string name = descriptor.Name;
				object value = descriptor.GetValue(response);
				Console.WriteLine("{0}={1}", name, value);
			}
		}
		else
		{
			Console.WriteLine("Error fetching MBusIndexData (response is null)...");
		}
	}

	private void SetMBusChannelData(int channelID)
	{
		MBusChannelData selectedChannel = channelData.FirstOrDefault(c => c.id == channelID);
		mBusService.SetMBusChannelData(AuthService.ipaddress, AuthService.sessionid, channelID, selectedChannel);
		GetChannelData(this.deviceID);
	}
}
