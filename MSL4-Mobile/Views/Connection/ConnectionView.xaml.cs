using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Connection;

public partial class ConnectionView : ContentPage
{
    private DeviceService deviceService;
    public List<Services.Device> devices { get; set; }
    public string ip { get; set; }

	public ConnectionView()
	{
		deviceService = new DeviceService();
		GetDevices();
		InitializeComponent();
	}

	private async void GetDevices()
	{
        deviceService = new DeviceService();
        devices = await deviceService.GetDevices();

        foreach (Services.Device device in devices)
        {
            Console.WriteLine(device.devicename + " -- " + device.hostname);
        }
		OnPropertyChanged(nameof(devices));
    }

    async void OnConnectToMSL(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e != null)
        {
            ip = (e.CurrentSelection.FirstOrDefault() as Services.Device).hostname;
        }
        string resp = await AuthService.GetSessionID(ip);
        Console.WriteLine(resp);
        if (resp == "false")
        {
            await DisplayAlert("Verbindungsfehler", "Verbindung konnte nicht hergestellt werden.", "OK");
        }
        else
        {
            MessagingCenter.Send<ConnectionView>(this, "isConnected");
            Console.WriteLine("Verbindungsherstellung erfolgreich");
            await Shell.Current.GoToAsync("//infoView");
        }
    }

    void OnMSLConnect(System.Object sender, System.EventArgs e)
    {
        OnConnectToMSL(null, null);
    }
}
