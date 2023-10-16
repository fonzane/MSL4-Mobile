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
		Shell.SetTabBarIsVisible(Application.Current.MainPage, false);
	}

    async void OnConnectToMSL(System.Object sender, System.EventArgs e)
    {
		Console.WriteLine(ip);
		string resp = await AuthService.GetSessionID(ip);
		Console.WriteLine(resp);
		if (resp == "false")
		{
			await DisplayAlert("Verbindungsfehler", "Verbindung konnte nicht hergestellt werden.", "OK");
		} else
		{
			Console.WriteLine("Verbindungsherstellung erfolgreich");
			await Shell.Current.GoToAsync("//infoView");
			//await Navigation.PushAsync(new Views.Home.HomeView());
		}
	}

	async void OnWebmonitorLogin(System.Object sender, System.EventArgs e)
	{
		bool loginResponse = await AuthService.LoginWebmonitor(email, password);
		if (loginResponse)
		{
			string ip = await DisplayPromptAsync("Login Erfolgreich", "MSL4 IP-Adresse eingeben", "OK");
			this.ip = ip;
			OnConnectToMSL(null, null);
    //        string resp = await AuthService.GetSessionID(ip);
    //        if (resp == "false")
    //        {
    //            await DisplayAlert("Verbindungsfehler", "Verbindung konnte nicht hergestellt werden.", "OK");
    //            return;
    //        }
    //        else
    //        {
				//Console.WriteLine("Connection successful" + resp);
    //            await Shell.Current.GoToAsync("//infoView");
    //        }
        }
	}

}