using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class MainPage : ContentPage
{

	public string ip { get; set; }

	public MainPage()
	{
		InitializeComponent();
	}

    async void OnConnectToMSL(System.Object sender, System.EventArgs e)
    {
		Console.WriteLine(ip);
		await AuthService.GetSessionID(ip);
		await Shell.Current.GoToAsync("//infoView");
		//Navigation.PushAsync(new Views.Home.HomeView());
	}

}