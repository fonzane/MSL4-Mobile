using MSL4_Mobile.Views;
using MSL4_Mobile.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MSL4_Mobile.Views;

public partial class DigitalView : ContentPage
{
	private ChannelService channelService;

	public ObservableCollection<DigitalInput> digitalInputs { get; }
	public ObservableCollection<DigitalOutput> digitalOutputs { get;  }

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

	private async void GetDigitalInputs()
	{
		Console.WriteLine("Getting Digital Inputs.");
		List<DigitalInput> digitalInputs = await channelService.GetDigitalInputs(AuthService.ipaddress, AuthService.sessionid);
		this.digitalInputs.Clear();
		digitalInputs.ForEach(d => this.digitalInputs.Add(d));
		OnPropertyChanged(nameof(digitalInputs));
		return;
	}

	private async void GetDigitalOutputs()
	{
		Console.WriteLine("Getting Digital Outputs.");
		List<DigitalOutput> digitalOutputs = await channelService.GetDigitalOutputs(AuthService.ipaddress, AuthService.sessionid);
		this.digitalOutputs.Clear();
		digitalOutputs.ForEach(d => this.digitalOutputs.Add(d));
		OnPropertyChanged(nameof(digitalOutputs));
		return;
	}

    async void OnSelectDigitalInput(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
		await Navigation.PushAsync(new Digital.DigitalDetailsView(e.SelectedItem as DigitalInput));
        if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}
