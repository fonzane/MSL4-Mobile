using MSL4_Mobile.Services;
using System.Collections.ObjectModel;

namespace MSL4_Mobile.Views;

public partial class AnalogView : ContentPage
{

	public ObservableCollection<AnalogInput> analogInputs { get; }
	public ObservableCollection<AnalogOutput> analogOutputs { get; }

	public AnalogView()
	{
		InitializeComponent();

		analogInputs = new ObservableCollection<AnalogInput>();
		analogOutputs = new ObservableCollection<AnalogOutput>();

		GetAnalogInputs();
		GetAnalogOutputs();
	}

	private async void GetAnalogInputs()
	{
		Console.WriteLine("Getting Analog Inputs.");
		List<AnalogInput> analogInputs = await ChannelService.GetAnalogInputs(AuthService.ipaddress, AuthService.sessionid);
		analogInputs.ForEach(a => this.analogInputs.Add(a));
		OnPropertyChanged(nameof(this.analogInputs));
		return;
	}

	private async void GetAnalogOutputs()
	{
		Console.WriteLine("Getting Analog Outputs.");
		List<AnalogOutput> analogOutputs = await ChannelService.GetAnalogOutputs(AuthService.ipaddress, AuthService.sessionid);
		analogOutputs.ForEach(a => this.analogOutputs.Add(a));
		OnPropertyChanged(nameof(this.analogOutputs));
		return;
	}

    public async void OnSelectAnalogInput(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
		(sender as ListView).SelectedItem = null;
		await Navigation.PushAsync(new Analog.AnalogDetailsView(e.SelectedItem as AnalogInput));
    }
}
