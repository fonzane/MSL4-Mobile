﻿using MSL4_Mobile.Services;

namespace MSL4_Mobile.Views.Status;

public partial class StatusView : ContentPage
{
	public MSL4StatusData statusData { get; set; }
	private StatusService _StatusService;

	public StatusView()
	{
		InitializeComponent();
		_StatusService = new StatusService();
		GetMSL4Status();
	}

	// ToDo: Modem Statistiken müssen noch in die View implementiert werden.
	private async void GetMSL4Status()
	{
		Console.WriteLine("Getting MSL4 StatusData...");
		statusData = await _StatusService.GetMSL4Status(AuthService.ipaddress, AuthService.sessionid);
		OnPropertyChanged(nameof(statusData));
		Console.WriteLine("Status Data fetched for " + statusData.name);
	}
}
