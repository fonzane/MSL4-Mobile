using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Digital;

public partial class DigitalDetailsView : ContentPage
{
	public DigitalInput digitalInput { get; set; }

    public List<string> DigitalTypes { get; } = new List<string>()
    {
        "Impulszähler", "Störmeldung", "Zeitzähler", "Zustand", "Störung Eingang"
    };

    public string channelType { get; set; }

	public DigitalDetailsView(DigitalInput digitalInput)
	{
		this.digitalInput = digitalInput;
		channelType = DigitalTypes.FirstOrDefault(t => t == digitalInput.pDigitalType);
		InitializeComponent();
	}

    async void OnSetChannelData(System.Object sender, System.EventArgs e)
    {
        Console.WriteLine(digitalInput.pDigitalType);
        bool response = await ChannelService.SetDigitalInput(AuthService.ipaddress, AuthService.sessionid, digitalInput);
        if (response)
        {
            await Navigation.PopToRootAsync();
            MessagingCenter.Send<DigitalDetailsView>(this, "updated");
        }
        else
        {
            Console.WriteLine("Error setting channel data.");
        }
    }

}