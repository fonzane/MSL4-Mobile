using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class MainPage : ContentPage
{

	public string ip { get; set; }
	public string email { get; set; }
	public string password { get; set; }


	public MainPage()
	{
#if MACCATALYST
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping("Inspect", (handler, view) =>
        {
            if (OperatingSystem.IsMacCatalystVersionAtLeast(16, 4))
                handler.PlatformView.Inspectable = true;
        });
#endif
        InitializeComponent();
		if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
		{
			email = "tschaffert@fw-systeme.de";
			password = "admin";
			OnWebmonitorLogin(null, null);
		}
		else if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
		{
			CheckLoggedIn();
		}
	}

 //   async Task<bool> OnConnectToMSL(System.Object sender, System.EventArgs e)
 //   {
	//	return true;

 //       string ip = await DisplayPromptAsync("Login Erfolgreich", "MSL4 IP-Adresse eingeben", "OK");
 //       this.ip = ip;
 //       string resp = await AuthService.GetSessionID(ip);
	//	Console.WriteLine(resp);
	//	if (resp == "false")
	//	{
	//		await DisplayAlert("Verbindungsfehler", "Verbindung konnte nicht hergestellt werden.", "OK");
	//		return false;
	//	} else
	//	{
	//		Console.WriteLine("Verbindungsherstellung erfolgreich");
	//		await Shell.Current.GoToAsync("//connection");
	//		MessagingCenter.Send<MainPage>(this, "isConnected");
	//		return true;
	//	}
	//}

	private async void OnWebmonitorLogin(object sender, EventArgs args)
	{
		bool loginResponse = await AuthService.LoginWebmonitor(email, password);
		if (loginResponse)
		{
            MessagingCenter.Send<MainPage>(this, "isLoggedIn");
			await Shell.Current.GoToAsync("//connection");
        } 
	}

	private async void CheckLoggedIn()
	{
		await Task.Delay(500);
		bool isLoggedIn = await AuthService.CheckLoggedIn();
		Console.WriteLine("CheckLoggedIn Response: " + isLoggedIn.ToString());
		if (isLoggedIn)
		{
			MessagingCenter.Send<MainPage>(this, "isLoggedIn");
            await Shell.Current.GoToAsync("//connection");
        }
	}
}