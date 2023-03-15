using MSL4_Mobile.Services;
using System.Collections.ObjectModel;

namespace MSL4_Mobile.Views.MBus;

public partial class MBusView : ContentPage
{
	private Timer timer;
	private int invokeCount = 0;
	private string searchInitTimestamp;
	private MBusService mBusService;

	public bool loading { get; set; } = false;

	public ObservableCollection<ComDataResponse.ComData> comDataCol { get; } = new ObservableCollection<ComDataResponse.ComData>();
	public ObservableCollection<BaudDataResponse.BaudData> baudDataCol { get; } = new ObservableCollection<BaudDataResponse.BaudData>();
	public ObservableCollection<MBusDeviceData> mBusDevices { get; } = new ObservableCollection<MBusDeviceData>();

	public ComDataResponse.ComData selectedCom { get; set; }
	public BaudDataResponse.BaudData selectedBaudRate { get; set; }

	public MBusView()
	{
		InitializeComponent();
		mBusService = new MBusService();
		GetBaudData();
		GetComData();
		GetMBusDevices();
	}

	private async void MBusSearchTimer(Object stateinfo)
	{
		MBusSearchResponse mBusSearchResponse = await CheckMBusSearchStatus(searchInitTimestamp);
		Console.WriteLine(mBusSearchResponse.LogText);
		if (mBusSearchResponse.LogType == 2)
		{
			timer.Dispose();
			loading = false;
			OnPropertyChanged(nameof(loading));
		}
	}

	private async void InitializeMBusSearch()
	{
		Console.WriteLine("Initializing MBus search");
		searchInitTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        MBusSearchResponse mBusSearchResponse = await mBusService.InitializeMBusSearch(AuthService.ipaddress, AuthService.sessionid, selectedCom.id, selectedBaudRate.pLabel, searchInitTimestamp);

		if (mBusSearchResponse != null && mBusSearchResponse.LogType == 1)
		{
			Console.WriteLine(mBusSearchResponse.LogText);
			timer = new Timer(MBusSearchTimer, null, 1000, 1000);
			loading = true;
            OnPropertyChanged(nameof(loading));
        }
        else
		{
			Console.WriteLine("Error initializing MBus Search");
			Console.WriteLine(mBusSearchResponse.LogText);
		}
	}

	private async Task<MBusSearchResponse> CheckMBusSearchStatus(string initialTimestamp)
	{
		MBusSearchResponse mBusSearchResponse = await mBusService.CheckMBusSearchStatus(AuthService.ipaddress, AuthService.sessionid, initialTimestamp);
		return mBusSearchResponse;
	}

	private async void GetMBusDevices()
	{
		Console.WriteLine("Getting MBus Devices");
        MBusDevicesResponse mBusDevicesResponse = await mBusService.GetMBusDevices(AuthService.ipaddress, AuthService.sessionid);
		foreach(MBusDeviceData mBusDevice in mBusDevicesResponse.items)
		{
			mBusDevices.Add(mBusDevice);
		}
    }

	private async void GetComData()
	{
		Console.WriteLine("Getting ComData");
		ComDataResponse comData = await mBusService.GetComData(AuthService.ipaddress, AuthService.sessionid);
		Console.WriteLine(comData.identifier + " - " + comData.label);
		foreach(ComDataResponse.ComData data in comData.items)
		{
			comDataCol.Add(data);
		}
		selectedCom = comDataCol[0];
	}

    private async void GetBaudData()
    {
        Console.WriteLine("Getting BaudData");
        BaudDataResponse baudData = await mBusService.GetBaudData(AuthService.ipaddress, AuthService.sessionid);
        Console.WriteLine(baudData.identifier + " - " + baudData.label);
        foreach (BaudDataResponse.BaudData data in baudData.items)
        {
			baudDataCol.Add(data);
        }
		selectedBaudRate = baudDataCol.FirstOrDefault(b => b.id == 2400);
    }

    async void OnStartMBusSearch(System.Object sender, System.EventArgs e)
    {
		if (selectedCom.id == "-")
		{
			await DisplayAlert("Kein Interface", "Bitte eine Schnittstelle auswählen", "OK");
			return;
		}
		InitializeMBusSearch();
    }

    async void OnSelectMBusDevice(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		MBusDeviceData mBusDeviceData = e.CurrentSelection.FirstOrDefault() as MBusDeviceData;
		selectedCom = comDataCol.FirstOrDefault(c => c.id == mBusDeviceData.pPortName, comDataCol[0]);

        await Navigation.PushAsync(new MBus.MBusDetailsView(
			mBusDeviceData.id,
			new List<ComDataResponse.ComData>(comDataCol),
			new List<BaudDataResponse.BaudData>(baudDataCol),
			selectedCom, selectedBaudRate
			)
		);
    }
}
