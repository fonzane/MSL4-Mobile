﻿using MSL4_Mobile.Services;
using System.Collections.ObjectModel;

namespace MSL4_Mobile.Views;

public partial class AnalogView : ContentPage
{

	private ChannelService channelService;

	public ObservableCollection<AnalogInput> analogInputs { get; }
	public ObservableCollection<AnalogOutput> analogOutputs { get; }

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
	}

	private async void GetAnalogInputs()
	{
		Console.WriteLine("Getting Analog Inputs.");
		List<AnalogInput> analogInputs = await channelService.GetAnalogInputs(AuthService.ipaddress, AuthService.sessionid);
		this.analogInputs.Clear();
		analogInputs.ForEach(a => this.analogInputs.Add(a));
		OnPropertyChanged(nameof(this.analogInputs));
		return;
	}

	private async void GetAnalogOutputs()
	{
		Console.WriteLine("Getting Analog Outputs.");
		List<AnalogOutput> analogOutputs = await channelService.GetAnalogOutputs(AuthService.ipaddress, AuthService.sessionid);
		this.analogOutputs.Clear();
		analogOutputs.ForEach(a => this.analogOutputs.Add(a));
		OnPropertyChanged(nameof(this.analogOutputs));
		return;
	}

    async void OnSelectAnalogInput(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		Console.WriteLine("Hellooo");
		await Navigation.PushAsync(new Analog.AnalogDetailsView(e.CurrentSelection.FirstOrDefault() as AnalogInput));
		if(DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
		{
			(sender as ListView).SelectedItem = null;
		}
    }
}
