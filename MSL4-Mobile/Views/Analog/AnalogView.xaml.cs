using MSL4_Mobile.Services;
using System.Collections.ObjectModel;

namespace MSL4_Mobile.Views;

public partial class AnalogView : ContentPage
{

	private ChannelService channelService;

	public ObservableCollection<AnalogInput> analogInputs { get; }
	public ObservableCollection<AnalogOutput> analogOutputs { get; }

	public int analogInputsHeight { get; set; }
	public int analogOutputsHeight { get; set; }

    public AnalogView()
	{
		InitializeComponent();
		channelService = new ChannelService();

		analogInputs = new ObservableCollection<AnalogInput>();
		analogOutputs = new ObservableCollection<AnalogOutput>();

		GetAnalogInputs();
		GetAnalogOutputs();

		MessagingCenter.Subscribe<Analog.AnalogDetailsView>(this, "updated", sender =>
		{
			GetAnalogInputs();
		});
        MessagingCenter.Subscribe<Analog.AnalogOutDetailsView>(this, "updated", sender =>
        {
            GetAnalogOutputs();
        });
    }

	private async void GetAnalogInputs()
	{
		Console.WriteLine("Getting Analog Inputs.");
		List<AnalogInput> analogInputs = await channelService.GetAnalogInputs(AuthService.ipaddress, AuthService.sessionid);
		this.analogInputs.Clear();
		analogInputs.ForEach(a => this.analogInputs.Add(a));
		OnPropertyChanged(nameof(this.analogInputs));

        if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            analogInputsHeight = analogInputs.Count * 60;
        else
            analogInputsHeight = analogInputs.Count * 90;
        OnPropertyChanged(nameof(analogInputsHeight));
        return;
	}

	private async void GetAnalogOutputs()
	{
		Console.WriteLine("Getting Analog Outputs.");
		List<AnalogOutput> analogOutputs = await channelService.GetAnalogOutputs(AuthService.ipaddress, AuthService.sessionid);
		this.analogOutputs.Clear();
		analogOutputs.ForEach(a => this.analogOutputs.Add(a));
		OnPropertyChanged(nameof(this.analogOutputs));

        if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            analogOutputsHeight = analogOutputs.Count * 60;
        else
            analogOutputsHeight = analogOutputs.Count * 90;
        OnPropertyChanged(nameof(analogOutputsHeight));
        return;
	}

    async void OnSelectAnalogInput(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		Console.WriteLine("Hellooo");
		await Navigation.PushAsync(new Analog.AnalogDetailsView(e.CurrentSelection.FirstOrDefault() as AnalogInput));
		if(DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
		{
			(sender as CollectionView).SelectedItem = null;
		}
    }

    async void OnSelectAnalogOutput(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		await Navigation.PushAsync(new Analog.AnalogOutDetailsView(e.CurrentSelection.FirstOrDefault() as AnalogOutput));
        if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            (sender as CollectionView).SelectedItem = null;
        }
    }

    void OnGetAnalogInputs(System.Object sender, System.EventArgs e)
    {
		GetAnalogInputs();
    }

    void OnGetAnalogOutputs(System.Object sender, System.EventArgs e)
    {
		GetAnalogOutputs();
    }
}
