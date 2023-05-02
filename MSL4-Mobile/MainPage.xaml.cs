using MSL4_Mobile.Services;

namespace MSL4_Mobile;

public partial class MainPage : ContentPage
{
	//public static readonly BindableProperty greetingTextProperty = BindableProperty.Create("GreetingText", typeof(string), typeof(MainPage), "Hello World");
	//public string GreetingText { get; set; } = "Hello Worlddd";

	public MSL4Data mSL4Data { get; set; }
	public List<PeriodDataResponse.PeriodData> PeriodData { get; set; }
	public PeriodDataResponse.PeriodData selectedPeriodData { get; set; }

	private MSL4Service _MSL4Service;

	public MainPage()
	{
		InitializeComponent();
		InitializeMSL4Connection();
		_MSL4Service = new MSL4Service();
	}

	private async void InitializeMSL4Connection()
	{
		// ToDo: Give user the option to set the ip adress manually.
		//string ip = await DisplayPromptAsync("Verbindung herstellen", "Geben Sie die IP-Adresse des Gerätes an.");
		//string ip = "msl4fw0189.fw-systeme.local";
		//string ip = "localhost:3333/?maui=192.168.20.54";
		string ip = "192.168.20.53";
		string sessionid = await AuthService.GetSessionID(ip);
		Console.WriteLine("MainPage sessionid: " + sessionid);
		//GreetingText = "You are logged in.";
		// ToDo: Do something if no sessionid.
		GetDeviceData();
		GetPeriodData();
    }

	private async void GetDeviceData()
	{
		Console.WriteLine("Getting MSL4 Device Data...");
		mSL4Data = await _MSL4Service.GetMSL4Data(AuthService.ipaddress, AuthService.sessionid);
		selectedPeriodData = _MSL4Service.PeriodData.FirstOrDefault(p => p.id == mSL4Data.pPeriod);
		Console.WriteLine("Selected Period: " + selectedPeriodData.pLabel);
		OnPropertyChanged(nameof(selectedPeriodData));
		OnPropertyChanged(nameof(mSL4Data));
		Console.WriteLine(mSL4Data.pName);
	}

	private async void GetPeriodData()
	{
		Console.WriteLine("Getting MSL4 PeriodData...");
		PeriodDataResponse mSL4PeriodData = await _MSL4Service.GetMSL4PeriodData(AuthService.ipaddress, AuthService.sessionid);
		PeriodData = mSL4PeriodData.items;
		OnPropertyChanged(nameof(PeriodData));
	}

	async void OnSetDevicedata(System.Object sender, System.EventArgs e)
    {
		Console.WriteLine("Setting MSL4Device Data...");
		mSL4Data.pSessionID = AuthService.sessionid;
		mSL4Data.pDBAction = "TYPE_UPDATE";
		mSL4Data.pPeriod = selectedPeriodData.id;
		MSL4Data mSL4DataResponse = await _MSL4Service.SetMSL4Data(AuthService.ipaddress, mSL4Data);
		if (mSL4DataResponse != null)
		{
            Console.WriteLine("Success Setting MSL4Device Data...");
            Console.WriteLine(mSL4DataResponse.pName);
			GetDeviceData();
			GetPeriodData();
		}
		else
		{
            Console.WriteLine("Error setting MSL4Device Data...");
        }
    }
}