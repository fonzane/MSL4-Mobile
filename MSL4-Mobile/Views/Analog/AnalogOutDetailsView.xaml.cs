using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Analog;

public partial class AnalogOutDetailsView : ContentPage
{
	private ChannelService channelService;
    public AnalogOutput analogOutput { get; set; }
    public List<string> units { get; set; }
    public KeyValuePair<string, AnalogType> channelType { get; set; }

    public List<KeyValuePair<string, AnalogType>> AnalogTypes { get; } = new List<KeyValuePair<string, AnalogType>>()
    {
        new ("0-10 V", AnalogType.TYPE_0_10_V),
        new ("0,4-2 V", AnalogType.TYPE_0K4_2_V),
        new ("0-20 mA", AnalogType.TYPE_0_20_MA),
        new ("4-20 mA", AnalogType.TYPE_4_20_MA),
    };

    public AnalogOutDetailsView(AnalogOutput analogOutput)
	{
        channelService = new ChannelService();
        Console.WriteLine("Initializing Analog Output Details View");
        this.analogOutput = analogOutput;
        channelType = AnalogTypes.FirstOrDefault(t => (int)t.Value == analogOutput.pAnalogType);
        GetUnits();
		InitializeComponent();
	}

    private async void GetUnits()
    {
        units = await channelService.GetUnits(AuthService.ipaddress, AuthService.sessionid);
        OnPropertyChanged(nameof(units));
    }

    void OnChangeAnalogType(System.Object sender, System.EventArgs e)
    {
        analogOutput.pAnalogType = (int)channelType.Value;
    }

    async void OnSetChannelData(System.Object sender, System.EventArgs e)
    {
        bool response = await channelService.SetAnalogOutput(AuthService.ipaddress, AuthService.sessionid, analogOutput);
        if (response)
        {
            // ToDo: Need to find a way to refresh AnalogView after setting channel data
            await Navigation.PopToRootAsync();
            MessagingCenter.Send<AnalogOutDetailsView>(this, "updated");
        }
        else
        {
            Console.WriteLine("Error setting channel data.");
        }
    }
}
