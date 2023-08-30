using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Meter;

public partial class MeterView : ContentPage
{
	private MeterService meterService;

	public UnitData unitData { get; set; }
	public List<Services.Meter> meters { get; set; }

	public MeterView()
	{
		meterService = new MeterService();
		GetMeterData();
		InitializeComponent();
	}

	private async void GetMeterData()
	{
		meters = await meterService.GetMeterData(AuthService.ipaddress, AuthService.sessionid);
		//OnPropertyChanged(nameof(meters));
		GetUnitData();
	}

	private async void GetUnitData()
	{
		unitData = await meterService.GetUnitData(AuthService.ipaddress, AuthService.sessionid);
		foreach(Services.Meter meter in meters)
		{
			meter.unitData = unitData;
		}
		OnPropertyChanged(nameof(meters));
	}

    void OnSetMeterData(object sender, EventArgs e)
    {
		int id = (int)(sender as Button).CommandParameter;
		Services.Meter meter = meters.Find(m => m.id == id);
		meterService.SetMeterData(AuthService.ipaddress, AuthService.sessionid, id.ToString(), meter);
        //var selectedGPIO = (sender as Button).CommandParameter as Services.GPIO;
        //selectedGPIO.pStartType = startTypes.Find(st => st.Key == selectedGPIO.initialStartType.Key).Key;
        //if (selectedDigitalMode != null) selectedGPIO.pDigitalModus = selectedDigitalMode;
        //if (selectedgpioType != null) selectedGPIO.pGPIOType = selectedgpioType;
        //if (selectedGPIO.initialVisualType != null) selectedGPIO.pDBVisualTypeID = dbVisualTypeIDs[dbVisualTypeOptions.FindIndex(vo => vo == selectedGPIO.initialVisualType)];
        //gPIOService.SetGPIOData(AuthService.ipaddress, AuthService.sessionid, selectedGPIO.id.ToString(), selectedGPIO);
    }

    void OnReadMeterData(System.Object sender, System.EventArgs e)
    {
		GetMeterData();
    }
}
