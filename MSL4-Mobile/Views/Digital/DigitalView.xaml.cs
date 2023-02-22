using MSL4_Mobile.Views;
using MSL4_Mobile.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MSL4_Mobile.Views;

public partial class DigitalView : ContentPage
{
	public ObservableCollection<DigitalInput> digitalInputs { get; }
	public ObservableCollection<DigitalOutput> digitalOutputs { get;  }

	public DigitalView()
	{
		InitializeComponent();

		digitalInputs = new ObservableCollection<DigitalInput>();
		digitalOutputs = new ObservableCollection<DigitalOutput>();

		GetDigitalInputs();
		GetDigitalOutputs();
	}

	private async void GetDigitalInputs()
	{
		Console.WriteLine("Getting Digital Inputs.");
		List<DigitalInput> digitalInputs = await ChannelService.GetDigitalInputs(AuthService.ipaddress, AuthService.sessionid);
		digitalInputs.ForEach(d => this.digitalInputs.Add(d));
		OnPropertyChanged(nameof(digitalInputs));
		return;
	}

	private async void GetDigitalOutputs()
	{
		Console.WriteLine("Getting Digital Outputs.");
		List<DigitalOutput> digitalOutputs = await ChannelService.GetDigitalOutputs(AuthService.ipaddress, AuthService.sessionid);
		digitalOutputs.ForEach(d => this.digitalOutputs.Add(d));
		OnPropertyChanged(nameof(digitalOutputs));
		return;
	}
}
