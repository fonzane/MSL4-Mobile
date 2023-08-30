using MSL4_Mobile.Views;
using MSL4_Mobile.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MSL4_Mobile.Views;

public partial class DigitalView : ContentPage
{
	private ChannelService channelService;

	public ObservableCollection<DigitalInput> digitalInputs { get; }
	public ObservableCollection<DigitalOutput> digitalOutputs { get; }

	public int digitalInputsHeight { get; set; }
	public int digitalOutputsHeight { get; set; }

	public DigitalView()
	{
		InitializeComponent();
		channelService = new ChannelService();

		digitalInputs = new ObservableCollection<DigitalInput>();
		digitalOutputs = new ObservableCollection<DigitalOutput>();

		GetDigitalInputs();
		GetDigitalOutputs();

        MessagingCenter.Subscribe<Digital.DigitalDetailsView>(this, "updated", sender =>
        {
            GetDigitalInputs();
        });
    }

	public async void GetDigitalInputs()
	{
		Console.WriteLine("Getting Digital Inputs.");
		List<DigitalInput> digitalInputs = await channelService.GetDigitalInputs(AuthService.ipaddress, AuthService.sessionid);
		this.digitalInputs.Clear();
		digitalInputs.ForEach(d => this.digitalInputs.Add(d));
		OnPropertyChanged(nameof(digitalInputs));

		if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
			digitalInputsHeight = digitalInputs.Count * 60;
		else
			digitalInputsHeight = digitalInputs.Count * 90;
		OnPropertyChanged(nameof(digitalInputsHeight));
        return;
	}

	public async void GetDigitalOutputs()
	{
		Console.WriteLine("Getting Digital Outputs.");
		List<DigitalOutput> digitalOutputs = await channelService.GetDigitalOutputs(AuthService.ipaddress, AuthService.sessionid);
		this.digitalOutputs.Clear();
		digitalOutputs.ForEach(d => this.digitalOutputs.Add(d));
		OnPropertyChanged(nameof(digitalOutputs));

        if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            digitalOutputsHeight = digitalOutputs.Count * 60;
        else
            digitalOutputsHeight = digitalOutputs.Count * 90;
        OnPropertyChanged(nameof(digitalOutputsHeight));
        return;
	}

    async void OnSelectDigitalInput(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        Console.WriteLine("Navigating Digital Input");
        await Navigation.PushAsync(new Digital.DigitalDetailsView(e.CurrentSelection.FirstOrDefault() as DigitalInput));
        if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
			(sender as CollectionView).SelectedItem = null;
        }
    }

    void OnSelectDigitalOutput(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		Console.WriteLine((e.CurrentSelection.FirstOrDefault() as DigitalOutput).modified);
		(e.CurrentSelection.FirstOrDefault() as DigitalOutput).modified = true;
        Console.WriteLine((e.CurrentSelection.FirstOrDefault() as DigitalOutput).modified);
    }

    async void OnSaveDigitalOutputs(System.Object sender, System.EventArgs e)
    {
		foreach (DigitalOutput channel in digitalOutputs)
		{
			bool response = await channelService.SetDigitalOutput(AuthService.ipaddress, AuthService.sessionid, channel);
			if (response)
			{
				await Navigation.PopToRootAsync();
			}
			else
			{
				Console.WriteLine("Error setting channel data.");
				await DisplayAlert("Error", "Couldn't set data for digital output " + channel.pName, "OK");
			}
        }
    }

    void OnReadDigitalInputs(System.Object sender, System.EventArgs e)
    {
		GetDigitalInputs();
    }

    void OnReadDigitalOutputs(System.Object sender, System.EventArgs e)
    {
		Console.WriteLine("HELLO");
		GetDigitalOutputs();
    }
}
