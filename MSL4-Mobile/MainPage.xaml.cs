using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class MainPage : ContentPage
{
	public static readonly BindableProperty greetingTextProperty = BindableProperty.Create("GreetingText", typeof(string), typeof(MainPage), "Hello World");
	public string GreetingText { get; set; } = "Hello Worlddd";

	public MainPage()
	{
		InitializeComponent();
		InitializeMSL4Connection();
	}

	private async void InitializeMSL4Connection()
	{
		// ToDo: Give user the option to set the ip adress manually.
		//string ip = await DisplayPromptAsync("Verbindung herstellen", "Geben Sie die IP-Adresse des Gerätes an.");
		string ip = "192.168.20.65";
		string sessionid = await AuthService.GetSessionID(ip);
		Console.WriteLine("MainPage sessionid: " + sessionid);
		GreetingText = "You are logged in.";
		// ToDo: Do something if no sessionid.
    }
}