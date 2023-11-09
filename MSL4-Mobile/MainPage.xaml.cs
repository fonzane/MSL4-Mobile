using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class MainPage : ContentPage
{

	public string ip { get; set; }
	public string email { get; set; }
	public string password { get; set; }

	public MainPage()
	{
		InitializeComponent();
		CheckLoggedIn();
	}

    async Task<bool> OnConnectToMSL(System.Object sender, System.EventArgs e)
    {
        string ip = await DisplayPromptAsync("Login Erfolgreich", "MSL4 IP-Adresse eingeben", "OK");
        this.ip = ip;
        string resp = await AuthService.GetSessionID(ip);
		Console.WriteLine(resp);
		if (resp == "false")
		{
			await DisplayAlert("Verbindungsfehler", "Verbindung konnte nicht hergestellt werden.", "OK");
			return false;
		} else
		{
			Console.WriteLine("Verbindungsherstellung erfolgreich");
			await Shell.Current.GoToAsync("//infoView");
			MessagingCenter.Send<MainPage>(this, "isConnected");
			return true;
		}
	}

	private async void OnWebmonitorLogin(object sender, EventArgs args)
	{
		Console.WriteLine("TEst");
		bool loginResponse = await AuthService.LoginWebmonitor(email, password);
		if (loginResponse)
		{
			bool success = await OnConnectToMSL(null, null);
			while (!success)
			{
				success = await OnConnectToMSL(null, null);
			}
        } else
		{
			return;
		}
	}

	private async void CheckLoggedIn()
	{
		bool isLoggedIn = await AuthService.CheckLoggedIn();
		if (isLoggedIn)
		{
			OnConnectToMSL(null, null);
		}
	}

}