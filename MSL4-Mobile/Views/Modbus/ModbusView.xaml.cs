using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Modbus;

public partial class ModbusView : ContentPage
{
	private ModbusService modbusService;
	public List<ModbusDevice> modbusDevices { get; set; }

	public ModbusView()
	{
		modbusService = new ModbusService();
		GetModbusDevices();
		InitializeComponent();
	}

	private async void GetModbusDevices()
	{
		Console.WriteLine("Getting Modbus Devices");
		ModbusResponse modbusResponse = await modbusService.GetModbusDevices(AuthService.ipaddress, AuthService.sessionid);
		if (modbusResponse != null)
		{
			modbusDevices = modbusResponse.items;
			OnPropertyChanged(nameof(modbusDevices));
		}
		else
		{
			await DisplayAlert("Fehler bei der Datenübertragung.", "Modbus Geräte konnten nicht geladen werden.", "OK");
		}
	}

    async void OnSelectModbusDevice(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        ModbusDevice modbusDevice = e.CurrentSelection.FirstOrDefault() as ModbusDevice;

		await Navigation.PushAsync(new Modbus.ModbusDetailsView(modbusDevice.id, modbusService));
    }
}
