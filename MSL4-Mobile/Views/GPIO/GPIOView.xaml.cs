using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.GPIO;

public partial class GPIOView : ContentPage
{
	public List<Services.GPIO> gPIOData { get; set; }

	private GPIOService gPIOService;

	public List<string> dbVisualTypeOptions { get; set; }
	public List<int> dbVisualTypeIDs { get; set; }
	public List<string> digitalModes { get; } = new List<string> { "NO", "NC" };
	public List<string> gpioTypes { get; } = new List<string> { "Input", "Output" };
	public List<KeyValuePair<int, string>> startTypes { get; } = new List<KeyValuePair<int, string>> {
		new KeyValuePair<int, string>(1, "Last Value"),
		new KeyValuePair<int, string>(2, "On"),
		new KeyValuePair<int, string>(3, "Off")
	};

	public string selectedDigitalMode { get; set; }
	public string selectedgpioType { get; set; }

	public GPIOView()
	{
		gPIOService = new GPIOService();
		GetGPIOData();
		InitializeComponent();
	}

	private async void GetGPIOData()
	{
		gPIOData = await gPIOService.GetGPIOData(AuthService.ipaddress, AuthService.sessionid);
		foreach(Services.GPIO gpio in gPIOData)
		{
			gpio.initialStartType = startTypes[gpio.pStartType - 1];
		}
		OnPropertyChanged(nameof(gPIOData));
		GetVisualTypes();
	}

	private async void GetVisualTypes()
	{
        DBDataType dBData = await gPIOService.GetVisualTypes(AuthService.ipaddress, AuthService.sessionid);
        dbVisualTypeOptions = dBData.options;
        dbVisualTypeIDs = dBData.values;
		foreach(Services.GPIO gpio in gPIOData)
		{
			gpio.initialVisualType = dbVisualTypeOptions[dbVisualTypeIDs.FindIndex(vid => vid == gpio.pDBVisualTypeID)];
			Console.WriteLine("Bruh " + gpio.initialVisualType);
		}
    }

    void OnSetGPIOData(object sender, EventArgs e)
    {
		var selectedGPIO = (sender as Button).CommandParameter as Services.GPIO;
		selectedGPIO.pStartType = startTypes.Find(st => st.Key == selectedGPIO.initialStartType.Key).Key;
		if (selectedDigitalMode != null) selectedGPIO.pDigitalModus = selectedDigitalMode;
		if (selectedgpioType != null) selectedGPIO.pGPIOType = selectedgpioType;
		if (selectedGPIO.initialVisualType != null) selectedGPIO.pDBVisualTypeID = dbVisualTypeIDs[dbVisualTypeOptions.FindIndex(vo => vo == selectedGPIO.initialVisualType)];
		gPIOService.SetGPIOData(AuthService.ipaddress, AuthService.sessionid, selectedGPIO.id.ToString(), selectedGPIO);
	}

    void OnReadGPIOData(System.Object sender, System.EventArgs e)
    {
		GetGPIOData();
    }
}