using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Connection;

public partial class ConnectionView : ContentPage
{
    private DeviceService deviceService;
    public List<Services.Device> devices { get; set; }
    public string ip { get; set; }
    private bool hasConnected = false;

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
        if (!hasConnected) hasConnected = true;
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
            // Get the current tab
            //var currentTab = Shell.Current.CurrentItem;

            //// Iterate through ShellContent pages in the current tab and reload them
            //foreach (var shellContent in currentTab.Items)
            //{
            //    if (shellContent is ShellSection content)
            //    {
            //        // Get the route of the page associated with the ShellContent
            //        var pageRoute = (string)content.Route;

            //        // Reload the page by navigating to the same route
            //        await Shell.Current.GoToAsync(pageRoute);
            //    }
            //}
            Console.WriteLine("Verbindungsherstellung erfolgreich");
            await Shell.Current.GoToAsync("//infoView");
        }
    }

    void OnMSLConnect(System.Object sender, System.EventArgs e)
    {
        OnConnectToMSL(null, null);
    }
}
