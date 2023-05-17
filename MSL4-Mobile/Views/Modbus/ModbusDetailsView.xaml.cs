using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Modbus;

public partial class ModbusDetailsView : ContentPage
{
	private ModbusService modbusService;
	public ModbusDeviceDetails modbusDevice { get; set; }

	public ModbusDetailsView(int deviceID, ModbusService modbusService)
	{
		Console.WriteLine("Modbus Device Details View");
		this.modbusService = modbusService;
		GetModbusDetails(deviceID);
		InitializeComponent();
	}

	private async void GetModbusDetails(int deviceID)
	{
		Console.WriteLine("Gettind Modbus Device Details");
		modbusDevice = await modbusService.GetModbusDeviceDetails(AuthService.ipaddress, AuthService.sessionid, deviceID);
		if (modbusDevice != null)
		{
			OnPropertyChanged(nameof(modbusDevice));
			Console.WriteLine(modbusDevice.pName);
		}
		else
		{
			await DisplayAlert("Fehler bei der Datenübertragung", "Modbus Gerät konnte nicht geladen werden.", "OK");
		}
	}
}
