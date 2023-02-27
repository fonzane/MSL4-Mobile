using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Analog;

public partial class AnalogDetailsView : ContentPage
{
	public AnalogInput analogInput { get; set; }

	public List<string> Units { get; } = new List<string>()
    {
        "A", "Bar", "°C", "K", "d", "h", "Hz",
        "kg", "kg/h", "kvar", "kvarh", "kVA",
        "kVAh", "kW/m²", "kW", "kWh", "I",
        "I/h", "kPa", "m³", "m³/h", "MByte",
        "min", "mA", "mK", "Nm³", "Nm³/h",
        "No", "on/off", "Pa", "ppm", "s",
        "Stck", "V", "%"
    };

    public List<KeyValuePair<string, AnalogType>> AnalogTypes { get; } = new List<KeyValuePair<string, AnalogType>>()
    {
        new ("0-10 V", AnalogType.TYPE_0_10_V),
        new ("0,4-2 V", AnalogType.TYPE_0K4_2_V),
        new ("0-20 mA", AnalogType.TYPE_0_20_MA),
        new ("4-20 mA", AnalogType.TYPE_4_20_MA),
        new ("NTC 5K", AnalogType.TYPE_NTC_5_K)
    };

    public KeyValuePair<string, AnalogType> channelType { get; set; }

    public AnalogDetailsView(AnalogInput analogInput)
	{
		this.analogInput = analogInput;
        channelType = AnalogTypes.FirstOrDefault(t => (int)t.Value == analogInput.pAnalogType);
		InitializeComponent();
    }

    async void OnSetChannelData(System.Object sender, System.EventArgs e)
    {
        bool response = await ChannelService.SetAnalogInput(AuthService.ipaddress, AuthService.sessionid, analogInput);
        if (response)
        {
            // ToDo: Need to find a way to refresh AnalogView after setting channel data
            await Navigation.PopToRootAsync();
            MessagingCenter.Send<AnalogDetailsView>(this, "updated");
        }
        else
        {
            Console.WriteLine("Error setting channel data.");
        }
    }

    void OnChangeAnalogType(System.Object sender, System.EventArgs e)
    {
        analogInput.pAnalogType = (int)channelType.Value;
    }
}
